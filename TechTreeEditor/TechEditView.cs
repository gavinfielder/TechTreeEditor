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
    public partial class TechEditView : Form
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
            public bool techPermanizesSelf = false;

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
                techPermanizesSelf = other.techPermanizesSelf;
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
            FetchPrereqs();
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
            FetchGrantreqs();
        }
        //Adds a permanizes. Called by TechListView when user clicks add prereq
        public void AddPermanizes(uint id)
        {
            //Not allowed in read only mode
            if (Mode == ViewMode.READ_ONLY) return;
            current.techPermanizes.Add(id);
            current.CheckPermanizesChanged(original);
            FetchPermanizes();
        }

        //Validates the form data. Informs user of problems
        private bool ValidateForm()
        {
            //ID self validates
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
            //TODO
        }
        //Updates an existing record with the updated information
        private void EditTech(uint id)
        {
            //Only allowed while in edit mode
            if (Mode != ViewMode.EDITING) return; 
            //TODO
        }
        //Adds a new tech
        private void AddTech()
        {
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
            //TODO
            //Once done, change mode to edit and set the original to the new record
            Mode = ViewMode.EDITING;
            current.SetAllChanged(false);
            original.Copy(current);
            UpdateTitleBar();
        }
        
        //*********************************************************************
        //************************ Database Functions *************************
        //*********************************************************************

        //Inserts current as a new record into tech table
        private void InsertCurrentTech()
        {
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
                techListView.Log("An error occurred: " + ex.Message);
            }
            finally
            {
                techListView.Log("Tech inserted: " + current.techName);
                connection.Close();
            }
        }
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
                MessageBox.Show("An error occurred while loading categories: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates the Prereqs list box
        private void FetchPrereqs()
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
                techListView.Log("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates the Grantreqs list box
        private void FetchGrantreqs()
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
                techListView.Log("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        //Populates the Permanizes list box
        private void FetchPermanizes()
        {
            //TODO
        }
        //Inserts all prereqs as new prereq relationships
        private void InsertPrereqs()
        {
            if (current.techPrereqs.Count == 0) return;
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
                //techListView.Log("Inserting into tech_prereqs with command = \"" +
                    //command.CommandText + "\"");
                command.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
                techListView.Log(numberOfPrereqs + " prerequisites inserted.");
            }
        }
        //Inserts all grantreqs as new grantreq relationships
        private void InsertGrantreqs()
        {
            if (current.techGrantreqs.Count == 0) return;
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
                //techListView.Log("Inserting " + numberOfGrantreqs + " records into tech_grantreqs with command = \"" +
                    //command.CommandText + "\"");
                command.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred: " + ex.Message);
            }
            finally
            {
                connection.Close();
                techListView.Log(numberOfGrantreqs + " grantrequisites inserted.");
            }
        }

        //*********************************************************************
        //************************** Event Handlers ***************************
        //*********************************************************************
        private void IDInput_TextChanged(object sender, EventArgs e)
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
                }
            }
            current.techID = HexConverter.HexToInt(IDInput.Text);
            current.techIDChanged = (current.techID != original.techID);
            UpdateTitleBar();
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
            //TODO
        }
        private void RemoveGrantreqButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void RemovePermanizesButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (Mode == ViewMode.ADDING_NEW)
            {
                AddTech();
            }
            else if (Mode == ViewMode.EDITING)
            {
                //TODO
            }
        }
        private void RevertButton_Click(object sender, EventArgs e)
        {
            //TODO
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
            else if (Mode == ViewMode.EDITING)
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
        private void PermanizesSelfButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AlwaysViewSelectedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //TODO
        }



    }
}
