using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Data.MySql;
using System.Text.RegularExpressions;

namespace TechTreeEditor
{
    public partial class TechEditView : Form, Observer
    {
        //*********************************************************************
        //*************************** Data Structures *************************
        //*********************************************************************
        public enum ViewMode
        {
            ADDING_NEW,
            EDITING,
            READ_ONLY
        };
        public ViewMode Mode
        {
            get
            {
                return viewMode_internal;
            }
            private set
            {
                if (value == ViewMode.ADDING_NEW)
                {
                    viewMode_internal = value;
                    RevertBehaviorDisplay.Visible = false;
                    RevertButton.Enabled = false;
                    SaveBehaviorDisplay.Text = "Adds this tech to the database";
                    OtherInformationGroupBox.Enabled = false;
                    OtherInformationGroupBox.Visible = false;
                    SaveAndCloseButton.Visible = true;
                    SaveAndCloseButton.Enabled = true;
                }
                else if (value == ViewMode.EDITING)
                {
                    viewMode_internal = value;
                    SaveBehaviorDisplay.Text =
                         "Updates the Tech. If the ID was\r\n" +
                         "changed, references will update.";
                    RevertButton.Enabled = true;
                    RevertBehaviorDisplay.Visible = true;
                    OtherInformationGroupBox.Enabled = true;
                    OtherInformationGroupBox.Visible = true;
                    SaveAndCloseButton.Visible = true;
                    SaveAndCloseButton.Enabled = true;
                }
                else if (value == ViewMode.READ_ONLY)
                {
                    viewMode_internal = value;
                    IDInput.Enabled = false;
                    CostPerDayInput.Enabled = false;
                    NumberDaysInput.Enabled = false;
                    NameInput.Enabled = false;
                    CategoryComboBox.Enabled = false;
                    FieldNameInput.Enabled = false;
                    PrereqsListBox.Enabled = false;
                    GrantreqsListBox.Enabled = false;
                    PermanizesListBox.Enabled = false;
                    IsPrereqForListBox.Enabled = false;
                    IsGrantreqForListBox.Enabled = false;
                    IsPermanizedByListBox.Enabled = false;
                    SaveButton.Enabled = false;
                    RevertButton.Enabled = false;
                    RemovePrereqButton.Enabled = false;
                    RemoveGrantreqButton.Enabled = false;
                    RemovePermanizesButton.Enabled = false;
                    SaveBehaviorDisplay.Visible = false;
                    RevertBehaviorDisplay.Visible = false;
                    //Read only (view mode) enables this option:
                    AlwaysViewSelectedCheckBox.Enabled = true;
                    AlwaysViewSelectedCheckBox.Visible = true;
                }
                UpdateTitleBar();
            }
        }
        public TechListView techListView; //reference set by TechListView
        public int EditViewID { get; }
        
        private ViewMode viewMode_internal; //Do not use (outside of Mode property)
        private MySqlConnection connection;
        private Regex IDRgx; //matches if invalid hex string
        private Regex alphaRgx; //matches if any special characters

        //Data
        private class TechDataContainer
        {
            //Data fields
            public uint techID = 0;
            public bool techIDChanged = false;
            public float techCostPerDay = 0f;
            public bool techCostPerDayChanged = false;
            public float techNumberDays = 0f;
            public bool techNumberDaysChanged = false;
            public string techName = "";
            public bool techNameChanged = false;
            public string techCategory = "";
            public bool techCategoryChanged = false;
            public string techFieldName = "";
            public bool techFieldNameChanged = false;
            public List<uint> techPrereqs = new List<uint>();
            public bool techPrereqsChanged = false;
            public List<uint> techGrantreqs = new List<uint>();
            public bool techGrantreqsChanged = false;
            public List<uint> techPermanizes = new List<uint>();
            public bool techPermanizesChanged = false;

            //Sets all the fields to changed
            public void SetAllChanged(bool changed)
            {
                techIDChanged = changed;
                techCostPerDayChanged = changed;
                techNumberDaysChanged = changed;
                techNameChanged = changed;
                techCategoryChanged = changed;
                techFieldNameChanged = changed;
                techPrereqsChanged = changed;
                techGrantreqsChanged = changed;
                techPermanizesChanged = changed;
            }

            //If any fields are changed, return true
            public bool AnyAreChanged()
            {
                return (techIDChanged || techCostPerDayChanged ||
                    techNumberDaysChanged || techNameChanged ||
                    techCategoryChanged || techFieldNameChanged ||
                    techPrereqsChanged || techGrantreqsChanged ||
                    techPermanizesChanged);
            }

            //Copies another tech data container
            public void Copy(TechDataContainer other)
            {
                techID = other.techID;
                techIDChanged = other.techIDChanged;
                techCostPerDay = other.techCostPerDay;
                techCostPerDayChanged = other.techCostPerDayChanged;
                techNumberDays = other.techNumberDays;
                techNumberDaysChanged = other.techNumberDaysChanged;
                techName = other.techName;
                techNameChanged = other.techNameChanged;
                techCategory = other.techCategory;
                techCategoryChanged = other.techCategoryChanged;
                techFieldName = other.techFieldName;
                techFieldNameChanged = other.techFieldNameChanged;
                techPrereqs.Clear();
                techGrantreqs.Clear();
                techPermanizes.Clear();
                foreach (uint n in other.techPrereqs) techPrereqs.Add(n);
                foreach (uint n in other.techGrantreqs) techGrantreqs.Add(n);
                foreach (uint n in other.techPermanizes) techPermanizes.Add(n);
            }

            //Sets changed flag for prereq collection
            public void CheckPrereqsChanged(TechDataContainer orig)
            {
                techPrereqsChanged = false;
                if (techPrereqs.Count != orig.techPrereqs.Count)
                {
                    techPrereqsChanged = true;
                    return;
                }
                if (techPrereqs.Count == 0)
                {
                    return; // keep as false
                }
                foreach (uint n in orig.techPrereqs)
                {
                    if (!(techPrereqs.Contains(n)))
                    {
                        techPrereqsChanged = true;
                        return;
                    }
                }
            }

            //Sets changed flag for grantreq collection
            public void CheckGrantreqsChanged(TechDataContainer orig)
            {
                techGrantreqsChanged = false;
                if (techGrantreqs.Count != orig.techGrantreqs.Count)
                {
                    techGrantreqsChanged = true;
                    return;
                }
                if (techGrantreqs.Count == 0)
                {
                    return; // keep as false
                }
                foreach (uint n in orig.techGrantreqs)
                {
                    if (!(techGrantreqs.Contains(n)))
                    {
                        techGrantreqsChanged = true;
                        return;
                    }
                }
            }

            //Sets changed flag for permanizes collection
            public void CheckPermanizesChanged(TechDataContainer orig)
            {
                techPermanizesChanged = false;
                if (techPermanizes.Count != orig.techPermanizes.Count)
                {
                    techPermanizesChanged = true;
                    return;
                }
                if (techPermanizes.Count == 0)
                {
                    return; // keep as false
                }
                foreach (uint n in orig.techPermanizes)
                {
                    if (!(techPermanizes.Contains(n)))
                    {
                        techPermanizesChanged = true;
                        return;
                    }
                }
            }
        }
        private TechDataContainer current, original;

        //*********************************************************************
        //**************************** Basic Methods **************************
        //*********************************************************************

        //Constructor
        public TechEditView(int viewID, uint id = 0, ViewMode mode = ViewMode.ADDING_NEW)
        {
            InitializeComponent();
            //Set the ID of this window
            EditViewID = viewID;
            //Instantiate validation regex's
            IDRgx = new Regex(@"[^0123456789ABCDEFabcdef]");
            alphaRgx = new Regex(@"[^a-zA-Z0-9 ]");
            //Set up database connection
            connection = new MySqlConnection(Properties.Settings.Default.dbConnectionString);
            //Set up data containers
            current = new TechDataContainer();
            original = new TechDataContainer();
            //Populate the categories combo box
            LoadCategories();
            //Set view mode
            if (id != 0) ViewTech(id);
            Mode = mode;
            UpdateTitleBar();
        }

        //Changes the title bar to reflect current editing mode and tech name
        public void UpdateTitleBar()
        {
            if (Mode == ViewMode.READ_ONLY)
                Text = "Viewing Tech: " + original.techName + " (" +
                    HexConverter.IntToHex(original.techID) + ")";
            else if (Mode == ViewMode.ADDING_NEW)
                Text = "Add Tech: " + current.techName + " (" +
                    HexConverter.IntToHex(current.techID) + ")";
            else if (Mode == ViewMode.EDITING)
                Text = "Editing Tech: " + original.techName + " (" +
                    HexConverter.IntToHex(original.techID) + ")";
        }

        //Receives notification when new tech selected in tech list view
        public void Notify(uint id)
        {
            ViewTech(id);
        }

        //*********************************************************************
        //***************************** Form Tasks ****************************
        //*********************************************************************

        //Adds a prereq. Called by TechListView when user clicks add prereq
        public void AddPrereq(uint id)
        {
            //Not allowed in read only mode
            if (Mode == ViewMode.READ_ONLY) return;
            if (id == current.techID)
            {
                MessageBox.Show("A tech may not be a prerequisite for itself.");
                return;
            }
            current.techPrereqs.Add(id);
            current.CheckPrereqsChanged(original);
            PopulatePrereqs();
        }
        //Adds a grantreq. Called by TechListView when user clicks add prereq
        public void AddGrantreq(uint id)
        {
            //Not allowed in read only mode
            if (Mode == ViewMode.READ_ONLY) return; if (id == current.techID)
            {
                MessageBox.Show("A tech may not be a grantrequisite for itself.");
                return;
            }
            current.techGrantreqs.Add(id);
            current.CheckGrantreqsChanged(original);
            PopulateGrantreqs();
        }
        //Adds a permanizes. Called by TechListView when user clicks add prereq
        public void AddPermanizes(uint id)
        {
            //Not allowed in read only mode
            if (Mode == ViewMode.READ_ONLY) return;
            current.techPermanizes.Add(id);
            current.CheckPermanizesChanged(original);
            PopulatePermanizes();
        }
        //Populates the fields with the provided data set
        public void DisplayCurrent()
        {
            IDInput.Text = HexConverter.IntToHex(current.techID);
            NameInput.Text = current.techName;
            int i = 0;
            while (CategoryComboBox.Items[i] 
                as string != current.techCategory) i++;
            CategoryComboBox.SelectedIndex = i;
            FieldNameInput.Text = current.techFieldName;
            CostPerDayInput.Text = current.techCostPerDay.ToString();
            NumberDaysInput.Text = current.techNumberDays.ToString();
            PopulatePrereqs();
            PopulateGrantreqs();
            PopulatePermanizes();
        }
        //Reverts the form to the original data
        public void Revert()
        {
            //Only allowed in edit mode
            if (Mode != ViewMode.EDITING) return;
            current.Copy(original);
            current.SetAllChanged(false);
            DisplayCurrent();
        }
        

        //Validates the form data. Informs user of problems
        private bool ValidateForm()
        {
            //Validate that ID isn't 0
            if (IDInput.Text == "00000000")
            {
                MessageBox.Show("00000000 Is a reserved ID.");
                return false;
            }
            //Validate cost per day
            if (current.techCostPerDay < 0)
            {
                MessageBox.Show("Cost per day cannot be negative");
                return false;
            }
            //validate number days
            if (current.techNumberDays < 0)
            {
                MessageBox.Show("Number days cannot be negative");
                return false;
            }
            //Validate name
            if (current.techName == "")
            {
                MessageBox.Show("Name cannot be null.");
                return false;
            }
            else if (alphaRgx.IsMatch(current.techName))
            {
                MessageBox.Show("Name can have only letters, numbers, and spaces.");
                return false;
            }
            //Validate category
            if (current.techCategory == "")
            {
                MessageBox.Show("Category cannot be null.");
                return false;
            }
            //All validated
            return true;
        }

        //*********************************************************************
        //************************** Database Tasks ***************************
        //*********************************************************************

        //Loads a tech from database and populates the fields
        public void ViewTech(uint id)
        {
            //This uses the changed flags to flag load success
            TechDataContainer input = new TechDataContainer();
            input.SetAllChanged(false);
            input.techIDChanged = FetchTech(id, ref input);
            input.techPrereqsChanged = FetchPrereqs(id, ref input);
            input.techGrantreqsChanged = FetchGrantreqs(id, ref input);
            input.techPermanizesChanged = FetchPermanizes(id, ref input);
            //Check if success and then copy to current and original
            if (input.techIDChanged && input.techPrereqsChanged &&
                input.techGrantreqsChanged && input.techPermanizesChanged)
            {
                //success
                current.Copy(input);
                original.Copy(input);
                current.SetAllChanged(false);
                original.SetAllChanged(false);
                DisplayCurrent();
                PopulateIsPrereqFor();
                PopulateIsGrantreqFor();
                PopulateIsPermanizedBy();
            }
            else
            {
                //failure
                techListView.Log("ViewTech failed for id " +
                    HexConverter.IntToHex(id));
            }
        }
        //Updates an existing record with the updated information
        private void EditTech()
        {
            //Only allowed while in edit mode
            if (Mode != ViewMode.EDITING) return;            
            //Validate the form. Inform user of any problems
            bool valid = ValidateForm();
            if (!(valid)) return;
            bool error = false;
            if (current.techIDChanged)
            {
                //Get connections to original
                List<uint> IsPrereqFor = new List<uint>();
                List<uint> IsGrantreqFor = new List<uint>();
                List<uint> IsPermanizedBy = new List<uint>();
                error = !(FetchConnectionsToOriginal(ref IsPrereqFor,
                    ref IsGrantreqFor, ref IsPermanizedBy));
                if (error) return;
                //Delete connections to the original tech
                error = !(DeleteConnectionsToOriginal());
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech_prereqs, tech_grantreqs, " +
                        "and tech_permanizes for errors involving ids " +
                        original.techID + " or " + current.techID);
                    return;
                }
                //Delete the original tech's connections
                error = !(DeletePrereqsOfOriginal() &
                    DeleteGrantreqsOfOriginal() &
                    DeletePermanizesOfOriginal());
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech_prereqs, tech_grantreqs, " +
                        "and tech_permanizes for errors involving ids " +
                        original.techID + " or " + current.techID);
                    return;
                }
                //Delete the original tech
                error = !(DeleteOriginalTech());
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech for errors involving id " +
                        original.techID);
                    return;
                }
                //Insert the tech with updated ID
                error = !(InsertCurrentTech());
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech, tech_prereqs, tech_grantreqs, " +
                        "and tech_permanizes for errors involving id " +
                        current.techID);
                    return;
                }
                //Insert connections to the tech
                error = !(InsertConnectionsToUpdated(IsPrereqFor,
                    IsGrantreqFor, IsPermanizedBy));
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech_prereqs, tech_grantreqs, " +
                        "and tech_permanizes for errors involving ids " +
                        current.techID);
                    return;
                }
                //Insert prereqs and grantreqs and permanizes of the updated tech
                error = !(InsertPrereqs() & InsertGrantreqs() & InsertPermanizes());
                if (error)
                {
                    techListView.Log("Tech update compromised, " +
                        "aborting. Check tech_prereqs, tech_grantreqs, " +
                        "and tech_permanizes for errors involving ids " +
                        current.techID);
                    return;
                }
                techListView.Log("Edited tech: " + original.techID + " has " +
                    "been changed to " + current.techID + " (" + current.techName +
                    ")");
            }
            else
            {
                //ID has not been changed. 
                if (current.techCostPerDayChanged || 
                    current.techNumberDaysChanged ||
                    current.techCategoryChanged ||
                    current.techFieldNameChanged ||
                    current.techNameChanged)
                {
                    error = !(UpdateTech());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech. Aborting. Please check " +
                            "integrity of id " + current.techID + " in tech");
                        return;
                    }
                }
                if (current.techPrereqsChanged)
                {
                    error = !(DeletePrereqs());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (deleting prereqs). Aborting. " +
                            "Please check integrity of id " + 
                            current.techID + " in tech_prereqs");
                        return;
                    }
                    error = !(InsertPrereqs());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (inserting prereqs). Aborting. " +
                            "Please check integrity of id " +
                            current.techID + " in tech_prereqs");
                        return;
                    }
                }
                if (current.techGrantreqsChanged)
                {
                    error = !(DeleteGrantreqs());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (deleting grantreqs). Aborting. " +
                            "Please check integrity of id " +
                            current.techID + " in tech_grantreqs");
                        return;
                    }
                    error = !(InsertGrantreqs());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (inserting grantreqs). Aborting. " +
                            "Please check integrity of id " +
                            current.techID + " in tech_grantreqs");
                        return;
                    }
                }
                if (current.techPermanizesChanged)
                {
                    error = !(DeletePermanizes());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (deleting permanizes). Aborting. " +
                            "Please check integrity of id " +
                            current.techID + " in tech_permanizes");
                        return;
                    }
                    error = !(InsertPermanizes());
                    if (error)
                    {
                        techListView.Log("An error has occurred while " +
                            "editing tech (inserting permanizes). Aborting. " +
                            "Please check integrity of id " +
                            current.techID + " in tech_permanizes");
                        return;
                    }
                }
                techListView.Log("Tech updated: " + current.techName +
                    "(" + HexConverter.IntToHex(current.techID) + ")");
            }
            //Tech updated successfully. Update original to reflect new
            current.SetAllChanged(false);
            original.Copy(current);
            UpdateTitleBar();
            techListView.RefreshList();
            techListView.Select(current.techID);
        }
        //Adds a new tech
        private void AddTech()
        {
            //TODO: add error checking in this function
            //Only allowed while in add mode
            if (Mode != ViewMode.ADDING_NEW) return;
            //Validate the form. Inform user of any problems
            bool valid = ValidateForm();
            if (!(valid)) return;
            //Insert the tech
            InsertCurrentTech();
            //Insert new tech prerequisites
            InsertPrereqs();
            //Insert new tech grantrequisites
            InsertGrantreqs();
            //Insert new tech permanizes
            InsertPermanizes();
            //Once done, change mode to edit and set the original to the new record
            Mode = ViewMode.EDITING;
            current.SetAllChanged(false);
            original.Copy(current);
            UpdateTitleBar();
            techListView.Log("Tech added: " + current.techName +
               "(" + HexConverter.IntToHex(current.techID) + ")");
            techListView.RefreshList();
            techListView.Select(current.techID);
        }
        
        //*********************************************************************
        //************************ Database Functions *************************
        //*********************************************************************

        //Read operations only

        //Populates the categories combo box
        public void LoadCategories()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM categories;";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                CategoryComboBox.Items.Clear();
                while (reader.Read()) {
                    CategoryComboBox.Items.Add(reader.GetString(0));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while loading categories: " + 
                    ex.Message);
                techListView.Log("An error occurred while loading categories: " +
                    ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Fetches basic tech data and puts it in the data container. Returns true if successful
        private bool FetchTech(uint id, ref TechDataContainer data)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM tech WHERE id=" + id + ";";
            connection.Open();
            MySqlDataReader reader;
            bool success = true;
            try
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    data.techID = reader.GetUInt32(0);
                    data.techName = reader.GetString(1);
                    data.techCategory = reader.GetString(2);
                    data.techFieldName = reader.GetString(3);
                    data.techCostPerDay = reader.GetFloat(4);
                    data.techNumberDays = reader.GetFloat(5);
                    success = true;
                }
                else
                {
                    techListView.Log("Tech not found: " + HexConverter.IntToHex(id),
                        command.CommandText);
                    success = false;
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while fetching tech " + 
                    HexConverter.IntToHex(id) + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            return success;
        }
        //Fetches prerequisites for the tech and puts it in the data container. Returns true if successful
        private bool FetchPrereqs(uint id, ref TechDataContainer data)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT prereq_id " +
                "FROM tech_prereqs WHERE id=" + id + ";";
            MySqlDataReader reader;
            connection.Open();
            bool success = true;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.techPrereqs.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while fetching " +
                    "prereqs for id " + HexConverter.IntToHex(id) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            return success;
        }
        //Fetches grantrequisites for the tech and puts it in the data container. Returns true if successful
        private bool FetchGrantreqs(uint id, ref TechDataContainer data)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT grantreq_id " +
                "FROM tech_grantreqs WHERE id=" + id + ";";
            MySqlDataReader reader;
            connection.Open();
            bool success = true;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.techGrantreqs.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while fetching " +
                    "grantreqs for id " + HexConverter.IntToHex(id) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            return success;
        }
        //Fetches permanizes relationships for the tech and puts it in the data container. Returns true if successful
        private bool FetchPermanizes(uint id, ref TechDataContainer data)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT permanizes_id " +
                "FROM tech_permanizes WHERE id=" + id + ";";
            MySqlDataReader reader;
            connection.Open();
            bool success = true;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    data.techPermanizes.Add(reader.GetUInt32(0));
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while fetching " +
                    "permanizes for id " + HexConverter.IntToHex(id) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            return success;
        }
        //Populates the Prereqs list box
        private void PopulatePrereqs()
        {
            PrereqsListBox.Items.Clear();
            if (current.techPrereqs.Count == 0) return;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, name FROM tech " +
                "WHERE ";
            int numberOfTechs = 0;
            foreach (uint id in current.techPrereqs)
            {
                if (numberOfTechs > 0) command.CommandText += "OR ";
                command.CommandText += "id=" + id + " ";
                numberOfTechs++;
            }
            string nameInput;
            uint idInput;
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    idInput = reader.GetUInt32(0);
                    nameInput = reader.GetString(1);
                    PrereqsListBox.Items.Add(
                        HexConverter.IntToHex(idInput) + ": " + nameInput);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while populating " +
                    "prereqs for id " + HexConverter.IntToHex(current.techID) +
                    ": " + ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates the Grantreqs list box
        private void PopulateGrantreqs()
        {
            GrantreqsListBox.Items.Clear();
            if (current.techGrantreqs.Count == 0) return;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, name FROM tech " +
                "WHERE ";
            int numberOfTechs = 0;
            foreach (uint id in current.techGrantreqs)
            {
                if (numberOfTechs > 0) command.CommandText += "OR ";
                command.CommandText += "id=" + id + " ";
                numberOfTechs++;
            }
            string nameInput;
            uint idInput;
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    idInput = reader.GetUInt32(0);
                    nameInput = reader.GetString(1);
                    GrantreqsListBox.Items.Add(
                        HexConverter.IntToHex(idInput) + ": " + nameInput);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while populating " +
                    "grantreqs for id " + HexConverter.IntToHex(current.techID) +
                    ": " + ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates the Permanizes list box
        private void PopulatePermanizes()
        {
            PermanizesListBox.Items.Clear();
            if (current.techPermanizes.Count == 0) return;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, name FROM tech " +
                "WHERE ";
            int numberOfTechs = 0;
            foreach (uint id in current.techPermanizes)
            {
                if (numberOfTechs > 0) command.CommandText += "OR ";
                command.CommandText += "id=" + id + " ";
                numberOfTechs++;
            }
            string nameInput;
            uint idInput;
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    idInput = reader.GetUInt32(0);
                    nameInput = reader.GetString(1);
                    PermanizesListBox.Items.Add(
                        HexConverter.IntToHex(idInput) + ": " + nameInput);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while populating " +
                    "permanizes for id " + HexConverter.IntToHex(current.techID) +
                    ": " + ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates "is a prereq for" listbox
        public void PopulateIsPrereqFor()
        {
            IsPrereqForListBox.Items.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT T.name, P.id " +
                "FROM tech AS T, tech_prereqs AS P " +
                "WHERE T.id = P.id " +
                "AND P.prereq_id = " + current.techID + ";";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                string item;
                IsPrereqForListBox.ClearSelected();
                while (reader.Read())
                {
                    item = reader.GetString(0) + " (" +
                        HexConverter.IntToHex(reader.GetUInt32(1)) + ")";
                    IsPrereqForListBox.Items.Add(item);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "populating IsPrereqFor for id " + 
                    HexConverter.IntToHex(current.techID) + ": "+ 
                    ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates "is a grantreq for" listbox
        public void PopulateIsGrantreqFor()
        {
            IsGrantreqForListBox.Items.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT T.name, G.id " +
                "FROM tech AS T, tech_grantreqs AS G " +
                "WHERE T.id = G.id " +
                "AND G.grantreq_id = " + current.techID + ";";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                string item;
                IsGrantreqForListBox.ClearSelected();
                while (reader.Read())
                {
                    item = reader.GetString(0) + " (" +
                        HexConverter.IntToHex(reader.GetUInt32(1)) + ")";
                    IsGrantreqForListBox.Items.Add(item);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "populating IsGrantreqFor for id " +
                    HexConverter.IntToHex(current.techID) + ": " +
                    ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates "is permanized by" listbox
        public void PopulateIsPermanizedBy()
        {
            IsPermanizedByListBox.Items.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT T.name, P.id " +
                "FROM tech AS T, tech_permanizes AS P " +
                "WHERE T.id = P.id " +
                "AND P.permanizes_id = " + current.techID + ";";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                string item;
                IsPermanizedByListBox.ClearSelected();
                while (reader.Read())
                {
                    item = reader.GetString(0) + " (" +
                        HexConverter.IntToHex(reader.GetUInt32(1)) + ")";
                    IsPermanizedByListBox.Items.Add(item);
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "populating IsPermanizedBy for id " +
                    HexConverter.IntToHex(current.techID) + ": " +
                    ex.Message, command.CommandText);
            }
            finally
            {
                connection.Close();
            }
        }
        //Retrieves lists of connections to the original tech.
        private bool FetchConnectionsToOriginal(ref List<uint> IsPrereqFor,
            ref List<uint> IsGrantreqFor, ref List<uint> IsPermanizedBy)
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            MySqlDataReader reader;
            connection.Open();
            try
            {
                command.CommandText = "SELECT id FROM tech_prereqs " +
                    "WHERE prereq_id=" + original.techID;
                reader = command.ExecuteReader();
                while (reader.Read())
                    IsPrereqFor.Add(reader.GetUInt32(0));
                command.CommandText = "SELECT id FROM tech_grantreqs " +
                    "WHERE grantreq_id=" + original.techID;
                reader = command.ExecuteReader();
                while (reader.Read())
                    IsGrantreqFor.Add(reader.GetUInt32(0));
                command.CommandText = "SELECT id FROM tech_permanizes " +
                    "WHERE permanizes_id=" + original.techID;
                reader = command.ExecuteReader();
                while (reader.Read())
                    IsPermanizedBy.Add(reader.GetUInt32(0));
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while loading " +
                    "other connections in update process for " +
                    "id " + original.techID + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }

            return success;
        }

        //Writing operations

        //Inserts current as a new record into tech table
        private bool InsertCurrentTech()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tech " +
                "VALUES(" + current.techID + ",'" + current.techName + "','" +
                current.techCategory + "','" + current.techFieldName + "'," +
                current.techCostPerDay + "," + current.techNumberDays + ");";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                //command.BeginExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while inserting tech with id " +
                    HexConverter.IntToHex(current.techID) + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                techListView.QuietLog("Tech inserted: " + current.techName +
                    "(" + HexConverter.IntToHex(current.techID) + ")",
                    command.CommandText);
                connection.Close();
            }
            return success;
        }
        //Inserts all prereqs as new prereq relationships
        private bool InsertPrereqs()
        {
            if (current.techPrereqs.Count == 0) return true;
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tech_prereqs VALUES";
            int numberOfPrereqs = 0;
            foreach (uint id in current.techPrereqs)
            {
                if (numberOfPrereqs > 0) command.CommandText += ",";
                command.CommandText += "(" + current.techID + "," + id + ")";
                numberOfPrereqs++;
            }
            command.CommandText += ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while inserting prereqs for id " +
                    HexConverter.IntToHex(current.techID) + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
                techListView.QuietLog(numberOfPrereqs + " prerequisites inserted for id " +
                    HexConverter.IntToHex(current.techID) + ".", command.CommandText);
            }
            return success;
        }        
        //Inserts all grantreqs as new grantreq relationships
        private bool InsertGrantreqs()
        {
            if (current.techGrantreqs.Count == 0) return true;
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tech_grantreqs VALUES";
            int numberOfGrantreqs = 0;
            foreach (uint id in current.techGrantreqs)
            {
                if (numberOfGrantreqs > 0) command.CommandText += ",";
                command.CommandText += "(" + current.techID + "," + id + ")";
                numberOfGrantreqs++;
            }
            command.CommandText += ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while inserting grantreqs for id " +
                    HexConverter.IntToHex(current.techID) + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
                techListView.QuietLog(numberOfGrantreqs + " grantrequisites inserted for id " +
                    HexConverter.IntToHex(current.techID) + ".", command.CommandText);
            }
            return success;
        }
        //Inserts all permanizes as new permanizes relationships
        private bool InsertPermanizes()
        {
            if (current.techPermanizes.Count == 0) return true;
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO tech_permanizes VALUES";
            int numberOfPermanizes = 0;
            foreach (uint id in current.techPermanizes)
            {
                if (numberOfPermanizes > 0) command.CommandText += ",";
                command.CommandText += "(" + current.techID + "," + id + ")";
                numberOfPermanizes++;
            }
            command.CommandText += ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while inserting permanizes " +
                    "for id " + HexConverter.IntToHex(current.techID) + ": " +
                    ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
                techListView.QuietLog(numberOfPermanizes + " permanizes " +
                    "relationships inserted for id " + 
                    HexConverter.IntToHex(current.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Updates the current tech record. Assumes ID unchanged.
        private bool UpdateTech()
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE tech " +
                "SET name='" + current.techName + "', " +
                "category='" + current.techCategory + "', " +
                "field_name='" + current.techFieldName + "', " +
                "cost_per_day=" + current.techCostPerDay + ", " +
                "number_days=" + current.techNumberDays + " " +
                "WHERE id=" + current.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while updating tech " +
                    HexConverter.IntToHex(current.techID) + ": " + ex.Message,
                    command.CommandText);
                success = false;
            }
            finally
            {
                techListView.QuietLog("Tech updated: " + current.techName + 
                    "(" + HexConverter.IntToHex(current.techID) + ")", 
                    command.CommandText);
                connection.Close();
            }
            return success;
        }
        //Deletes all prereqs of the current tech
        public bool DeletePrereqs()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_prereqs " +
                "WHERE id=" + current.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting prereqs for id " + 
                    HexConverter.IntToHex(current.techID) + ": " + 
                    ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted prereqs for id " +
                    HexConverter.IntToHex(current.techID) + ".", 
                    command.CommandText);
            }
            return success;
        }
        //Deletes all grantreqs of the current tech
        public bool DeleteGrantreqs()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_grantreqs " +
                "WHERE id=" + current.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting grantreqs: " + ex.Message);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted grantreqs for id " +
                    HexConverter.IntToHex(current.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes all permanizes relationships of the current tech
        public bool DeletePermanizes()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_permanizes " +
                "WHERE id=" + current.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting permanizes: " + ex.Message);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted permanizes for id " +
                    HexConverter.IntToHex(current.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes the original tech. Does not check for connections to it
        public bool DeleteOriginalTech()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech WHERE id=" + original.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while deleting the original " +
                    "tech: " + ex.Message);
                success = false;
            }
            finally
            {
                techListView.Log("Tech deleted: " + original.techName +
                    "(" + HexConverter.IntToHex(original.techID) + ")",
                    command.CommandText);
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted original tech with id " +
                    HexConverter.IntToHex(original.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes connections to the original tech
        private bool DeleteConnectionsToOriginal()
        {

            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            connection.Open();
            try
            {
                command.CommandText = "DELETE FROM tech_prereqs " +
                    "WHERE prereq_id=" + original.techID;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM tech_grantreqs " +
                    "WHERE grantreq_id=" + original.techID;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM tech_permanizes " +
                    "WHERE permanizes_id=" + original.techID;
                command.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while deleting " +
                    "connections to the original tech in update " +
                    "process for original id " + 
                    HexConverter.IntToHex(original.techID) +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted connections to original tech with id " +
                    HexConverter.IntToHex(original.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Inserts connections to this tech using the current tech
        private bool InsertConnectionsToUpdated(List<uint> IsPrereqFor,
            List<uint> IsGrantreqFor, List<uint> IsPermanizedBy)
        {
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            connection.Open();
            try
            {
                //Insert prereqs
                if (IsPrereqFor.Count > 0)
                {
                    command.CommandText = "INSERT INTO tech_prereqs " +
                        "VALUES ";
                    foreach (uint n in IsPrereqFor)
                        command.CommandText += "(" + n + "," + current.techID + "),";
                    command.CommandText = command.CommandText.Trim(',') + ";";
                    command.ExecuteNonQuery();
                }
                //Insert grantreqs
                if (IsGrantreqFor.Count > 0)
                {
                    command.CommandText = "INSERT INTO tech_grantreqs " +
                        "VALUES ";
                    foreach (uint n in IsGrantreqFor)
                        command.CommandText += "(" + n + "," + current.techID + "),";
                    command.CommandText = command.CommandText.Trim(',') + ";";
                    command.ExecuteNonQuery();
                }
                //Insert permanizes
                if (IsPermanizedBy.Count > 0)
                {
                    command.CommandText = "INSERT INTO tech_permanizes " +
                        "VALUES ";
                    foreach (uint n in IsPermanizedBy)
                        command.CommandText += "(" + n + "," + current.techID + "),";
                    command.CommandText = command.CommandText.Trim(',') + ";";
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occured while inserting connections " +
                    "to id " + HexConverter.IntToHex(current.techID) + ": " + 
                    ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Inserted connections to updated tech with id " +
                    HexConverter.IntToHex(current.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes all prereqs of the current tech
        public bool DeletePrereqsOfOriginal()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_prereqs " +
                "WHERE id=" + original.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting prereqs of original with id " +
                    HexConverter.IntToHex(original.techID) + "" +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted prereqs of original tech with id " +
                    HexConverter.IntToHex(original.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes all grantreqs of the current tech
        public bool DeleteGrantreqsOfOriginal()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_grantreqs " +
                "WHERE id=" + original.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting grantreqs of original with id " +
                    HexConverter.IntToHex(original.techID) + "" +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted grantreqs of original tech with id " +
                    HexConverter.IntToHex(original.techID) + ".",
                    command.CommandText);
            }
            return success;
        }
        //Deletes all permanizes relationships of the original tech
        public bool DeletePermanizesOfOriginal()
        {
            //Note: this function is public so tech list view can call it
            bool success = true;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM tech_permanizes " +
                "WHERE id=" + original.techID + ";";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred while " +
                    "deleting permanizes of original with id " +
                    HexConverter.IntToHex(original.techID) + "" +
                    ": " + ex.Message, command.CommandText);
                success = false;
            }
            finally
            {
                connection.Close();
            }
            if (success)
            {
                techListView.QuietLog("Deleted permanizes of original tech with id " +
                    HexConverter.IntToHex(original.techID) + ".",
                    command.CommandText);
            }
            return success;
        }

        //*********************************************************************
        //************************** Event Handlers ***************************
        //*********************************************************************
        private void IDInput_TextChanged(object sender, EventArgs e)
        {
            if (IDInput.Text != "")
            {
                //Validate valid hex. If not hex, remove all non-hex characters
                if (IDRgx.IsMatch(IDInput.Text))
                {
                    int i = 0;
                    while (i < IDInput.Text.Length)
                    {
                        //Remove all non-hex characters
                        if (IDRgx.IsMatch(IDInput.Text.Substring(i, 1)))
                            IDInput.Text = IDInput.Text.Substring(0, i) + IDInput.Text.Substring(i + 1);
                        else i++;
                    }
                }
                current.techID = HexConverter.HexToInt(IDInput.Text);
                current.techIDChanged = (current.techID != original.techID);
                UpdateTitleBar();
            }
        }
        private void IDInput_Leave(object sender, EventArgs e)
        {
            //Pad leading zeroes to make length 8
            while (IDInput.Text.Length < 8)
            {
                IDInput.Text = "0" + IDInput.Text;
            }
        }
        private void CostPerDayInput_Leave(object sender, EventArgs e)
        {
            bool isNumeric = float.TryParse(CostPerDayInput.Text, out current.techCostPerDay);
            if (!(isNumeric))
            {
                MessageBox.Show("Cost per day must be numeric.");
                CostPerDayInput.Text = "0.0";
                current.techCostPerDay = 0f;
            }
            current.techCostPerDayChanged = (current.techCostPerDay != original.techCostPerDay);
        }
        private void NumberDaysInput_Leave(object sender, EventArgs e)
        {
            bool isNumeric = float.TryParse(NumberDaysInput.Text, out current.techNumberDays);
            if (!(isNumeric))
            {
                MessageBox.Show("Cost per day must be numeric.");
                NumberDaysInput.Text = "0.0";
                current.techNumberDays = 0f;
            }
            current.techNumberDaysChanged = (current.techNumberDays != original.techNumberDays);
        }
        private void NameInput_TextChanged(object sender, EventArgs e)
        {
            current.techName = NameInput.Text.Trim(' '); ;
            current.techNameChanged = (current.techName != original.techName);
            UpdateTitleBar();
        }
        private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            current.techCategory = CategoryComboBox.Items[CategoryComboBox.SelectedIndex] as string;
            current.techCategoryChanged = (current.techCategory != original.techCategory);
        }
        private void FieldNameInput_TextChanged(object sender, EventArgs e)
        {
            current.techFieldName = FieldNameInput.Text.Trim(' ');
            current.techFieldNameChanged = (current.techFieldName != original.techFieldName);
        }
        private void RemovePrereqButton_Click(object sender, EventArgs e)
        {
            if (PrereqsListBox.SelectedIndex >= 0)
            {
                string idString = PrereqsListBox.Items[PrereqsListBox.SelectedIndex] as string;
                idString = idString.Substring(0, 8);
                current.techPrereqs.Remove(HexConverter.HexToInt(idString));
                current.CheckPrereqsChanged(original);
                PopulatePrereqs();
            }
        }
        private void RemoveGrantreqButton_Click(object sender, EventArgs e)
        {
            if (GrantreqsListBox.SelectedIndex >= 0)
            {
                string idString = GrantreqsListBox.Items[GrantreqsListBox.SelectedIndex] as string;
                idString = idString.Substring(0, 8);
                current.techGrantreqs.Remove(HexConverter.HexToInt(idString));
                current.CheckGrantreqsChanged(original);
                PopulateGrantreqs();
            }
        }
        private void RemovePermanizesButton_Click(object sender, EventArgs e)
        {
            if (PermanizesListBox.SelectedIndex >= 0)
            {
                string idString = PermanizesListBox.Items[PermanizesListBox.SelectedIndex] as string;
                idString = idString.Substring(0, 8);
                uint id = HexConverter.HexToInt(idString);
                current.techPermanizes.Remove(id);
                current.CheckPermanizesChanged(original);
                PopulatePermanizes();
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (Mode == ViewMode.ADDING_NEW)
            {
                AddTech();
            }
            else if (Mode == ViewMode.EDITING)
            {
                EditTech();
            }
        }
        private void RevertButton_Click(object sender, EventArgs e)
        {
            Revert();
        }
        private void TechEditView_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message, caption;
            MessageBoxButtons buttons;
            DialogResult result;
            if (Mode == ViewMode.ADDING_NEW && current.AnyAreChanged())
            {
                message = "Save changes before closing?";
                caption = "Unsaved Changes on Form";
                buttons = MessageBoxButtons.YesNoCancel;
                result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            else if (Mode == ViewMode.EDITING && current.AnyAreChanged())
            {
                message = "Save changes before closing?";
                caption = "Unsaved Changes on Form";
                buttons = MessageBoxButtons.YesNoCancel;
                result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            //else if (viewMode == ViewMode.READ_ONLY) { //just let it close }
            if (!(e.Cancel))
            {
                techListView.CloseEditView(EditViewID);
                //Now the form closes and disposes itself
            }
        }
        private void DebugDisplayDataButton_Click(object sender, EventArgs e)
        {
            string dataView = "Show data request received.\r\n" +
                "  Original, Current, changed\r\n" +
                "    " + original.techID + ", " + current.techID + ", " + current.techIDChanged.ToString() + "\r\n" +
                "    " + original.techName + ", " + current.techName + ", " + current.techNameChanged.ToString() + "\r\n" +
                "    " + original.techCategory + ", " + current.techCategory + ", " + current.techCategoryChanged.ToString() + "\r\n" +
                "    " + original.techFieldName + ", " + current.techFieldName + ", " + current.techFieldNameChanged.ToString() + "\r\n" +
                "     (cost per day) " + original.techCostPerDay + ", " + current.techCostPerDay + ", " + current.techCostPerDayChanged.ToString() + "\r\n" +
                "     (number days)  " + original.techNumberDays + ", " + current.techNumberDays + ", " + current.techNumberDaysChanged.ToString() + "\r\n";
            dataView += "    Original prereqs: ";
            foreach (uint id in original.techPrereqs) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Current prereqs: ";
            foreach (uint id in current.techPrereqs) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Prereqs changed: " + current.techPrereqsChanged.ToString() + "\r\n";
            dataView += "    Original grantreqs: ";
            foreach (uint id in original.techGrantreqs) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Current grantreqs: ";
            foreach (uint id in current.techGrantreqs) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Grantreqs changed: " + current.techGrantreqsChanged.ToString() + "\r\n";
            dataView += "    Original permanizes: ";
            foreach (uint id in original.techPermanizes) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Current permanizes: ";
            foreach (uint id in current.techPermanizes) dataView += HexConverter.IntToHex(id) + ", ";
            dataView += "\r\n    Permanizes changed: " + current.techPermanizesChanged.ToString() + "\r\n";
            techListView.Log(dataView);
        }
        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            SaveButton_Click(sender, e);
            techListView.CloseEditView(EditViewID);
            Dispose();
        }
        private void AlwaysViewSelectedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AlwaysViewSelectedCheckBox.Checked)
            {
                techListView.AddObserver(this);
            }
            else
            {
                techListView.RemoveObserver(this);
            }
        }
        
    }
}
