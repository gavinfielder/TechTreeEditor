namespace TechTreeEditor
{
    partial class TechEditView
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
            this.IDInput = new System.Windows.Forms.TextBox();
            this.IDInputLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PrereqsListBox = new System.Windows.Forms.ListBox();
            this.GrantreqsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PermanizesListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FieldNameInput = new System.Windows.Forms.TextBox();
            this.CostPerDayInput = new System.Windows.Forms.TextBox();
            this.NumberDaysInput = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.RemovePrereqButton = new System.Windows.Forms.Button();
            this.RemoveGrantreqButton = new System.Windows.Forms.Button();
            this.RemovePermanizesButton = new System.Windows.Forms.Button();
            this.CategoryComboBox = new System.Windows.Forms.ComboBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveBehaviorDisplay = new System.Windows.Forms.Label();
            this.RevertButton = new System.Windows.Forms.Button();
            this.IsPrereqForListBox = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.IsGrantreqForListBox = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OtherInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.IsPermanizedByListBox = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RevertBehaviorDisplay = new System.Windows.Forms.Label();
            this.AlwaysViewSelectedCheckBox = new System.Windows.Forms.CheckBox();
            this.PermanizesSelfButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.OtherInformationGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // IDInput
            // 
            this.IDInput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IDInput.Location = new System.Drawing.Point(82, 17);
            this.IDInput.MaxLength = 8;
            this.IDInput.Name = "IDInput";
            this.IDInput.Size = new System.Drawing.Size(106, 20);
            this.IDInput.TabIndex = 0;
            this.IDInput.Text = "00000000";
            this.IDInput.TextChanged += new System.EventHandler(this.IDInput_TextChanged);
            this.IDInput.Leave += new System.EventHandler(this.IDInput_Leave);
            // 
            // IDInputLabel
            // 
            this.IDInputLabel.AutoSize = true;
            this.IDInputLabel.Location = new System.Drawing.Point(47, 20);
            this.IDInputLabel.Name = "IDInputLabel";
            this.IDInputLabel.Size = new System.Drawing.Size(38, 13);
            this.IDInputLabel.TabIndex = 1;
            this.IDInputLabel.Text = "ID:  0x";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(226, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:";
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(270, 17);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(243, 20);
            this.NameInput.TabIndex = 2;
            this.NameInput.TextChanged += new System.EventHandler(this.NameInput_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Prerequisites";
            // 
            // PrereqsListBox
            // 
            this.PrereqsListBox.FormattingEnabled = true;
            this.PrereqsListBox.Location = new System.Drawing.Point(6, 34);
            this.PrereqsListBox.Name = "PrereqsListBox";
            this.PrereqsListBox.Size = new System.Drawing.Size(219, 95);
            this.PrereqsListBox.TabIndex = 5;
            // 
            // GrantreqsListBox
            // 
            this.GrantreqsListBox.FormattingEnabled = true;
            this.GrantreqsListBox.Location = new System.Drawing.Point(6, 149);
            this.GrantreqsListBox.Name = "GrantreqsListBox";
            this.GrantreqsListBox.Size = new System.Drawing.Size(219, 95);
            this.GrantreqsListBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Grantrequisites";
            // 
            // PermanizesListBox
            // 
            this.PermanizesListBox.FormattingEnabled = true;
            this.PermanizesListBox.Location = new System.Drawing.Point(6, 267);
            this.PermanizesListBox.Name = "PermanizesListBox";
            this.PermanizesListBox.Size = new System.Drawing.Size(219, 43);
            this.PermanizesListBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Permanizes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Category:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Field Name:";
            // 
            // FieldNameInput
            // 
            this.FieldNameInput.Location = new System.Drawing.Point(270, 70);
            this.FieldNameInput.Name = "FieldNameInput";
            this.FieldNameInput.Size = new System.Drawing.Size(243, 20);
            this.FieldNameInput.TabIndex = 13;
            this.FieldNameInput.TextChanged += new System.EventHandler(this.FieldNameInput_TextChanged);
            // 
            // CostPerDayInput
            // 
            this.CostPerDayInput.Location = new System.Drawing.Point(82, 43);
            this.CostPerDayInput.Name = "CostPerDayInput";
            this.CostPerDayInput.Size = new System.Drawing.Size(106, 20);
            this.CostPerDayInput.TabIndex = 14;
            this.CostPerDayInput.Text = "0.0";
            this.CostPerDayInput.Leave += new System.EventHandler(this.CostPerDayInput_Leave);
            // 
            // NumberDaysInput
            // 
            this.NumberDaysInput.Location = new System.Drawing.Point(82, 70);
            this.NumberDaysInput.Name = "NumberDaysInput";
            this.NumberDaysInput.Size = new System.Drawing.Size(106, 20);
            this.NumberDaysInput.TabIndex = 15;
            this.NumberDaysInput.Text = "0.0";
            this.NumberDaysInput.Leave += new System.EventHandler(this.NumberDaysInput_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Number days:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Cost per day:";
            // 
            // RemovePrereqButton
            // 
            this.RemovePrereqButton.Location = new System.Drawing.Point(231, 100);
            this.RemovePrereqButton.Name = "RemovePrereqButton";
            this.RemovePrereqButton.Size = new System.Drawing.Size(55, 29);
            this.RemovePrereqButton.TabIndex = 18;
            this.RemovePrereqButton.Text = "Remove";
            this.RemovePrereqButton.UseVisualStyleBackColor = true;
            this.RemovePrereqButton.Click += new System.EventHandler(this.RemovePrereqButton_Click);
            // 
            // RemoveGrantreqButton
            // 
            this.RemoveGrantreqButton.Location = new System.Drawing.Point(231, 215);
            this.RemoveGrantreqButton.Name = "RemoveGrantreqButton";
            this.RemoveGrantreqButton.Size = new System.Drawing.Size(55, 29);
            this.RemoveGrantreqButton.TabIndex = 19;
            this.RemoveGrantreqButton.Text = "Remove";
            this.RemoveGrantreqButton.UseVisualStyleBackColor = true;
            this.RemoveGrantreqButton.Click += new System.EventHandler(this.RemoveGrantreqButton_Click);
            // 
            // RemovePermanizesButton
            // 
            this.RemovePermanizesButton.Location = new System.Drawing.Point(231, 286);
            this.RemovePermanizesButton.Name = "RemovePermanizesButton";
            this.RemovePermanizesButton.Size = new System.Drawing.Size(55, 24);
            this.RemovePermanizesButton.TabIndex = 20;
            this.RemovePermanizesButton.Text = "Remove";
            this.RemovePermanizesButton.UseVisualStyleBackColor = true;
            this.RemovePermanizesButton.Click += new System.EventHandler(this.RemovePermanizesButton_Click);
            // 
            // CategoryComboBox
            // 
            this.CategoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryComboBox.FormattingEnabled = true;
            this.CategoryComboBox.Location = new System.Drawing.Point(270, 43);
            this.CategoryComboBox.Name = "CategoryComboBox";
            this.CategoryComboBox.Size = new System.Drawing.Size(243, 21);
            this.CategoryComboBox.TabIndex = 21;
            this.CategoryComboBox.SelectedIndexChanged += new System.EventHandler(this.CategoryComboBox_SelectedIndexChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 449);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(88, 29);
            this.SaveButton.TabIndex = 22;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveBehaviorDisplay
            // 
            this.SaveBehaviorDisplay.AutoSize = true;
            this.SaveBehaviorDisplay.Location = new System.Drawing.Point(106, 449);
            this.SaveBehaviorDisplay.Name = "SaveBehaviorDisplay";
            this.SaveBehaviorDisplay.Size = new System.Drawing.Size(162, 26);
            this.SaveBehaviorDisplay.TabIndex = 23;
            this.SaveBehaviorDisplay.Text = "Updates the Tech. If the ID was \r\nchanged, references will update.\r\n";
            // 
            // RevertButton
            // 
            this.RevertButton.Location = new System.Drawing.Point(310, 449);
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(88, 29);
            this.RevertButton.TabIndex = 24;
            this.RevertButton.Text = "Revert";
            this.RevertButton.UseVisualStyleBackColor = true;
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // IsPrereqForListBox
            // 
            this.IsPrereqForListBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.IsPrereqForListBox.FormattingEnabled = true;
            this.IsPrereqForListBox.Location = new System.Drawing.Point(6, 34);
            this.IsPrereqForListBox.Name = "IsPrereqForListBox";
            this.IsPrereqForListBox.Size = new System.Drawing.Size(209, 95);
            this.IsPrereqForListBox.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Is a prereq for:";
            // 
            // IsGrantreqForListBox
            // 
            this.IsGrantreqForListBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.IsGrantreqForListBox.FormattingEnabled = true;
            this.IsGrantreqForListBox.Location = new System.Drawing.Point(6, 149);
            this.IsGrantreqForListBox.Name = "IsGrantreqForListBox";
            this.IsGrantreqForListBox.Size = new System.Drawing.Size(209, 95);
            this.IsGrantreqForListBox.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Is a grantreq for:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PermanizesSelfButton);
            this.groupBox1.Controls.Add(this.PrereqsListBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.GrantreqsListBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.PermanizesListBox);
            this.groupBox1.Controls.Add(this.RemovePrereqButton);
            this.groupBox1.Controls.Add(this.RemoveGrantreqButton);
            this.groupBox1.Controls.Add(this.RemovePermanizesButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 324);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph Connections";
            // 
            // OtherInformationGroupBox
            // 
            this.OtherInformationGroupBox.Controls.Add(this.label11);
            this.OtherInformationGroupBox.Controls.Add(this.IsPermanizedByListBox);
            this.OtherInformationGroupBox.Controls.Add(this.label9);
            this.OtherInformationGroupBox.Controls.Add(this.label10);
            this.OtherInformationGroupBox.Controls.Add(this.IsPrereqForListBox);
            this.OtherInformationGroupBox.Controls.Add(this.IsGrantreqForListBox);
            this.OtherInformationGroupBox.Location = new System.Drawing.Point(310, 119);
            this.OtherInformationGroupBox.Name = "OtherInformationGroupBox";
            this.OtherInformationGroupBox.Size = new System.Drawing.Size(226, 324);
            this.OtherInformationGroupBox.TabIndex = 33;
            this.OtherInformationGroupBox.TabStop = false;
            this.OtherInformationGroupBox.Text = "Other Information";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 251);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "Is permanized by:";
            // 
            // IsPermanizedByListBox
            // 
            this.IsPermanizedByListBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.IsPermanizedByListBox.FormattingEnabled = true;
            this.IsPermanizedByListBox.Location = new System.Drawing.Point(6, 267);
            this.IsPermanizedByListBox.Name = "IsPermanizedByListBox";
            this.IsPermanizedByListBox.Size = new System.Drawing.Size(209, 43);
            this.IsPermanizedByListBox.TabIndex = 32;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.IDInput);
            this.groupBox3.Controls.Add(this.IDInputLabel);
            this.groupBox3.Controls.Add(this.NameInput);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.FieldNameInput);
            this.groupBox3.Controls.Add(this.CategoryComboBox);
            this.groupBox3.Controls.Add(this.CostPerDayInput);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.NumberDaysInput);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(524, 101);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Basic Information";
            // 
            // RevertBehaviorDisplay
            // 
            this.RevertBehaviorDisplay.AutoSize = true;
            this.RevertBehaviorDisplay.Location = new System.Drawing.Point(404, 449);
            this.RevertBehaviorDisplay.Name = "RevertBehaviorDisplay";
            this.RevertBehaviorDisplay.Size = new System.Drawing.Size(108, 26);
            this.RevertBehaviorDisplay.TabIndex = 25;
            this.RevertBehaviorDisplay.Text = "Discard changes and\r\nreload from database";
            // 
            // AlwaysViewSelectedCheckBox
            // 
            this.AlwaysViewSelectedCheckBox.AutoSize = true;
            this.AlwaysViewSelectedCheckBox.Enabled = false;
            this.AlwaysViewSelectedCheckBox.Location = new System.Drawing.Point(12, 484);
            this.AlwaysViewSelectedCheckBox.Name = "AlwaysViewSelectedCheckBox";
            this.AlwaysViewSelectedCheckBox.Size = new System.Drawing.Size(346, 17);
            this.AlwaysViewSelectedCheckBox.TabIndex = 35;
            this.AlwaysViewSelectedCheckBox.Text = "Always view the currently selected tech in list view (View mode only)";
            this.AlwaysViewSelectedCheckBox.UseVisualStyleBackColor = true;
            this.AlwaysViewSelectedCheckBox.Visible = false;
            this.AlwaysViewSelectedCheckBox.CheckedChanged += new System.EventHandler(this.AlwaysViewSelectedCheckBox_CheckedChanged);
            // 
            // PermanizesSelfButton
            // 
            this.PermanizesSelfButton.Location = new System.Drawing.Point(231, 260);
            this.PermanizesSelfButton.Name = "PermanizesSelfButton";
            this.PermanizesSelfButton.Size = new System.Drawing.Size(55, 24);
            this.PermanizesSelfButton.TabIndex = 21;
            this.PermanizesSelfButton.Text = "Add Self";
            this.PermanizesSelfButton.UseVisualStyleBackColor = true;
            this.PermanizesSelfButton.Click += new System.EventHandler(this.PermanizesSelfButton_Click);
            // 
            // TechEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 507);
            this.Controls.Add(this.AlwaysViewSelectedCheckBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.OtherInformationGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RevertBehaviorDisplay);
            this.Controls.Add(this.RevertButton);
            this.Controls.Add(this.SaveBehaviorDisplay);
            this.Controls.Add(this.SaveButton);
            this.MaximumSize = new System.Drawing.Size(564, 546);
            this.Name = "TechEditView";
            this.Text = "View / Edit Tech";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TechEditView_FormClosing);
            this.Enter += new System.EventHandler(this.TechEditView_Enter);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.OtherInformationGroupBox.ResumeLayout(false);
            this.OtherInformationGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IDInput;
        private System.Windows.Forms.Label IDInputLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox PrereqsListBox;
        private System.Windows.Forms.ListBox GrantreqsListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox PermanizesListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox FieldNameInput;
        private System.Windows.Forms.TextBox CostPerDayInput;
        private System.Windows.Forms.TextBox NumberDaysInput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button RemovePrereqButton;
        private System.Windows.Forms.Button RemoveGrantreqButton;
        private System.Windows.Forms.Button RemovePermanizesButton;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label SaveBehaviorDisplay;
        private System.Windows.Forms.Button RevertButton;
        private System.Windows.Forms.ListBox IsPrereqForListBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox IsGrantreqForListBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox OtherInformationGroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox IsPermanizedByListBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label RevertBehaviorDisplay;
        private System.Windows.Forms.CheckBox AlwaysViewSelectedCheckBox;
        private System.Windows.Forms.Button PermanizesSelfButton;
    }
}