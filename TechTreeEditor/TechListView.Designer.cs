namespace TechTreeEditor
{
    partial class TechListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LogGroupBox = new System.Windows.Forms.GroupBox();
            this.LogDisplay = new System.Windows.Forms.TextBox();
            this.TechListGrid = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hammertechtreedbDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.hammertechtreedbDataSet = new TechTreeEditor.hammertechtreedbDataSet();
            this.TechListGroupBox = new System.Windows.Forms.GroupBox();
            this.CategoryComboBox = new System.Windows.Forms.ComboBox();
            this.UpdateFiltersButton = new System.Windows.Forms.Button();
            this.ClearFiltersButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.FieldNameInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NameFilterInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IDRangeMaxInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.IDRangeMinInput = new System.Windows.Forms.TextBox();
            this.AddTechButton = new System.Windows.Forms.Button();
            this.DeleteTechButton = new System.Windows.Forms.Button();
            this.EditTechButton = new System.Windows.Forms.Button();
            this.ShowTablesButton = new System.Windows.Forms.Button();
            this.InitializeDatabaseButton = new System.Windows.Forms.Button();
            this.AddPrereqButton = new System.Windows.Forms.Button();
            this.AddGrantreqButton = new System.Windows.Forms.Button();
            this.AddPermanizesButton = new System.Windows.Forms.Button();
            this.ViewTechButton = new System.Windows.Forms.Button();
            this.LogGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TechListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hammertechtreedbDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hammertechtreedbDataSet)).BeginInit();
            this.TechListGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogGroupBox
            // 
            this.LogGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogGroupBox.Controls.Add(this.LogDisplay);
            this.LogGroupBox.Location = new System.Drawing.Point(12, 380);
            this.LogGroupBox.Name = "LogGroupBox";
            this.LogGroupBox.Size = new System.Drawing.Size(744, 116);
            this.LogGroupBox.TabIndex = 0;
            this.LogGroupBox.TabStop = false;
            this.LogGroupBox.Text = "Log";
            // 
            // LogDisplay
            // 
            this.LogDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogDisplay.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogDisplay.Location = new System.Drawing.Point(6, 19);
            this.LogDisplay.Multiline = true;
            this.LogDisplay.Name = "LogDisplay";
            this.LogDisplay.ReadOnly = true;
            this.LogDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogDisplay.Size = new System.Drawing.Size(732, 91);
            this.LogDisplay.TabIndex = 0;
            // 
            // TechListGrid
            // 
            this.TechListGrid.AllowUserToAddRows = false;
            this.TechListGrid.AllowUserToDeleteRows = false;
            this.TechListGrid.AllowUserToResizeRows = false;
            this.TechListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TechListGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TechListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TechListGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.category});
            this.TechListGrid.Location = new System.Drawing.Point(6, 71);
            this.TechListGrid.MultiSelect = false;
            this.TechListGrid.Name = "TechListGrid";
            this.TechListGrid.ReadOnly = true;
            this.TechListGrid.RowHeadersVisible = false;
            this.TechListGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TechListGrid.Size = new System.Drawing.Size(641, 285);
            this.TechListGrid.TabIndex = 1;
            this.TechListGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TechListGrid_CellDoubleClick);
            this.TechListGrid.SelectionChanged += new System.EventHandler(this.TechListGrid_SelectionChanged);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // category
            // 
            this.category.HeaderText = "Category";
            this.category.Name = "category";
            this.category.ReadOnly = true;
            // 
            // hammertechtreedbDataSet
            // 
            this.hammertechtreedbDataSet.DataSetName = "hammertechtreedbDataSet";
            this.hammertechtreedbDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TechListGroupBox
            // 
            this.TechListGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TechListGroupBox.Controls.Add(this.CategoryComboBox);
            this.TechListGroupBox.Controls.Add(this.UpdateFiltersButton);
            this.TechListGroupBox.Controls.Add(this.ClearFiltersButton);
            this.TechListGroupBox.Controls.Add(this.label4);
            this.TechListGroupBox.Controls.Add(this.FieldNameInput);
            this.TechListGroupBox.Controls.Add(this.label3);
            this.TechListGroupBox.Controls.Add(this.NameFilterInput);
            this.TechListGroupBox.Controls.Add(this.label2);
            this.TechListGroupBox.Controls.Add(this.IDRangeMaxInput);
            this.TechListGroupBox.Controls.Add(this.label1);
            this.TechListGroupBox.Controls.Add(this.IDRangeMinInput);
            this.TechListGroupBox.Controls.Add(this.TechListGrid);
            this.TechListGroupBox.Location = new System.Drawing.Point(12, 12);
            this.TechListGroupBox.Name = "TechListGroupBox";
            this.TechListGroupBox.Size = new System.Drawing.Size(653, 362);
            this.TechListGroupBox.TabIndex = 2;
            this.TechListGroupBox.TabStop = false;
            this.TechListGroupBox.Text = "Tech List";
            // 
            // CategoryComboBox
            // 
            this.CategoryComboBox.FormattingEnabled = true;
            this.CategoryComboBox.Location = new System.Drawing.Point(68, 44);
            this.CategoryComboBox.Name = "CategoryComboBox";
            this.CategoryComboBox.Size = new System.Drawing.Size(150, 21);
            this.CategoryComboBox.TabIndex = 13;
            // 
            // UpdateFiltersButton
            // 
            this.UpdateFiltersButton.Location = new System.Drawing.Point(487, 19);
            this.UpdateFiltersButton.Name = "UpdateFiltersButton";
            this.UpdateFiltersButton.Size = new System.Drawing.Size(82, 20);
            this.UpdateFiltersButton.TabIndex = 12;
            this.UpdateFiltersButton.Text = "Update Filters";
            this.UpdateFiltersButton.UseVisualStyleBackColor = true;
            this.UpdateFiltersButton.Click += new System.EventHandler(this.UpdateFiltersButton_Click);
            // 
            // ClearFiltersButton
            // 
            this.ClearFiltersButton.Location = new System.Drawing.Point(487, 45);
            this.ClearFiltersButton.Name = "ClearFiltersButton";
            this.ClearFiltersButton.Size = new System.Drawing.Size(82, 20);
            this.ClearFiltersButton.TabIndex = 11;
            this.ClearFiltersButton.Text = "Clear Filters";
            this.ClearFiltersButton.UseVisualStyleBackColor = true;
            this.ClearFiltersButton.Click += new System.EventHandler(this.ClearFiltersButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Field name:";
            // 
            // FieldNameInput
            // 
            this.FieldNameInput.Location = new System.Drawing.Point(330, 45);
            this.FieldNameInput.Name = "FieldNameInput";
            this.FieldNameInput.Size = new System.Drawing.Size(151, 20);
            this.FieldNameInput.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name string match:";
            // 
            // NameFilterInput
            // 
            this.NameFilterInput.Location = new System.Drawing.Point(330, 19);
            this.NameFilterInput.Name = "NameFilterInput";
            this.NameFilterInput.Size = new System.Drawing.Size(151, 20);
            this.NameFilterInput.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Category:";
            // 
            // IDRangeMaxInput
            // 
            this.IDRangeMaxInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IDRangeMaxInput.Location = new System.Drawing.Point(146, 19);
            this.IDRangeMaxInput.Name = "IDRangeMaxInput";
            this.IDRangeMaxInput.Size = new System.Drawing.Size(72, 20);
            this.IDRangeMaxInput.TabIndex = 4;
            this.IDRangeMaxInput.Text = "FFFFFFFF";
            this.IDRangeMaxInput.TextChanged += new System.EventHandler(this.IDRangeMaxInput_TextChanged);
            this.IDRangeMaxInput.Leave += new System.EventHandler(this.IDRangeMaxInput_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID Range:";
            // 
            // IDRangeMinInput
            // 
            this.IDRangeMinInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IDRangeMinInput.Location = new System.Drawing.Point(68, 19);
            this.IDRangeMinInput.Name = "IDRangeMinInput";
            this.IDRangeMinInput.Size = new System.Drawing.Size(72, 20);
            this.IDRangeMinInput.TabIndex = 2;
            this.IDRangeMinInput.Text = "00000000";
            this.IDRangeMinInput.TextChanged += new System.EventHandler(this.IDRangeMinInput_TextChanged);
            this.IDRangeMinInput.Leave += new System.EventHandler(this.IDRangeMinInput_Leave);
            // 
            // AddTechButton
            // 
            this.AddTechButton.Location = new System.Drawing.Point(671, 83);
            this.AddTechButton.Name = "AddTechButton";
            this.AddTechButton.Size = new System.Drawing.Size(86, 23);
            this.AddTechButton.TabIndex = 3;
            this.AddTechButton.Text = "Add Tech";
            this.AddTechButton.UseVisualStyleBackColor = true;
            this.AddTechButton.Click += new System.EventHandler(this.AddTechButton_Click);
            // 
            // DeleteTechButton
            // 
            this.DeleteTechButton.Enabled = false;
            this.DeleteTechButton.Location = new System.Drawing.Point(671, 170);
            this.DeleteTechButton.Name = "DeleteTechButton";
            this.DeleteTechButton.Size = new System.Drawing.Size(86, 23);
            this.DeleteTechButton.TabIndex = 4;
            this.DeleteTechButton.Text = "Delete Tech";
            this.DeleteTechButton.UseVisualStyleBackColor = true;
            this.DeleteTechButton.Click += new System.EventHandler(this.DeleteTechButton_Click);
            // 
            // EditTechButton
            // 
            this.EditTechButton.Enabled = false;
            this.EditTechButton.Location = new System.Drawing.Point(671, 112);
            this.EditTechButton.Name = "EditTechButton";
            this.EditTechButton.Size = new System.Drawing.Size(86, 23);
            this.EditTechButton.TabIndex = 5;
            this.EditTechButton.Text = "Edit Tech";
            this.EditTechButton.UseVisualStyleBackColor = true;
            this.EditTechButton.Click += new System.EventHandler(this.EditTechButton_Click);
            // 
            // ShowTablesButton
            // 
            this.ShowTablesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowTablesButton.Enabled = false;
            this.ShowTablesButton.Location = new System.Drawing.Point(671, 351);
            this.ShowTablesButton.Name = "ShowTablesButton";
            this.ShowTablesButton.Size = new System.Drawing.Size(86, 23);
            this.ShowTablesButton.TabIndex = 6;
            this.ShowTablesButton.Text = "Show Tables";
            this.ShowTablesButton.UseVisualStyleBackColor = true;
            this.ShowTablesButton.Visible = false;
            this.ShowTablesButton.Click += new System.EventHandler(this.ShowTablesButton_Click);
            // 
            // InitializeDatabaseButton
            // 
            this.InitializeDatabaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InitializeDatabaseButton.Enabled = false;
            this.InitializeDatabaseButton.Location = new System.Drawing.Point(671, 322);
            this.InitializeDatabaseButton.Name = "InitializeDatabaseButton";
            this.InitializeDatabaseButton.Size = new System.Drawing.Size(86, 23);
            this.InitializeDatabaseButton.TabIndex = 7;
            this.InitializeDatabaseButton.Text = "Initialize DB";
            this.InitializeDatabaseButton.UseVisualStyleBackColor = true;
            this.InitializeDatabaseButton.Visible = false;
            this.InitializeDatabaseButton.Click += new System.EventHandler(this.InitializeDatabaseButton_Click);
            // 
            // AddPrereqButton
            // 
            this.AddPrereqButton.Enabled = false;
            this.AddPrereqButton.Location = new System.Drawing.Point(671, 217);
            this.AddPrereqButton.Name = "AddPrereqButton";
            this.AddPrereqButton.Size = new System.Drawing.Size(86, 23);
            this.AddPrereqButton.TabIndex = 8;
            this.AddPrereqButton.Text = "Add Prereq";
            this.AddPrereqButton.UseVisualStyleBackColor = true;
            this.AddPrereqButton.Click += new System.EventHandler(this.AddPrereqButton_Click);
            // 
            // AddGrantreqButton
            // 
            this.AddGrantreqButton.Enabled = false;
            this.AddGrantreqButton.Location = new System.Drawing.Point(671, 246);
            this.AddGrantreqButton.Name = "AddGrantreqButton";
            this.AddGrantreqButton.Size = new System.Drawing.Size(86, 23);
            this.AddGrantreqButton.TabIndex = 9;
            this.AddGrantreqButton.Text = "Add Grantreq";
            this.AddGrantreqButton.UseVisualStyleBackColor = true;
            this.AddGrantreqButton.Click += new System.EventHandler(this.AddGrantreqButton_Click);
            // 
            // AddPermanizesButton
            // 
            this.AddPermanizesButton.Enabled = false;
            this.AddPermanizesButton.Location = new System.Drawing.Point(671, 275);
            this.AddPermanizesButton.Name = "AddPermanizesButton";
            this.AddPermanizesButton.Size = new System.Drawing.Size(86, 23);
            this.AddPermanizesButton.TabIndex = 10;
            this.AddPermanizesButton.Text = "Permanizes";
            this.AddPermanizesButton.UseVisualStyleBackColor = true;
            this.AddPermanizesButton.Click += new System.EventHandler(this.AddPermanizesButton_Click);
            // 
            // ViewTechButton
            // 
            this.ViewTechButton.Enabled = false;
            this.ViewTechButton.Location = new System.Drawing.Point(671, 141);
            this.ViewTechButton.Name = "ViewTechButton";
            this.ViewTechButton.Size = new System.Drawing.Size(86, 23);
            this.ViewTechButton.TabIndex = 11;
            this.ViewTechButton.Text = "View Tech";
            this.ViewTechButton.UseVisualStyleBackColor = true;
            this.ViewTechButton.Click += new System.EventHandler(this.ViewTechButton_Click);
            // 
            // TechListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 508);
            this.Controls.Add(this.ViewTechButton);
            this.Controls.Add(this.AddPermanizesButton);
            this.Controls.Add(this.AddGrantreqButton);
            this.Controls.Add(this.AddPrereqButton);
            this.Controls.Add(this.InitializeDatabaseButton);
            this.Controls.Add(this.ShowTablesButton);
            this.Controls.Add(this.EditTechButton);
            this.Controls.Add(this.DeleteTechButton);
            this.Controls.Add(this.AddTechButton);
            this.Controls.Add(this.TechListGroupBox);
            this.Controls.Add(this.LogGroupBox);
            this.Name = "TechListView";
            this.Text = "View Tech List";
            this.LogGroupBox.ResumeLayout(false);
            this.LogGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TechListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hammertechtreedbDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hammertechtreedbDataSet)).EndInit();
            this.TechListGroupBox.ResumeLayout(false);
            this.TechListGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LogGroupBox;
        private System.Windows.Forms.TextBox LogDisplay;
        private System.Windows.Forms.DataGridView TechListGrid;
        private System.Windows.Forms.BindingSource hammertechtreedbDataSetBindingSource;
        private hammertechtreedbDataSet hammertechtreedbDataSet;
        private System.Windows.Forms.GroupBox TechListGroupBox;
        private System.Windows.Forms.Button UpdateFiltersButton;
        private System.Windows.Forms.Button ClearFiltersButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FieldNameInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NameFilterInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IDRangeMaxInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IDRangeMinInput;
        private System.Windows.Forms.Button AddTechButton;
        private System.Windows.Forms.Button DeleteTechButton;
        private System.Windows.Forms.Button EditTechButton;
        private System.Windows.Forms.Button ShowTablesButton;
        private System.Windows.Forms.Button InitializeDatabaseButton;
        private System.Windows.Forms.Button AddPrereqButton;
        private System.Windows.Forms.Button AddGrantreqButton;
        private System.Windows.Forms.Button AddPermanizesButton;
        private System.Windows.Forms.Button ViewTechButton;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
    }
}