namespace DispatchSystem.cl.Windows
{
    partial class DispatchMain
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
            this.civLabel = new MaterialSkin.Controls.MaterialLabel();
            this.firstName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lastName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btnViewCiv = new MaterialSkin.Controls.MaterialRaisedButton();
            this.civVehLabel = new MaterialSkin.Controls.MaterialLabel();
            this.plate = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btnViewCivVeh = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.btnViewBolos = new MaterialSkin.Controls.MaterialRaisedButton();
            this.boloLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialDivider2 = new MaterialSkin.Controls.MaterialDivider();
            this.cadLabel = new MaterialSkin.Controls.MaterialLabel();
            this.viewOfficers = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnViewAssignments = new MaterialSkin.Controls.MaterialRaisedButton();
            this.SuspendLayout();
            // 
            // civLabel
            // 
            this.civLabel.AutoSize = true;
            this.civLabel.Depth = 0;
            this.civLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.civLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.civLabel.Location = new System.Drawing.Point(12, 77);
            this.civLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.civLabel.Name = "civLabel";
            this.civLabel.Size = new System.Drawing.Size(62, 19);
            this.civLabel.TabIndex = 0;
            this.civLabel.Text = "Civilian:";
            // 
            // firstName
            // 
            this.firstName.Depth = 0;
            this.firstName.Hint = "First Name";
            this.firstName.Location = new System.Drawing.Point(16, 108);
            this.firstName.MaxLength = 32767;
            this.firstName.MouseState = MaterialSkin.MouseState.HOVER;
            this.firstName.Name = "firstName";
            this.firstName.PasswordChar = '\0';
            this.firstName.ReadOnly = false;
            this.firstName.SelectedText = "";
            this.firstName.SelectionLength = 0;
            this.firstName.SelectionStart = 0;
            this.firstName.Size = new System.Drawing.Size(129, 23);
            this.firstName.TabIndex = 1;
            this.firstName.TabStop = false;
            this.firstName.UseSystemPasswordChar = false;
            this.firstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnFirstNameKeyPress);
            // 
            // lastName
            // 
            this.lastName.Depth = 0;
            this.lastName.Hint = "Last Name";
            this.lastName.Location = new System.Drawing.Point(16, 153);
            this.lastName.MaxLength = 32767;
            this.lastName.MouseState = MaterialSkin.MouseState.HOVER;
            this.lastName.Name = "lastName";
            this.lastName.PasswordChar = '\0';
            this.lastName.ReadOnly = false;
            this.lastName.SelectedText = "";
            this.lastName.SelectionLength = 0;
            this.lastName.SelectionStart = 0;
            this.lastName.Size = new System.Drawing.Size(129, 23);
            this.lastName.TabIndex = 2;
            this.lastName.TabStop = false;
            this.lastName.UseSystemPasswordChar = false;
            this.lastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnLastNameKeyPress);
            // 
            // btnViewCiv
            // 
            this.btnViewCiv.AutoSize = true;
            this.btnViewCiv.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewCiv.Depth = 0;
            this.btnViewCiv.Icon = null;
            this.btnViewCiv.Location = new System.Drawing.Point(16, 197);
            this.btnViewCiv.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewCiv.Name = "btnViewCiv";
            this.btnViewCiv.Primary = true;
            this.btnViewCiv.Size = new System.Drawing.Size(113, 36);
            this.btnViewCiv.TabIndex = 3;
            this.btnViewCiv.Text = "View Civilian";
            this.btnViewCiv.UseVisualStyleBackColor = true;
            this.btnViewCiv.Click += new System.EventHandler(this.OnViewCivClick);
            // 
            // civVehLabel
            // 
            this.civVehLabel.AutoSize = true;
            this.civVehLabel.Depth = 0;
            this.civVehLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.civVehLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.civVehLabel.Location = new System.Drawing.Point(310, 77);
            this.civVehLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.civVehLabel.Name = "civVehLabel";
            this.civVehLabel.Size = new System.Drawing.Size(116, 19);
            this.civVehLabel.TabIndex = 4;
            this.civVehLabel.Text = "Civilian Vehicle:";
            // 
            // plate
            // 
            this.plate.Depth = 0;
            this.plate.Hint = "License Plate";
            this.plate.Location = new System.Drawing.Point(314, 108);
            this.plate.MaxLength = 37282;
            this.plate.MouseState = MaterialSkin.MouseState.HOVER;
            this.plate.Name = "plate";
            this.plate.PasswordChar = '\0';
            this.plate.ReadOnly = false;
            this.plate.SelectedText = "";
            this.plate.SelectionLength = 0;
            this.plate.SelectionStart = 0;
            this.plate.Size = new System.Drawing.Size(175, 23);
            this.plate.TabIndex = 5;
            this.plate.TabStop = false;
            this.plate.UseSystemPasswordChar = false;
            this.plate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnPlateKeyPress);
            // 
            // btnViewCivVeh
            // 
            this.btnViewCivVeh.AutoSize = true;
            this.btnViewCivVeh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewCivVeh.Depth = 0;
            this.btnViewCivVeh.Icon = null;
            this.btnViewCivVeh.Location = new System.Drawing.Point(314, 197);
            this.btnViewCivVeh.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewCivVeh.Name = "btnViewCivVeh";
            this.btnViewCivVeh.Primary = true;
            this.btnViewCivVeh.Size = new System.Drawing.Size(171, 36);
            this.btnViewCivVeh.TabIndex = 6;
            this.btnViewCivVeh.Text = "View Civilian Vehicle";
            this.btnViewCivVeh.UseVisualStyleBackColor = true;
            this.btnViewCivVeh.Click += new System.EventHandler(this.OnViewCivVehClick);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(16, 261);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(473, 17);
            this.materialDivider1.TabIndex = 7;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // btnViewBolos
            // 
            this.btnViewBolos.AutoSize = true;
            this.btnViewBolos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewBolos.Depth = 0;
            this.btnViewBolos.Icon = null;
            this.btnViewBolos.Location = new System.Drawing.Point(16, 334);
            this.btnViewBolos.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewBolos.Name = "btnViewBolos";
            this.btnViewBolos.Primary = true;
            this.btnViewBolos.Size = new System.Drawing.Size(100, 36);
            this.btnViewBolos.TabIndex = 8;
            this.btnViewBolos.Text = "View Bolos";
            this.btnViewBolos.UseVisualStyleBackColor = true;
            this.btnViewBolos.Click += new System.EventHandler(this.OnViewBolosClick);
            // 
            // boloLabel
            // 
            this.boloLabel.AutoSize = true;
            this.boloLabel.Depth = 0;
            this.boloLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.boloLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.boloLabel.Location = new System.Drawing.Point(12, 297);
            this.boloLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.boloLabel.Name = "boloLabel";
            this.boloLabel.Size = new System.Drawing.Size(106, 19);
            this.boloLabel.TabIndex = 11;
            this.boloLabel.Text = "BOLO Options:";
            // 
            // materialDivider2
            // 
            this.materialDivider2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider2.Depth = 0;
            this.materialDivider2.Location = new System.Drawing.Point(173, 297);
            this.materialDivider2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider2.Name = "materialDivider2";
            this.materialDivider2.Size = new System.Drawing.Size(17, 73);
            this.materialDivider2.TabIndex = 12;
            this.materialDivider2.Text = "materialDivider2";
            // 
            // cadLabel
            // 
            this.cadLabel.AutoSize = true;
            this.cadLabel.Depth = 0;
            this.cadLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.cadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cadLabel.Location = new System.Drawing.Point(216, 297);
            this.cadLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.cadLabel.Name = "cadLabel";
            this.cadLabel.Size = new System.Drawing.Size(99, 19);
            this.cadLabel.TabIndex = 14;
            this.cadLabel.Text = "CAD Options:";
            // 
            // viewOfficers
            // 
            this.viewOfficers.AutoSize = true;
            this.viewOfficers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewOfficers.Depth = 0;
            this.viewOfficers.Icon = null;
            this.viewOfficers.Location = new System.Drawing.Point(220, 334);
            this.viewOfficers.MouseState = MaterialSkin.MouseState.HOVER;
            this.viewOfficers.Name = "viewOfficers";
            this.viewOfficers.Primary = true;
            this.viewOfficers.Size = new System.Drawing.Size(119, 36);
            this.viewOfficers.TabIndex = 13;
            this.viewOfficers.Text = "View Officers";
            this.viewOfficers.UseVisualStyleBackColor = true;
            this.viewOfficers.Click += new System.EventHandler(this.OnViewOfficersClick);
            // 
            // btnViewAssignments
            // 
            this.btnViewAssignments.AutoSize = true;
            this.btnViewAssignments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewAssignments.Depth = 0;
            this.btnViewAssignments.Icon = null;
            this.btnViewAssignments.Location = new System.Drawing.Point(345, 334);
            this.btnViewAssignments.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewAssignments.Name = "btnViewAssignments";
            this.btnViewAssignments.Primary = true;
            this.btnViewAssignments.Size = new System.Drawing.Size(152, 36);
            this.btnViewAssignments.TabIndex = 15;
            this.btnViewAssignments.Text = "View Assignments";
            this.btnViewAssignments.UseVisualStyleBackColor = true;
            this.btnViewAssignments.Click += new System.EventHandler(this.OnViewAssignmentsClick);
            // 
            // DispatchMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(505, 382);
            this.Controls.Add(this.btnViewAssignments);
            this.Controls.Add(this.cadLabel);
            this.Controls.Add(this.viewOfficers);
            this.Controls.Add(this.materialDivider2);
            this.Controls.Add(this.boloLabel);
            this.Controls.Add(this.btnViewBolos);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.btnViewCivVeh);
            this.Controls.Add(this.plate);
            this.Controls.Add(this.civVehLabel);
            this.Controls.Add(this.btnViewCiv);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.firstName);
            this.Controls.Add(this.civLabel);
            this.MaximizeBox = false;
            this.Name = "DispatchMain";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dispatcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel civLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField firstName;
        private MaterialSkin.Controls.MaterialSingleLineTextField lastName;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewCiv;
        private MaterialSkin.Controls.MaterialLabel civVehLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField plate;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewCivVeh;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewBolos;
        private MaterialSkin.Controls.MaterialLabel boloLabel;
        private MaterialSkin.Controls.MaterialDivider materialDivider2;
        private MaterialSkin.Controls.MaterialLabel cadLabel;
        private MaterialSkin.Controls.MaterialRaisedButton viewOfficers;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewAssignments;
    }
}