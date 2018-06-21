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
            this.TechID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TechName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TechCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hammertechtreedbDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.hammertechtreedbDataSet = new TechTreeEditor.hammertechtreedbDataSet();
            this.TechListGroupBox = new System.Windows.Forms.GroupBox();
            this.UpdateFiltersButton = new System.Windows.Forms.Button();
            this.ClearFiltersButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.TechListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TechListGrid.AutoGenerateColumns = false;
            this.TechListGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TechListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TechListGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TechID,
            this.TechName,
            this.TechCategory});
            this.TechListGrid.DataSource = this.hammertechtreedbDataSetBindingSource;
            this.TechListGrid.Location = new System.Drawing.Point(6, 71);
            this.TechListGrid.Name = "TechListGrid";
            this.TechListGrid.ReadOnly = true;
            this.TechListGrid.RowHeadersVisible = false;
            this.TechListGrid.Size = new System.Drawing.Size(641, 285);
            this.TechListGrid.TabIndex = 1;
            // 
            // TechID
            // 
            this.TechID.HeaderText = "ID";
            this.TechID.Name = "TechID";
            this.TechID.ReadOnly = true;
            // 
            // TechName
            // 
            this.TechName.HeaderText = "Name";
            this.TechName.Name = "TechName";
            this.TechName.ReadOnly = true;
            // 
            // TechCategory
            // 
            this.TechCategory.HeaderText = "Category";
            this.TechCategory.Name = "TechCategory";
            this.TechCategory.ReadOnly = true;
            // 
            // hammertechtreedbDataSetBindingSource
            // 
            this.hammertechtreedbDataSetBindingSource.DataSource = this.hammertechtreedbDataSet;
            this.hammertechtreedbDataSetBindingSource.Position = 0;
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
            this.TechListGroupBox.Controls.Add(this.UpdateFiltersButton);
            this.TechListGroupBox.Controls.Add(this.ClearFiltersButton);
            this.TechListGroupBox.Controls.Add(this.label4);
            this.TechListGroupBox.Controls.Add(this.textBox5);
            this.TechListGroupBox.Controls.Add(this.label3);
            this.TechListGroupBox.Controls.Add(this.textBox4);
            this.TechListGroupBox.Controls.Add(this.label2);
            this.TechListGroupBox.Controls.Add(this.textBox3);
            this.TechListGroupBox.Controls.Add(this.textBox2);
            this.TechListGroupBox.Controls.Add(this.label1);
            this.TechListGroupBox.Controls.Add(this.textBox1);
            this.TechListGroupBox.Controls.Add(this.TechListGrid);
            this.TechListGroupBox.Location = new System.Drawing.Point(12, 12);
            this.TechListGroupBox.Name = "TechListGroupBox";
            this.TechListGroupBox.Size = new System.Drawing.Size(653, 362);
            this.TechListGroupBox.TabIndex = 2;
            this.TechListGroupBox.TabStop = false;
            this.TechListGroupBox.Text = "Tech List";
            // 
            // UpdateFiltersButton
            // 
            this.UpdateFiltersButton.Location = new System.Drawing.Point(596, 19);
            this.UpdateFiltersButton.Name = "UpdateFiltersButton";
            this.UpdateFiltersButton.Size = new System.Drawing.Size(51, 20);
            this.UpdateFiltersButton.TabIndex = 12;
            this.UpdateFiltersButton.Text = "Update";
            this.UpdateFiltersButton.UseVisualStyleBackColor = true;
            this.UpdateFiltersButton.Click += new System.EventHandler(this.UpdateFiltersButton_Click);
            // 
            // ClearFiltersButton
            // 
            this.ClearFiltersButton.Location = new System.Drawing.Point(597, 45);
            this.ClearFiltersButton.Name = "ClearFiltersButton";
            this.ClearFiltersButton.Size = new System.Drawing.Size(51, 20);
            this.ClearFiltersButton.TabIndex = 11;
            this.ClearFiltersButton.Text = "Clear";
            this.ClearFiltersButton.UseVisualStyleBackColor = true;
            this.ClearFiltersButton.Click += new System.EventHandler(this.ClearFiltersButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Field name:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(385, 45);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(206, 20);
            this.textBox5.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(281, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name string match:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(385, 19);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(206, 20);
            this.textBox4.TabIndex = 7;
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
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(68, 45);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(206, 20);
            this.textBox3.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(174, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 4;
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(68, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
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
            this.ShowTablesButton.Location = new System.Drawing.Point(671, 351);
            this.ShowTablesButton.Name = "ShowTablesButton";
            this.ShowTablesButton.Size = new System.Drawing.Size(86, 23);
            this.ShowTablesButton.TabIndex = 6;
            this.ShowTablesButton.Text = "Show Tables";
            this.ShowTablesButton.UseVisualStyleBackColor = true;
            this.ShowTablesButton.Click += new System.EventHandler(this.ShowTablesButton_Click);
            // 
            // InitializeDatabaseButton
            // 
            this.InitializeDatabaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InitializeDatabaseButton.Location = new System.Drawing.Point(671, 322);
            this.InitializeDatabaseButton.Name = "InitializeDatabaseButton";
            this.InitializeDatabaseButton.Size = new System.Drawing.Size(86, 23);
            this.InitializeDatabaseButton.TabIndex = 7;
            this.InitializeDatabaseButton.Text = "Initialize DB";
            this.InitializeDatabaseButton.UseVisualStyleBackColor = true;
            this.InitializeDatabaseButton.Click += new System.EventHandler(this.InitializeDatabaseButton_Click);
            // 
            // AddPrereqButton
            // 
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
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button AddTechButton;
        private System.Windows.Forms.Button DeleteTechButton;
        private System.Windows.Forms.Button EditTechButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn TechID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TechName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TechCategory;
        private System.Windows.Forms.Button ShowTablesButton;
        private System.Windows.Forms.Button InitializeDatabaseButton;
        private System.Windows.Forms.Button AddPrereqButton;
        private System.Windows.Forms.Button AddGrantreqButton;
        private System.Windows.Forms.Button AddPermanizesButton;
        private System.Windows.Forms.Button ViewTechButton;
    }
}