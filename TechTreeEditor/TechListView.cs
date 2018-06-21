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

        //Constructor and Destructor
        public TechListView()
        {
            InitializeComponent();
            connection = new MySqlConnection(Properties.Settings.Default.dbConnectionString);
            //connection.Open(); //changing to not always open

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
            //TODO
        }
        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AddTechButton_Click(object sender, EventArgs e)
        {
            //TODO
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
            //TODO
        }
        private void AddGrantreqButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AddPermanizesButton_Click(object sender, EventArgs e)
        {
            //TODO
        }

        //Log management
        private void Log(string message)
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
