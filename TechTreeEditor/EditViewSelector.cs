using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechTreeEditor
{
    public partial class EditViewSelector : Form
    {
        public int SelectedViewIndex { get; private set; }
        private List<int> viewIndices;

        public EditViewSelector(ref List<TechEditView> techEditViews)
        {
            InitializeComponent();
            viewIndices = new List<int>();
            for (int i = 0; i < techEditViews.Count; i++)
            {
                if (techEditViews[i].Mode == TechEditView.ViewMode.ADDING_NEW ||
                    techEditViews[i].Mode == TechEditView.ViewMode.EDITING)
                {
                    viewComboBox.Items.Add(techEditViews[i].Text);
                    viewIndices.Add(i);
                }
            }
            viewComboBox.SelectedIndex = 0;
            SelectedViewIndex = -1;
        }


        //Processes request for the selected editing form
        private void OkButton_Click(object sender, EventArgs e)
        {
            //callback.Invoke(viewIndices[viewComboBox.SelectedIndex], idToAdd);
            //Dispose();
            SelectedViewIndex = viewIndices[viewComboBox.SelectedIndex];
            Close();
        }

        //Closes the dialog
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Sets focus to Ok button upon visible
        private void EditViewSelector_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                OkButton.Focus();
        }
    }
}
