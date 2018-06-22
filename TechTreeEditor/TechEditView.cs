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
        public enum ViewMode
        {
            ADDING_NEW,
            EDITING,
            READ_ONLY
        };
        //Fields
        private ViewMode viewMode_internal; //Do not use outside of below property
        public ViewMode Mode {
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
        private MySqlConnection connection;
        public TechListView techListView; //set by TechListView
        private Regex IDRgx;
        private Regex alphaRgx;
        public int EditViewID { get; }
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
            public bool techhGrantreqsChanged = false;
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
                techhGrantreqsChanged = changed;
                techPermanizesChanged = changed;
            }

            //If any fields are changed, return true
            public bool AnyAreChanged()
            {
                return (techIDChanged || techCostPerDayChanged ||
                    techNumberDaysChanged || techNameChanged ||
                    techCategoryChanged || techFieldNameChanged ||
                    techPrereqsChanged || techhGrantreqsChanged ||
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

        }
        private TechDataContainer current, original;

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
            //TODO
            //Insert new tech grantrequisites
            //TODO
            //Insert new tech permanizes
            //TODO
            //Once done, change mode to edit and set the original to the new record
            Mode = ViewMode.EDITING;
            original.Copy(current);
            UpdateTitleBar();
        }

        //Database functions

        //Inserts current as a new record into tech table
        private void InsertCurrentTech()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT tech " +
                "VALUES(" + current.techID + ",'" + current.techName + "','" +
                current.techCategory + "','" + current.techFieldName + "'," +
                current.techCostPerDay + "," + current.techNumberDays + ");";
            techListView.Log("Inserting tech with command: \"" + command.CommandText); 
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                //command.BeginExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                techListView.Log("An error occurred: " + ex.Message);
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                techListView.Log("Closing connection.");
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

        //Validates the form data
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
    
        //Event handlers
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
        private void AlwaysViewSelectedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //TODO
        }



    }
}
