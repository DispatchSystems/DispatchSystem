namespace Client
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
            this.btnRemoveBolo = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnAddBolo = new MaterialSkin.Controls.MaterialRaisedButton();
            this.boloLabel = new MaterialSkin.Controls.MaterialLabel();
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
            this.btnViewBolos.Location = new System.Drawing.Point(16, 330);
            this.btnViewBolos.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewBolos.Name = "btnViewBolos";
            this.btnViewBolos.Primary = true;
            this.btnViewBolos.Size = new System.Drawing.Size(100, 36);
            this.btnViewBolos.TabIndex = 8;
            this.btnViewBolos.Text = "View Bolos";
            this.btnViewBolos.UseVisualStyleBackColor = true;
            this.btnViewBolos.Click += new System.EventHandler(this.OnViewBolosClick);
            // 
            // btnRemoveBolo
            // 
            this.btnRemoveBolo.AutoSize = true;
            this.btnRemoveBolo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRemoveBolo.Depth = 0;
            this.btnRemoveBolo.Icon = null;
            this.btnRemoveBolo.Location = new System.Drawing.Point(206, 330);
            this.btnRemoveBolo.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRemoveBolo.Name = "btnRemoveBolo";
            this.btnRemoveBolo.Primary = true;
            this.btnRemoveBolo.Size = new System.Drawing.Size(114, 36);
            this.btnRemoveBolo.TabIndex = 9;
            this.btnRemoveBolo.Text = "Remove BOLO";
            this.btnRemoveBolo.UseVisualStyleBackColor = true;
            this.btnRemoveBolo.Click += new System.EventHandler(this.OnRemoveBoloClick);
            // 
            // btnAddBolo
            // 
            this.btnAddBolo.AutoSize = true;
            this.btnAddBolo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddBolo.Depth = 0;
            this.btnAddBolo.Icon = null;
            this.btnAddBolo.Location = new System.Drawing.Point(399, 330);
            this.btnAddBolo.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddBolo.Name = "btnAddBolo";
            this.btnAddBolo.Primary = true;
            this.btnAddBolo.Size = new System.Drawing.Size(86, 36);
            this.btnAddBolo.TabIndex = 10;
            this.btnAddBolo.Text = "Add BOLO";
            this.btnAddBolo.UseVisualStyleBackColor = true;
            this.btnAddBolo.Click += new System.EventHandler(this.OnAddBoloClick);
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
            // DispatchMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(505, 382);
            this.Controls.Add(this.boloLabel);
            this.Controls.Add(this.btnAddBolo);
            this.Controls.Add(this.btnRemoveBolo);
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
        private MaterialSkin.Controls.MaterialRaisedButton btnRemoveBolo;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddBolo;
        private MaterialSkin.Controls.MaterialLabel boloLabel;
    }
}