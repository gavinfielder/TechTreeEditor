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

namespace TechTreeEditor
{
    public partial class TechListView : Form
    {
        //Fields
        private MySqlConnection connection;
        private List<TechEditView> openEditViews;
        private int nextEditViewID = 0;
        public int lastFocusViewID = -1;
        private struct FilterOptions
        {
            public bool idRangeActive, categoryActive, nameStringMatchActive, fieldNameActive;
            public uint idRangeMin, idRangeMax;
            public string category;
            public string nameString;
            public string fieldName;
        }
        FilterOptions currentFilters;

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
        }
        ~TechListView()
        {
            EndDatabaseSession();
        }

        //Connection management
        private void EndDatabaseSession()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        //Open or closes a new tech edit view
        private void OpenEditView(TechEditView.ViewMode mode, uint id = 0)
        {
            openEditViews.Add(new TechEditView(nextEditViewID, id, mode));
            nextEditViewID++;
            openEditViews[openEditViews.Count - 1].techListView = this;
            openEditViews[openEditViews.Count - 1].Visible = true;
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
            lastFocusViewID = -1;
        }

        //Event Handlers
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
            //TODO
        }
        private void UpdateFiltersButton_Click(object sender, EventArgs e)
        {
            /*
            if (IDRangeMinInput.Text != "" || IDRangeMaxInput.Text != "")
            {
                currentFilters.idRangeActive = true;
                currentFilters.idRangeMin = HexConverter.HexToInt(IDRangeMinInput.Text);
                currentFilters.idRangeMax = HexConverter.HexToInt(IDRangeMaxInput.Text);
            }
            //TODO other filters
            */
            FetchTechList(currentFilters);
        }
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            IDRangeMinInput.Text = "";
            IDRangeMaxInput.Text = "";
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
            //TODO
        }
        private void ViewTechButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AddPrereqButton_Click(object sender, EventArgs e)
        {
            //Find an open TechEditView with the most recent focus
            int i = 0;
            while (i < openEditViews.Count && i != openEditViews[i].EditViewID) i++;
            if (i < openEditViews.Count)
            {
                //Found. Add the prereq
                openEditViews[i].AddPrereq(
                    HexConverter.HexToInt(TechListGrid.SelectedRows[0].Cells[0].Value as string));
            }
        }
        private void AddGrantreqButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AddPermanizesButton_Click(object sender, EventArgs e)
        {
            //TODO
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
        //Log management
        public void Log(string message)
        {
            LogDisplay.AppendText(message + "\r\n");
        }

        //Database functions
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
                command.CommandText += "category = '" + filters.category + "' ";
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
            Log("Fetching data with command = \"" + command.CommandText + "\"");

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
            /*catch (Exception ex) //For debug only
            {
                Log("An unhandled exception occurred: " + ex.Message);
            }*/
            finally
            {
                connection.Close();
                Log("Fetched " + rowsFetched + " records.");
            }

        }
        





        /*
        static void Main(string[] args)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.myfirstdatabaseConnectionString);
            MySqlCommand command = new MySqlCommand();
            command.CommandText = 
                "INSERT INTO customers(CustomerID,CompanyName,ContactName,Phone) " +
                    "VALUES ('00001','AllState','Bill','(760) 375-1234')," +
                           "('00002','StateFarm','Mandy','(760) 375-4321')," +
                           "('00005','Geico','Greg','(760) 382-1234');";
            command.Connection = connection;


            connection.Open();
            try
            {
                int aff = command.ExecuteNonQuery();
                Console.WriteLine(aff + " rows were affected.");
            }
            catch
            {
                Console.WriteLine("Error encountered during INSERT operation.");
            }
            finally
            {
                connection.Close();
            }
            

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }
        */
    }
}
