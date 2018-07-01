using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Devart.Data.MySql;

namespace TechTreeEditor
{
    public partial class GraphView : Form, IObserver<uint>
    {
        //*********************************************************************
        //************ Data Structures and their initializors *****************
        //*********************************************************************
        //General
        private Graphics graphics;
        private TechListView techListView;
        private MySqlConnection connection;
        private DateTime lastView;
        private const double MIN_VIEWTECH_INTERVAL = 1.0; //seconds

        //Graphics
        public class GraphNodeStyle
        {
            public Point[] points;
            public Pen borderPen;
            public SolidBrush fillBrush;
            public Font font;
            public SolidBrush textBrush;
            public bool ellipse = false;
            //Edge connection vertex locations
            public Dictionary<string, Point> connectors;

            //Draws the node onto the graphics object
            public void Draw(ref Graphics g, Point location, string label, bool permanizes = false)
            {
                if (ellipse)
                {
                    Rectangle rect = new Rectangle(location.X - 50, location.Y - 23, 100, 46);
                    //Draw fill
                    g.FillEllipse(fillBrush, rect);
                    //Draw border
                    g.DrawEllipse(borderPen, rect);
                }
                else
                {
                    Point[] vertices = new Point[points.Length];
                    //compute vertices
                    for (int i = 0; i < points.Length; i++)
                    {
                        vertices[i].X = location.X + points[i].X;
                        vertices[i].Y = location.Y + points[i].Y;
                    }
                    //Draw fill
                    g.FillPolygon(fillBrush, vertices);
                    //Draw border
                    g.DrawPolygon(borderPen, vertices);
                }

                //Draw text
                DrawLabel(ref g, label, location);

                //If permanizes, draw a P on the upper right of the node
                if (permanizes)
                {
                    Point P = new Point(location.X + 51, location.Y - 30);
                    g.DrawString("P", font, textBrush, P);
                }
            }

            //Draws a label with text wrapping
            private void DrawLabel(ref Graphics g, string label, Point location)
            {
                const int minChar = 6;
                const int maxChar = 16;
                Point offset = new Point(0, 0);
                if (label.Length <= maxChar)
                {
                    //one line
                    offset.X -= (label.Length * 4 - 15);
                    offset.Y -= 6;
                }
                else 
                {
                    //two or more lines
                    label = CenterJustify(WrapText(label, minChar, maxChar));
                    int numLines = CountLines(label);
                    int maxChars = CountMaxCharLine(label);
                    offset.X -= (maxChars * 4 - 21);
                    offset.Y -= (5 + numLines * 8);
                }
                location.X = location.X + offset.X;
                location.Y = location.Y + offset.Y;
                g.DrawString(label, font, textBrush, location);
            }
            //Wraps a string according to minimum and maximum characters on a line
            private string WrapText(string input, int minChar, int maxChar)
            {
                string upper = "", lower = "";
                int brk = 0;
                if (input.Substring(minChar, maxChar - minChar).Contains(" "))
                {
                    brk = input.Substring(minChar, maxChar - minChar).IndexOf(' ') + minChar;
                    upper = input.Substring(0, brk);
                    lower = input.Substring(brk + 1);
                }
                else
                {
                    brk = maxChar;
                    upper = input.Substring(0, brk);
                    lower = input.Substring(brk);
                }
                if (lower.Length > maxChar)
                    lower = WrapText(lower, minChar, maxChar);
                return (upper + "\r\n" + lower);
            }
            //finds the highest number of characters on a line
            private int CountMaxCharLine(string input)
            {
                int current = 0;
                int max = 0;
                int i = 0;
                while (i < input.Length)
                {
                    if (input.Substring(i,1) == "\r")
                    {
                        i++;
                        current = 0;
                    }
                    else
                    {
                        current++;
                        if (current > max) max = current;
                    }
                    i++;
                }
                return max;
            }
            //Counts the number of lines in a string
            private int CountLines(string input)
            {
                int count = 0;
                while (input.IndexOf('\r') != -1)
                {
                    count++;
                    input = input.Substring(input.IndexOf('\r') + 2);
                }
                return count;
            }
            //Center justifies multiline text
            private string CenterJustify(string input)
            {
                int width = CountMaxCharLine(input);
                int i = 0;
                string line = "", output = "";
                while (i < input.Length)
                {
                    if (input.Substring(i, 1) == "\r")
                    {
                        //new line found. Process old line.
                        while (line.Length < width)
                        {
                            line = " " + line + " ";
                        }
                        output += line + "\r\n";
                        //Done processing. Get next line.
                        i++;
                        line = "";
                    }
                    else
                    {
                        //Add to line
                        line += input.Substring(i, 1);
                    }
                    i++;
                }
                //Process last line
                while (line.Length < width)
                {
                    line = " " + line + " ";
                }
                output += line;
                return output;
            }
            //Releases assets
            public void Dispose() { 
                borderPen.Dispose();
                fillBrush.Dispose();
                font.Dispose();
                textBrush.Dispose();
            }
        }
        private GraphNodeStyle Foundational;
        private GraphNodeStyle Topical;
        private GraphNodeStyle Specialized;
        private GraphNodeStyle Technology;
        private GraphNodeStyle Archival;
        public Dictionary<string, GraphNodeStyle> nodeStyles;
        private Pen PrereqEdgeStyle;
        private Pen GrantreqEdgeStyle;
        private void InitializeStyles()
        {
            //Foundational is a blue rectangle with bold black border
            Foundational = new GraphNodeStyle();
            Foundational.points = new Point[4];
            Foundational.points[0] = new Point(-50, 23);
            Foundational.points[1] = new Point(50, 23);
            Foundational.points[2] = new Point(50, -23);
            Foundational.points[3] = new Point(-50, -23);
            Foundational.fillBrush = new SolidBrush(Color.LightSkyBlue);
            Foundational.font = new Font("Tahoma", 8f, FontStyle.Regular);
            Foundational.borderPen = new Pen(Color.Black, 2f);
            Foundational.textBrush = new SolidBrush(Color.Black);
            Foundational.connectors = new Dictionary<string, Point>();
            Foundational.connectors.Add("left", new Point(-50, 0));
            Foundational.connectors.Add("right", new Point(50, 0));
            Foundational.connectors.Add("top", new Point(0, -23));
            Foundational.connectors.Add("bottom", new Point(0, 23));

            //Topical is a salmon parallelogram with a regular black border
            Topical = new GraphNodeStyle();
            Topical.points = new Point[4];
            Topical.points[0] = new Point(-46, 23);
            Topical.points[1] = new Point(54, 23);
            Topical.points[2] = new Point(46, -23);
            Topical.points[3] = new Point(-54, -23);
            Topical.fillBrush = new SolidBrush(Color.LightSalmon);
            Topical.font = new Font("Tahoma", 8f, FontStyle.Regular);
            Topical.textBrush = new SolidBrush(Color.Black);
            Topical.borderPen = new Pen(Color.Black, 1f);
            Topical.connectors = new Dictionary<string, Point>();
            Topical.connectors.Add("left", new Point(-50, 0));
            Topical.connectors.Add("right", new Point(50, 0));
            Topical.connectors.Add("top", new Point(0, -23));
            Topical.connectors.Add("bottom", new Point(0, 23));

            //Specialized is a green 6-pointed star-like shape with a regular black border
            Specialized = new GraphNodeStyle();
            Specialized.points = new Point[12];
            Specialized.points[0] = new Point(-15, 21);
            Specialized.points[1] = new Point(0, 25);
            Specialized.points[2] = new Point(15, 21);
            Specialized.points[3] = new Point(52, 21);
            Specialized.points[4] = new Point(46, 0);
            Specialized.points[5] = new Point(52, -21);
            Specialized.points[6] = new Point(15, -21);
            Specialized.points[7] = new Point(0, -25);
            Specialized.points[8] = new Point(-15, -21);
            Specialized.points[9] = new Point(-52, -21);
            Specialized.points[10] = new Point(-46, 0);
            Specialized.points[11] = new Point(-52, 21);
            Specialized.fillBrush = new SolidBrush(Color.LightGreen);
            Specialized.font = new Font("Tahoma", 8f, FontStyle.Regular);
            Specialized.textBrush = new SolidBrush(Color.Black);
            Specialized.borderPen = new Pen(Color.Black, 1f);
            Specialized.connectors = new Dictionary<string, Point>();
            Specialized.connectors.Add("left", new Point(-46, 0));
            Specialized.connectors.Add("right", new Point(46, 0));
            Specialized.connectors.Add("top", new Point(0, -25));
            Specialized.connectors.Add("bottom", new Point(0, 25));

            //Technology is yellow ellipse with a regular black border
            Technology = new GraphNodeStyle();
            Technology.ellipse = true;
            Technology.fillBrush = new SolidBrush(Color.LemonChiffon);
            Technology.font = new Font("Tahoma", 8f, FontStyle.Regular);
            Technology.textBrush = new SolidBrush(Color.Black);
            Technology.borderPen = new Pen(Color.Black, 1f);
            Technology.connectors = new Dictionary<string, Point>();
            Technology.connectors.Add("left", new Point(-50, 0));
            Technology.connectors.Add("right", new Point(50, 0));
            Technology.connectors.Add("top", new Point(0, 23));
            Technology.connectors.Add("bottom", new Point(0, -23));

            //Archival is a gray ellipse with a white border
            Archival = new GraphNodeStyle();
            Archival.ellipse = true;
            Archival.fillBrush = new SolidBrush(Color.LightGray);
            Archival.font = new Font("Tahoma", 8f, FontStyle.Regular);
            Archival.textBrush = new SolidBrush(Color.Black);
            Archival.borderPen = new Pen(Color.White, 1f);
            Archival.connectors = new Dictionary<string, Point>();
            Archival.connectors.Add("left", new Point(-50, 0));
            Archival.connectors.Add("right", new Point(50, 0));
            Archival.connectors.Add("top", new Point(0, -23));
            Archival.connectors.Add("bottom", new Point(0, 23));

            //Prereq edge style is a solid black arrow
            PrereqEdgeStyle = new Pen(Color.Black, 1f);
            AdjustableArrowCap cap = new AdjustableArrowCap(5f, 5f, true);
            cap.BaseInset = 5; 
            cap.StrokeJoin = LineJoin.Bevel;
            PrereqEdgeStyle.EndCap = LineCap.Custom;
            PrereqEdgeStyle.CustomEndCap = cap;
            PrereqEdgeStyle.Width = 2f;

            //Grantreq edge style is a yellow-green dashed arrow
            GrantreqEdgeStyle = new Pen(Color.YellowGreen, 1f);
            GrantreqEdgeStyle.EndCap = LineCap.Custom;
            GrantreqEdgeStyle.CustomEndCap = cap;
            GrantreqEdgeStyle.DashStyle = DashStyle.Dash;
            GrantreqEdgeStyle.Width = 2f;

            //Initialize dictionary
            nodeStyles = new Dictionary<string, GraphNodeStyle>();
            nodeStyles.Add("Foundational", Foundational);
            nodeStyles.Add("Topical", Topical);
            nodeStyles.Add("Specialized", Specialized);
            nodeStyles.Add("Technologies", Technology);
            nodeStyles.Add("Archival", Archival);
        }
        //Holds a node
        private class GraphNode
        {
            public uint id;
            public string name;
            public string category;
            private Point location;
            public Point Location
            {
                get
                {
                    return location;
                }
                set
                {
                    location = value;
                    boundingBox = new Rectangle(location.X - 50, location.Y - 23, 100, 46);
                }
            }
            private GraphNodeStyle renderer;
            public Rectangle boundingBox;
            private bool valid = false;
            public static MySqlConnection connection;
            public static TechListView techListView;
            public bool permanizes = false;

            //Creates an empty graph node
            public GraphNode() { }
            //Creates a graphnode and loads the tech
            public GraphNode(uint techID, ref Dictionary<string, GraphNodeStyle> styles)
            {
                Load(techID, ref styles);
            }
            
            //Releases assets
            public void Dispose()
            {
                
            }

            //Renders the node
            public void Draw(ref Graphics g, Point loc)
            {
                Location = loc;
                renderer.Draw(ref g, loc, name, permanizes);
            }

            //Gets connection vertex by name
            public Point GetConnector(string key)
            {
                Point result = new Point(0,0);
                Point C;
                try
                {
                    C = renderer.connectors[key];
                    result.X = Location.X + C.X;
                    result.Y = Location.Y + C.Y;
                }
                catch (KeyNotFoundException) { }
                return result;
            }

            //Fetches name and category from database
            public bool Load(uint techID, ref Dictionary<string, GraphNodeStyle> styles)
            {
                bool success = true;
                id = techID;
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT name, category FROM tech " +
                    "WHERE id=" + id + ";";
                connection.Open();
                MySqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        name = reader.GetString(0);
                        category = reader.GetString(1);
                        renderer = styles[category];
                    }
                    else success = false;
                }
                catch (MySqlException ex)
                {
                    techListView.Log("An error occurred while loading graph node " +
                        "information for id " + id + ": " +
                        ex.Message,command.CommandText);
                    success = false;
                }
                catch (KeyNotFoundException)
                {
                    techListView.Log("Error: category '" + category + "' not found in " +
                        "GraphNodeStyles dictionary.");
                    success = false;
                }
                finally
                {
                    connection.Close();
                }
                valid = success;
                return success;
            }
        }
        //Holds an edge
        private class GraphEdge
        {
            public Point edgeStart;
            public Point edgeEnd;
            public Pen edgeStyle;

            public GraphEdge(Pen style)
            {
                edgeStyle = style;
            }
            public void Draw(ref Graphics g)
            {
                Draw(ref g, edgeStart, edgeEnd);
            }
            public void Draw(ref Graphics g, Point start, Point end)
            {
                edgeStart = start;
                edgeEnd = end;
                g.DrawLine(edgeStyle, start, end);
            }
            
        }

        //Graph data
        private uint techID;
        private List<uint> prereqs; //prereqs of selected
        private List<uint> grantreqs; //grantreqs of selected
        private List<uint> prereqFor; //selected is prereq for
        private List<uint> grantreqFor; //selected is grantreq for
        private List<uint> permanizedBy; //selected is permanized by
        private List<GraphNode> nodes; //holds the primary reference to all nodes
        GraphNode centerNode; //holds an additional reference to the center node
        private List<GraphNode> prereqNodes; //holds additional references to the prereq nodes
        private List<GraphNode> grantreqNodes; //holds additional references to the grantreqs nodes
        private List<GraphNode> prereqForNodes; //holds additional references to the prereqFor nodes
        private List<GraphNode> grantreqForNodes; //holds additional references to the grantreqFor nodes
        private List<GraphEdge> edges;

        //*********************************************************************
        //************************** Basic Functions **************************
        //*********************************************************************
        
        public GraphView(TechListView listView)
        {
            InitializeComponent();
            InitializeStyles();
            connection = new MySqlConnection(Properties.Settings.Default.dbConnectionString);
            GraphNode.connection = connection;
            techListView = listView;
            GraphNode.techListView = techListView;
            techID = 0;
            prereqs = new List<uint>();
            grantreqs = new List<uint>();
            prereqFor = new List<uint>();
            grantreqFor = new List<uint>();
            permanizedBy = new List<uint>();
            nodes = new List<GraphNode>();
            centerNode = null;
            prereqNodes = new List<GraphNode>();
            grantreqNodes = new List<GraphNode>();
            prereqForNodes = new List<GraphNode>();
            grantreqForNodes = new List<GraphNode>();
            edges = new List<GraphEdge>();
            techListView.Subscribe(this);
            graphics = picBox.CreateGraphics();
            lastView = DateTime.Now;
    }
        ~GraphView()
        {
            Clear();
            graphics.Dispose();
            Foundational.Dispose();
            Topical.Dispose();
            Specialized.Dispose();
            Technology.Dispose();
            Archival.Dispose();
            PrereqEdgeStyle.Dispose();
            GrantreqEdgeStyle.Dispose();
        }

        //*********************************************************************
        //******************************* Tasks *******************************
        //*********************************************************************

        //Focuses the graph view on the given id
        public void ViewTech(uint id)
        {
            if ((DateTime.Now - lastView).TotalSeconds < MIN_VIEWTECH_INTERVAL) return;
            lastView = DateTime.Now;
            Clear();
            ClearData();
            techID = id;
            if (!(GetPrereqs() & GetGrantreqs() & GetPrereqFor() & GetGrantreqFor() & GetPermanizedBy()))
            {
                techListView.Log("An error occurred while fetching data " +
                    "for the local graph view. See log file for details. " +
                    "Continuing execution and display of available data.");
            }
            nodes.Add(new GraphNode(techID, ref nodeStyles));
            centerNode = nodes[0];
            foreach (uint n in prereqs)
            {
                nodes.Add(new GraphNode(n, ref nodeStyles));
                prereqNodes.Add(nodes[nodes.Count - 1]);
                if (permanizedBy.Contains(n))
                    nodes[nodes.Count - 1].permanizes = true;
            }
            foreach (uint n in grantreqs)
            {
                nodes.Add(new GraphNode(n, ref nodeStyles));
                grantreqNodes.Add(nodes[nodes.Count - 1]);
                if (permanizedBy.Contains(n))
                    nodes[nodes.Count - 1].permanizes = true;
            }
            foreach (uint n in prereqFor)
            {
                nodes.Add(new GraphNode(n, ref nodeStyles));
                prereqForNodes.Add(nodes[nodes.Count - 1]);
                if (permanizedBy.Contains(n))
                    nodes[nodes.Count - 1].permanizes = true;
            }
            foreach (uint n in grantreqFor)
            {
                nodes.Add(new GraphNode(n, ref nodeStyles));
                grantreqForNodes.Add(nodes[nodes.Count - 1]);
                if (permanizedBy.Contains(n))
                    nodes[nodes.Count - 1].permanizes = true;
            }
            DrawAll();
        }

        //draws all the nodes and edges
        public void DrawAll()
        {
            DrawCenterNode();
            DrawPrereqs();
            DrawPrereqFor();
            DrawGrantreqs();
            DrawGrantreqFor();
        }

        //Clears all data
        public void ClearData()
        {
            techID = 0;
            prereqs.Clear();
            grantreqs.Clear();
            prereqFor.Clear();
            grantreqFor.Clear();
            permanizedBy.Clear();
            for (int i = 0; i < nodes.Count; i++)
                nodes[i].Dispose();
            nodes.Clear();
            centerNode = null;
            prereqNodes.Clear();
            grantreqNodes.Clear();
            prereqForNodes.Clear();
            grantreqForNodes.Clear();
            edges.Clear();
        }

        //Clears the graph view
        public void Clear()
        {
            graphics.Clear(Color.White);
            graphics.Dispose();
            graphics = picBox.CreateGraphics();
        }

        //*********************************************************************
        //***************** Graph Arrangement and Rendering *******************
        //*********************************************************************

        //Transforms center-based coordinates to top-left coordinates
        private Point FromCenter(int x, int y)
        {
            //Gavin: I adjusted this a little
            return new Point(x + ((this.Width - 50) / 2), y + ((this.Height - 50) / 2));
        }

        //Draws the center node
        private void DrawCenterNode()
        {
            if (centerNode != null)
                centerNode.Draw(ref graphics, FromCenter(0, 0));
        }

        //Draws prerequisite nodes
        private void DrawPrereqs()
        {
            int x = -175;
            int y = -27 * (prereqNodes.Count - 1);
            for (int i = 0; i < prereqNodes.Count; i++)
            {
                prereqNodes[i].Draw(ref graphics, FromCenter(x, y));
                edges.Add(new GraphEdge(PrereqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    prereqNodes[i].GetConnector("right"),
                    centerNode.GetConnector("left"));
                y += 54;
            }
        }

        //Draws prereqFor nodes
        private void DrawPrereqFor()
        {
            int x = 175;
            int y = -27 * (prereqForNodes.Count - 1);
            for (int i = 0; i < prereqForNodes.Count; i++)
            {
                prereqForNodes[i].Draw(ref graphics, FromCenter(x, y));
                edges.Add(new GraphEdge(PrereqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    centerNode.GetConnector("right"),
                    prereqForNodes[i].GetConnector("left"));
                y += 54;
            }
        }

        //Draws grantrequisite nodes
        private void DrawGrantreqs()
        {
            //Draw first grantreq on the upper top right
            if (grantreqNodes.Count > 0)
            {
                grantreqNodes[0].Draw(ref graphics, FromCenter(55, -125));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    grantreqNodes[0].GetConnector("bottom"),
                    centerNode.GetConnector("top"));
            }
            else return;
            //Draw second grantreq on the upper top left
            if (grantreqNodes.Count > 1)
            {
                grantreqNodes[1].Draw(ref graphics, FromCenter(-55, -125));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    grantreqNodes[1].GetConnector("bottom"),
                    centerNode.GetConnector("top"));
            }
            else return;
            //If only one more, draw it on upper top center in triangle shape
            if (grantreqNodes.Count == 3)
            {
                grantreqNodes[2].Draw(ref graphics, FromCenter(0, -180));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    grantreqNodes[2].GetConnector("bottom"),
                    centerNode.GetConnector("top"));
            }
            //If more, arrange rectangularly but don't draw any more edges
            else if (grantreqNodes.Count > 3)
            {
                for (int i = 3; i < grantreqNodes.Count; i++)
                {
                    grantreqNodes[i].Draw(ref graphics, FromCenter(
                            55 * (2 * (i % 2) - 1), 
                            -125 - 55 * ((i-1)/2)
                        ));
                }
            }
        }

        //Draws grantreqFor nodes
        private void DrawGrantreqFor()
        {
            //Draw first grantreq on the lower bottom left
            if (grantreqForNodes.Count > 0)
            {
                grantreqForNodes[0].Draw(ref graphics, FromCenter(-55, 125));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    centerNode.GetConnector("bottom"),
                    grantreqForNodes[0].GetConnector("top"));
            }
            else return;
            //Draw second grantreq on the lower bottom right
            if (grantreqForNodes.Count > 1)
            {
                grantreqForNodes[1].Draw(ref graphics, FromCenter(55, 125));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    centerNode.GetConnector("bottom"),
                    grantreqForNodes[1].GetConnector("top"));
            }
            else return;
            //If only one more, draw it on lower bottom center in triangle shape
            if (grantreqForNodes.Count == 3)
            {
                grantreqForNodes[2].Draw(ref graphics, FromCenter(0, 180));
                edges.Add(new GraphEdge(GrantreqEdgeStyle));
                edges[edges.Count - 1].Draw(ref graphics,
                    centerNode.GetConnector("bottom"),
                    grantreqForNodes[2].GetConnector("top"));
            }
            //If more, arrange rectangularly but don't draw any more edges
            else if (grantreqForNodes.Count > 3)
            {
                for (int i = 3; i < grantreqForNodes.Count; i++)
                {
                    grantreqForNodes[i].Draw(ref graphics, FromCenter(
                            55 * (2 * (i % 2) - 1),
                            125 + 55 * ((i - 1) / 2)
                        ));
                }
            }
        }

        //*********************************************************************
        //************************ Database Functions *************************
        //*********************************************************************

        private bool GetPrereqs()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT prereq_id FROM tech_prereqs " +
                "WHERE id=" + techID + ";";
            command.CommandTimeout = 2;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    prereqs.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.QuietLog("An error occurred while getting prereqs " +
                    "for the graph view of tech with id " + HexConverter.IntToHex(techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            return success;
        }
        private bool GetGrantreqs()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT grantreq_id FROM tech_grantreqs " +
                "WHERE id=" + techID + ";";
            command.CommandTimeout = 2;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    grantreqs.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.QuietLog("An error occurred while getting grantreqs " +
                    "for the graph view of tech with id " + HexConverter.IntToHex(techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            return success;
        }
        private bool GetPrereqFor()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id FROM tech_prereqs " +
                "WHERE prereq_id=" + techID + ";";
            command.CommandTimeout = 2;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    prereqFor.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.QuietLog("An error occurred while getting prereqFor " +
                    "for the graph view of tech with id " + HexConverter.IntToHex(techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            return success;
        }
        private bool GetGrantreqFor()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id FROM tech_grantreqs " +
                "WHERE grantreq_id=" + techID + ";";
            command.CommandTimeout = 2;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    grantreqFor.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.QuietLog("An error occurred while getting grantreqFor " +
                    "for the graph view of tech with id " + HexConverter.IntToHex(techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            return success;
        }
        private bool GetPermanizedBy()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id FROM tech_permanizes " +
                "WHERE permanizes_id=" + techID + ";";
            command.CommandTimeout = 2;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    permanizedBy.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.QuietLog("An error occurred while getting permanizedBy " +
                    "for the graph view of tech with id " + HexConverter.IntToHex(techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            return success;
        }

        //*********************************************************************
        //************************** Event Handlers ***************************
        //*********************************************************************

        //Redraws the graph in the center of the window when resized
        private void GraphView_ResizeEnd(object sender, EventArgs e)
        {
            Clear();
            DrawAll();
        }

        //Observer functions
        public void OnNext(uint id)
        {
            ViewTech(id);
        }
        public void OnError(Exception ex)
        {
            techListView.Log("An unknown error occurred in GraphView.OnError: " + ex.Message);
        }
        public void OnCompleted()
        {

        }
    }
}
