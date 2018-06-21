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
        private ViewMode viewMode;
        private MySqlConnection connection;
        //Data
        private class TechDataContainer
        {
            //Data fields
            public uint techID;
            public bool techIDChanged = false;
            public float techCostPerDay;
            public bool techCostPerDayChanged = false;
            public float techNumberDays;
            public bool techNumberDaysChanged = false;
            public string techName;
            public bool techNameChanged = false;
            public string techCategory;
            public bool techCategoryChanged = false;
            public string techFieldName;
            public bool techFieldNameChanged = false;
            public List<uint> techPrereqs;
            public bool techPrereqsChanged = false;
            public List<uint> techGrantreqs;
            public bool techhGrantreqsChanged = false;
            public List<uint> techPermanizes;
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

        }
        private TechDataContainer current, original;

        //Constructor
        public TechEditView(uint id = 0, ViewMode mode = ViewMode.ADDING_NEW)
        {
            InitializeComponent();
            connection = new MySqlConnection(Properties.Settings.Default.dbConnectionString);
            current = new TechDataContainer();
            original = new TechDataContainer();
            if (id != 0) ViewTech(id);
            viewMode = mode;
            if (mode == ViewMode.ADDING_NEW)
            {
                RevertBehaviorDisplay.Visible = false;
                RevertButton.Enabled = false;
                SaveBehaviorDisplay.Text = "Adds this tech to the database";
                OtherInformationGroupBox.Enabled = false;
                OtherInformationGroupBox.Visible = false;
            }
            else if (mode == ViewMode.READ_ONLY)
            {
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

            }
            else if (mode == ViewMode.EDITING)
            {
                SaveBehaviorDisplay.Text = 
                    "Updates the Tech. If the ID was\r\n" +
                    "changed, references will update.";
            }
            UpdateTitleBar();
        }

        //Changes the title bar to reflect current editing mode and tech name
        public void UpdateTitleBar()
        {
            if (viewMode == ViewMode.READ_ONLY)
                Text = "Viewing Tech: " + original.techName + " (" +
                    HexConverter.IntToHex(original.techID) + ")";
            else if (viewMode == ViewMode.ADDING_NEW)
                Text = "Add Tech: " + current.techName + " (" +
                    HexConverter.IntToHex(current.techID) + ")";
            else if (viewMode == ViewMode.EDITING)
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
            if (viewMode != ViewMode.EDITING) return; 
            //TODO
        }

        //Adds a new tech
        private void AddTech()
        {
            //Only allowed while in add mode
            if (viewMode != ViewMode.ADDING_NEW) return;
            //TODO
        }
    
        //Event handlers
        private void IDInput_TextChanged(object sender, EventArgs e)
        {
            //TODO
        }
        private void CostPerDayInput_TextChanged(object sender, EventArgs e)
        {
            //TODO
        }
        private void NumberDaysInput_TextChanged(object sender, EventArgs e)
        {
            //TODO
        }
        private void NameInput_TextChanged(object sender, EventArgs e)
        {
            //TODO
        }
        private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO
        }
        private void FieldNameInput_TextChanged(object sender, EventArgs e)
        {
            //TODO
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
            //TODO
        }
        private void RevertButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
        private void AlwaysViewSelectedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //TODO
        }



    }
}
