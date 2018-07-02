using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Data.MySql;
using System.Text.RegularExpressions;
using System.IO;


namespace TechTreeEditor
{
    public partial class TechListView : Form, Observable
    {
        //*********************************************************************
        //************************** Data Structures **************************
        //*********************************************************************
        private MySqlConnection connection;
        private List<TechEditView> openEditViews;
        private int nextEditViewID = 0;
        private struct FilterOptions
        {
            public bool idRangeActive, categoryActive, nameStringMatchActive, fieldNameActive;
            public uint idRangeMin, idRangeMax;
            public string category;
            public string nameString;
            public string fieldName;
        }
        private FilterOptions currentFilters;
        private Regex IDRgx; //matches if invalid hex string
        private GraphView graphView;

        //Returns the number of open tech edit views that are in Edit or Add mode
        public int NumberOfOpenEditingViews
        {
            get
            {
                int num = 0;
                for (int i = 0; i < openEditViews.Count; i++)
                {
                    if (openEditViews[i].Mode == TechEditView.ViewMode.EDITING ||
                        openEditViews[i].Mode == TechEditView.ViewMode.ADDING_NEW)
                        num++;
                }
                return num;
            }
        }

        //Returns the currently selected id, or 0 if none selected
        public uint SelectedTechID
        {
            get
            {
                if (TechListGrid.SelectedRows.Count == 0) return 0;
                return HexConverter.HexToInt(
                    TechListGrid.SelectedRows[0].Cells[0].Value as string);
            }
        }

        //Stores observer views of the currently selected tech
        private List<Observer> observers;

        //*********************************************************************
        //*************************** Basic Methods ***************************
        //*********************************************************************

        //Constructor and Destructor
        public TechListView()
        {
            InitializeComponent();
            connection = new MySqlConnection(Properties.Settings.Default.dbConnectionString);
            //connection.Open(); //changing to not always open
            openEditViews = new List<TechEditView>();
            //Set initial filter options
            currentFilters.categoryActive = false;
            currentFilters.idRangeActive = false;
            currentFilters.nameStringMatchActive = false;
            currentFilters.fieldNameActive = false;
            currentFilters.idRangeMin = 0;
            currentFilters.idRangeMax = 0xFFFFFFFF;
            currentFilters.nameString = "";
            currentFilters.fieldName = "";
            currentFilters.category = "";
            observers = new List<Observer>();
            LoadCategories();
            IDRgx = new Regex(@"[^0123456789ABCDEFabcdef]");
            OpenGraphView();
        }

        //Adds a message to the log
        public void Log(string message, string associatedCommand = "")
        {
            LogDisplay.AppendText(message + "\r\n");
            WriteToLogFile(message, associatedCommand);
        }
        //Adds a message to the log file but doesn't print to display
        public void QuietLog(string message, string associatedCommand = "")
        {
            WriteToLogFile(message, associatedCommand);
        }
        //Writes to the log file
        private void WriteToLogFile(string message, string associatedCommand = "")
        {
            string line = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            line += ": " + message;
            if (associatedCommand != "")
                line += "\r\n    Associated command: \"" + associatedCommand + "\"";
            StreamWriter fout = new StreamWriter(
                Directory.GetCurrentDirectory() + "\\log.txt", true);
            fout.WriteLine(line);
            fout.Close();
        }
        
        //*********************************************************************
        //************************** Form Operations **************************
        //*********************************************************************

        //Finds a tech in the current view and selects it. REturns true if selected
        public bool Select(uint id)
        {
            int rows = TechListGrid.Rows.Count;
            if (rows == 0) return false;
            int a = 0;
            int b = rows - 1;
            int c = (b - a) / 2;
            uint current = HexConverter.HexToInt(TechListGrid.Rows[c].Cells[0].Value as string);
            //Check last record
            if (HexConverter.HexToInt(TechListGrid.Rows[b].Cells[0].Value as string) == id)
            {
                current = id; //skips the loop
                c = b; //postcondition is c is target row
            }
            while ((current != id) && (b - a > 1))
            {
                current = HexConverter.HexToInt(TechListGrid.Rows[c].Cells[0].Value as string);
                if (id > current)
                {
                    //upper range selected for further search
                    a = c;
                    c = a + (b - a) / 2;
                }
                else if (id < current)
                {
                    //lower range selected for further search
                    b = c;
                    c = a + (b - a) / 2;
                }
            }
            if (id == current)
            {
                //Found. Select the tech found at row c
                TechListGrid.ClearSelection();
                TechListGrid.Rows[c].Selected = true;
                TechListGrid.FirstDisplayedScrollingRowIndex = c;
                return true;
            }
            return false;
        }

        //Open or closes a new tech edit view
        private void OpenEditView(TechEditView.ViewMode mode, uint id = 0)
        {
            openEditViews.Add(new TechEditView(nextEditViewID, id, mode));
            nextEditViewID++;
            openEditViews[openEditViews.Count - 1].techListView = this;
            openEditViews[openEditViews.Count - 1].Visible = true;
            int offset = (openEditViews.Count - 1) * 30;
            openEditViews[openEditViews.Count - 1].Left = this.Left + this.Width + offset;
            openEditViews[openEditViews.Count - 1].Top = this.Top + offset;
            openEditViews[openEditViews.Count - 1].Activate();
        }
        public void CloseEditView(int viewID)
        {
            int i = 0;
            while (i < openEditViews.Count && openEditViews[i].EditViewID != viewID)
                i++;
            if (i < openEditViews.Count)
            {
                openEditViews.RemoveAt(i);
                //TechEditView handles closing and disposing the form
            }
        }

        //Opens or closes the Graph View
        private void OpenGraphView()
        {
            if (graphView == null)
            {
                graphView = new GraphView(this);
                graphView.Visible = true;
            }
        }
        private void CloseGraphView()
        {
            if (graphView != null)
            {
                graphView.Close();
                graphView = null;
            }
        }

        //Adds the requested prereq to the requested form
        public void AddPrereqToEditForm(int viewIndex, uint id)
        {
            openEditViews[viewIndex].AddPrereq(id);
        }
        //Adds the requested grantreq to the requested form
        public void AddGrantreqToEditForm(int viewIndex, uint id)
        {
            openEditViews[viewIndex].AddGrantreq(id);
        }

        //Refreshes the list view
        public void RefreshList()
        {
            //Suppress notification to the graph view while refreshing
            graphView.suppressNotify = true;
            FetchTechList(currentFilters);
            graphView.suppressNotify = false;
        }

        //Add or remove observers
        public void AddObserver(Observer obs)
        {
            if (!observers.Contains(obs))
                observers.Add(obs);
        }
        public void RemoveObserver(Observer obs)
        {
            observers.Remove(obs);
        }


        //*********************************************************************
        //************************** Event Handlers ***************************
        //*********************************************************************

        private void InitializeDatabaseButton_Click(object sender, EventArgs e)
        {
            //Initializes the database
            InitializeDatabase();
        }
        private void ShowTablesButton_Click(object sender, EventArgs e)
        {
            List<string> tableNames = GetTableNames();
            foreach (string name in tableNames)
            {
                Log(name);
            }
        }
        private void DeleteTechButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedTech();
        }
        private void UpdateFiltersButton_Click(object sender, EventArgs e)
        {
            //Determine if filtering by ID Range
            if (IDRangeMinInput.Text != "00000000" || IDRangeMaxInput.Text != "FFFFFFFF")
            {
                currentFilters.idRangeActive = true;
                currentFilters.idRangeMin = HexConverter.HexToInt(IDRangeMinInput.Text);
                currentFilters.idRangeMax = HexConverter.HexToInt(IDRangeMaxInput.Text);
            }
            else
            {
                currentFilters.idRangeActive = false;
            }
            //Determine if filtering by Category
            if (CategoryComboBox.SelectedIndex >= 0)
            {
                currentFilters.categoryActive = true;
                currentFilters.category = CategoryComboBox.Items[
                    CategoryComboBox.SelectedIndex] as string;
            }
            else
            {
                currentFilters.categoryActive = false;
            }
            //Determine if filtering by name string match
            if (NameFilterInput.Text != "")
            {
                currentFilters.nameStringMatchActive = true;
                currentFilters.nameString = NameFilterInput.Text.Trim(' ');
            }
            else
            {
                currentFilters.nameStringMatchActive = false;
            }
            //Determine if filtering by field name
            if (FieldNameInput.Text != "")
            {
                currentFilters.fieldNameActive = true;
                currentFilters.fieldName = FieldNameInput.Text;
            }
            else
            {
                currentFilters.fieldNameActive = false;
            }
            
            RefreshList();
        }
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            IDRangeMinInput.Text = "00000000";
            IDRangeMaxInput.Text = "FFFFFFFF";
            CategoryComboBox.SelectedIndex = -1;
            NameFilterInput.Text = "";
            FieldNameInput.Text = "";
            currentFilters.categoryActive = false;
            currentFilters.idRangeActive = false;
            currentFilters.nameStringMatchActive = false;
            currentFilters.fieldNameActive = false;
            currentFilters.idRangeMin = 0;
            currentFilters.idRangeMax = 0xFFFFFFFF;
            currentFilters.nameString = "";
            currentFilters.fieldName = "";
            currentFilters.category = "";
        }
        private void AddTechButton_Click(object sender, EventArgs e)
        {
            OpenEditView(TechEditView.ViewMode.ADDING_NEW);
        }
        private void EditTechButton_Click(object sender, EventArgs e)
        {
            OpenEditView(TechEditView.ViewMode.EDITING, SelectedTechID);
        }
        private void ViewTechButton_Click(object sender, EventArgs e)
        {
            OpenEditView(TechEditView.ViewMode.READ_ONLY, SelectedTechID);
        }
        private void AddPrereqButton_Click(object sender, EventArgs e)
        {
            int views = NumberOfOpenEditingViews;
            if (views == 0) return;
            if (views == 1)
            {
                int viewIndex = -1;
                //Find the view that's in add or edit mode
                for (int j = 0; j < openEditViews.Count; j++)
                {
                    if (openEditViews[j].Mode == TechEditView.ViewMode.ADDING_NEW ||
                        openEditViews[j].Mode == TechEditView.ViewMode.EDITING)
                    {
                        viewIndex = j;
                        break;
                    }
                }
                openEditViews[viewIndex].AddPrereq(SelectedTechID);
                openEditViews[viewIndex].Activate();
            }
            else
            {
                //Multiple editing views are open. Pass to the editing form selector dialog
                EditViewSelector selector = new EditViewSelector(ref openEditViews);
                selector.ShowDialog(this);
                if (selector.SelectedViewIndex != -1)
                {
                    openEditViews[selector.SelectedViewIndex].AddPrereq(SelectedTechID);
                    openEditViews[selector.SelectedViewIndex].Activate();
                }
                selector.Dispose();
            }
        }
        private void AddGrantreqButton_Click(object sender, EventArgs e)
        {
            int views = NumberOfOpenEditingViews;
            if (views == 0) return;
            if (views == 1)
            {
                int viewIndex = -1;
                //Find the view that's in add or edit mode
                for (int j = 0; j < openEditViews.Count; j++)
                {
                    if (openEditViews[j].Mode == TechEditView.ViewMode.ADDING_NEW ||
                        openEditViews[j].Mode == TechEditView.ViewMode.EDITING)
                    {
                        viewIndex = j;
                        break;
                    }
                }
                openEditViews[viewIndex].AddGrantreq(SelectedTechID);
                openEditViews[viewIndex].Activate();
            }
            else
            {
                //Multiple editing views are open. Pass to the editing form selector dialog
                EditViewSelector selector = new EditViewSelector(ref openEditViews);
                selector.ShowDialog(this);
                if (selector.SelectedViewIndex != -1)
                {
                    openEditViews[selector.SelectedViewIndex].AddGrantreq(SelectedTechID);
                    openEditViews[selector.SelectedViewIndex].Activate();
                }
                selector.Dispose();
            }
        }
        private void AddPermanizesButton_Click(object sender, EventArgs e)
        {
            int views = NumberOfOpenEditingViews;
            if (views == 0) return;
            if (views == 1)
            {
                int viewIndex = -1;
                //Find the view that's in add or edit mode
                for (int j = 0; j < openEditViews.Count; j++)
                {
                    if (openEditViews[j].Mode == TechEditView.ViewMode.ADDING_NEW ||
                        openEditViews[j].Mode == TechEditView.ViewMode.EDITING)
                    {
                        viewIndex = j;
                        break;
                    }
                }
                openEditViews[viewIndex].AddPermanizes(SelectedTechID);
                openEditViews[viewIndex].Activate();
            }
            else
            {
                //Multiple editing views are open. Pass to the editing form selector dialog
                EditViewSelector selector = new EditViewSelector(ref openEditViews);
                selector.ShowDialog(this);
                if (selector.SelectedViewIndex != -1)
                {
                    openEditViews[selector.SelectedViewIndex].AddPermanizes(SelectedTechID);
                    openEditViews[selector.SelectedViewIndex].Activate();
                }
                selector.Dispose();
            }
        }
        private void TechListGrid_SelectionChanged(object sender, EventArgs e)
        {
            //Check if a record is selected
            if (TechListGrid.SelectedRows.Count > 0)
            {
                //A record is selected. Activate record-contextual buttons
                ViewTechButton.Enabled = true;
                DeleteTechButton.Enabled = true;
                EditTechButton.Enabled = true;
                AddPrereqButton.Enabled = true;
                AddGrantreqButton.Enabled = true;
                AddPermanizesButton.Enabled = true;
                for (int i = 0; i < observers.Count; i++)
                    observers[i].Notify(SelectedTechID);
            }
            else
            {
                //No record is selected. Deactivate record-contextual buttons
                ViewTechButton.Enabled = false;
                DeleteTechButton.Enabled = false;
                EditTechButton.Enabled = false;
                AddPrereqButton.Enabled = false;
                AddGrantreqButton.Enabled = false;
                AddPermanizesButton.Enabled = false;

            }
        }
        private void IDRangeMinInput_TextChanged(object sender, EventArgs e)
        {
            //Validate valid hex. If not hex, remove all non-hex characters
            if (IDRgx.IsMatch(IDRangeMinInput.Text))
            {
                int i = 0;
                while (i < IDRangeMinInput.Text.Length)
                {
                    //Remove all non-hex characters
                    if (IDRgx.IsMatch(IDRangeMinInput.Text.Substring(i, 1)))
                        IDRangeMinInput.Text = IDRangeMinInput.Text.Substring(0, i) + IDRangeMinInput.Text.Substring(i + 1);
                    else i++;
                }
            }
        }
        private void IDRangeMaxInput_TextChanged(object sender, EventArgs e)
        {
            //Validate valid hex. If not hex, remove all non-hex characters
            if (IDRgx.IsMatch(IDRangeMaxInput.Text))
            {
                int i = 0;
                while (i < IDRangeMaxInput.Text.Length)
                {
                    //Remove all non-hex characters
                    if (IDRgx.IsMatch(IDRangeMaxInput.Text.Substring(i, 1)))
                        IDRangeMaxInput.Text = IDRangeMaxInput.Text.Substring(0, i) + IDRangeMaxInput.Text.Substring(i + 1);
                    else i++;
                }
            }
        }
        private void IDRangeMinInput_Leave(object sender, EventArgs e)
        {
            //Pad leading zeroes to make length 8
            while (IDRangeMinInput.Text.Length < 8)
            {
                IDRangeMinInput.Text = "0" + IDRangeMinInput.Text;
            }
        }
        private void IDRangeMaxInput_Leave(object sender, EventArgs e)
        {
            //Pad leading zeroes to make length 8
            while (IDRangeMaxInput.Text.Length < 8)
            {
                IDRangeMaxInput.Text = "0" + IDRangeMaxInput.Text;
            }
        }
        private void TechListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewTechButton_Click(sender, e);
        }
        private void ViewGraphButton_Click(object sender, EventArgs e)
        {
            CloseGraphView();
            OpenGraphView();
        }

        //*********************************************************************
        //************************ Database Functions *************************
        //*********************************************************************

        private List<string> GetTableNames()
        {
            /*
             * Returns a list of table names in the database
             */
            List<string> tableNames = new List<string>();
            connection.Open();
            MySqlCommand command = new MySqlCommand("SHOW TABLES", connection);
            //Log("Getting table names. Connection state: " + connection.State.ToString());
            MySqlDataReader myReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (myReader.Read())
                {
                    tableNames.Add(myReader.GetString(0));
                }
            }
            catch (MySqlException ex)
            {
                Log("An error occurred: " + ex.Message);
            }
            finally
            {
                // always call Close when done reading.
                myReader.Close();
                connection.Close(); //not sure if this does anything more than the above
            }
            return tableNames;
        }
        private void InitializeDatabase()
        {
            List<string> tableNames = GetTableNames();
            MySqlCommand command;
            connection.Open();

            //Create categories
            if (!(tableNames.Contains("categories")))
            {
                //Create the categories table
                try
                {
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE categories (" +
                        "name VARCHAR(16) NOT NULL," +
                        "PRIMARY KEY (name));";
                    command.BeginExecuteNonQuery();
                    Log("categories table was created.");
                }
                catch (MySqlException ex)
                {
                    Log("An error occurred: " + ex.Message);
                }
                finally
                {
                    //don't have to do anything to a non query ?
                }

            }
            else Log("categories already exists.");

            //Create tech
            if (!(tableNames.Contains("tech")))
            {
                //Create the tech table
                try
                {
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE tech (" +
                        "id INT UNSIGNED NOT NULL," +
                        "name varchar(32) NOT NULL," +
                        "category varchar(16) NOT NULL," +
                        "field_name varchar(32)," +
                        "cost_per_day FLOAT UNSIGNED NOT NULL DEFAULT 0," +
                        "number_days FLOAT UNSIGNED NOT NULL DEFAULT 0," +
                        "PRIMARY KEY (id)," +
                        "FOREIGN KEY (category)" +
                        "  REFERENCES categories(name));";
                    command.BeginExecuteNonQuery();
                    Log("tech table was created.");
                }
                catch (MySqlException ex)
                {
                    Log("An error occurred: " + ex.Message);
                }
                finally
                {
                    //don't have to do anything to a non query ?
                }
            }
            else Log("tech already exists.");

            //Create tech_prereqs
            if (!(tableNames.Contains("tech_prereqs")))
            {
                //Create the tech_prereqs table
                try
                {
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE tech_prereqs (" +
                        "id INT UNSIGNED NOT NULL," +
                        "prereq_id INT UNSIGNED NOT NULL," +
                        "PRIMARY KEY (id, prereq_id)," +
                        //"UNIQUE (id, prereq_id)," +
                        "FOREIGN KEY (id)" +
                        "  REFERENCES tech(id)," +
                        "FOREIGN KEY (prereq_id)" +
                        "  REFERENCES tech(id));";
                    command.BeginExecuteNonQuery();
                    Log("tech_prereqs table was created.");
                }
                catch (MySqlException ex)
                {
                    Log("An error occurred: " + ex.Message);
                }
                finally
                {
                    //don't have to do anything to a non query ?
                }
            }
            else Log("tech_prereqs already exists.");

            //Create tech_grantreqs
            if (!(tableNames.Contains("tech_grantreqs")))
            {
                //Create the tech_grantreqs table
                try
                {
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE tech_grantreqs (" +
                        "id INT UNSIGNED NOT NULL," +
                        "grantreq_id INT UNSIGNED NOT NULL," +
                        "PRIMARY KEY (id, grantreq_id)," +
                        "FOREIGN KEY (id)" +
                        "  REFERENCES tech(id)," +
                        "FOREIGN KEY (grantreq_id)" +
                        "  REFERENCES tech(id));";
                    command.BeginExecuteNonQuery();
                    Log("tech_grantreqs table was created.");
                }
                catch (MySqlException ex)
                {
                    Log("An error occurred: " + ex.Message);
                }
                finally
                {
                    //don't have to do anything to a non query ?
                }
            }
            else Log("tech_grantreqs already exists.");

            //Create tech_permanizes
            if (!(tableNames.Contains("tech_permanizes")))
            {
                //Create the tech_permanizes table
                try
                {
                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE tech_permanizes (" +
                        "id INT UNSIGNED NOT NULL," +
                        "permanizes_id INT UNSIGNED NOT NULL," +
                        "PRIMARY KEY (id, permanizes_id)," +
                        "FOREIGN KEY (id)" +
                        "  REFERENCES tech(id)," +
                        "FOREIGN KEY (permanizes_id)" +
                        "  REFERENCES tech(id));";
                    command.BeginExecuteNonQuery();
                    Log("tech_permanizes table was created.");
                }
                catch (MySqlException ex)
                {
                    Log("An error occurred: " + ex.Message);
                }
                finally
                {
                    //don't have to do anything to a non query ?
                }
            }
            else Log("tech_permanizes already exists.");

            //Close the connection
            connection.Close();
        }
        private void FetchTechList(FilterOptions filters)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id,name,category " +
                                    "FROM tech ";

            //Structure the filter command conditions
            int numFilters = 0;
            if (filters.idRangeActive || 
                filters.categoryActive || 
                filters.fieldNameActive || 
                filters.nameStringMatchActive)
                command.CommandText += "WHERE ";
            if (filters.idRangeActive)
            {
                command.CommandText += "(id BETWEEN " + filters.idRangeMin +
                    " AND " + filters.idRangeMax + ") ";
                numFilters++;
            }
            if (filters.categoryActive)
            {
                if (numFilters > 0) command.CommandText += "AND ";
                command.CommandText += "category='" + filters.category + "' ";
                numFilters++;
            }
            if (filters.fieldNameActive)
            {
                if (numFilters > 0) command.CommandText += "AND ";
                command.CommandText += "field_name LIKE '%" + filters.fieldName + "%' ";
                numFilters++;
            }
            if (filters.nameStringMatchActive)
            {
                if (numFilters > 0) command.CommandText += "AND ";
                command.CommandText += "name LIKE '%" + filters.nameString + "%' ";
                numFilters++;
            }
            command.CommandText +=  "ORDER BY id ASC;";
            uint rowsFetched = 0;
            try
            {
                connection.Open();
                TechListGrid.Rows.Clear();
                MySqlDataReader reader = command.ExecuteReader();
                string[] values = new string[3];
                while (reader.Read())
                {
                    values[0] = HexConverter.IntToHex(reader.GetUInt32(0));
                    values[1] = reader.GetString(1);
                    values[2] = reader.GetString(2);
                    TechListGrid.Rows.Add(values);
                    rowsFetched++;
                }
            }
            catch (MySqlException ex)
            {
                Log("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Log("Fetched " + rowsFetched + " records.");
            }

        }
        private void DeleteSelectedTech()
        {
            if (SelectedTechID == 0) return;
            DialogResult result = MessageBox.Show(
                this, "Are you sure you wish to delete this tech " +
                "and all of its connections?", "Confirm Tech Deletion",
                MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel) return;
            TechEditView deletor = new TechEditView(
                99999, SelectedTechID, TechEditView.ViewMode.EDITING);
            deletor.techListView = this;
            bool success = true;
            success &= deletor.DeletePrereqsOfOriginal();
            success &= deletor.DeleteGrantreqsOfOriginal();
            success &= deletor.DeletePermanizesOfOriginal();
            success &= deletor.DeletePrereqs();
            success &= deletor.DeleteGrantreqs();
            success &= deletor.DeletePermanizes();
            success &= deletor.DeleteOriginalTech();
            if (success)
            {
                Log("Tech deleted: " + HexConverter.IntToHex(SelectedTechID));
            }
            else
            {
                Log("Error(s) encountered during tech deletion. Check " +
                    "integrity of id " + HexConverter.IntToHex(SelectedTechID));
            }
            deletor.Dispose();
            RefreshList();
        }
        private void LoadCategories()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM categories;";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                CategoryComboBox.Items.Clear();
                while (reader.Read())
                {
                    CategoryComboBox.Items.Add(reader.GetString(0));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while loading categories: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }



    }
}
