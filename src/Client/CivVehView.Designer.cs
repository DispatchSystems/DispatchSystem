namespace Client
{
    partial class CivVehView
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
            this.nameLabel = new MaterialSkin.Controls.MaterialLabel();
            this.firstNameView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lastNameView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.infoLabel = new MaterialSkin.Controls.MaterialLabel();
            this.stolenView = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.plateView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.plateLabel = new MaterialSkin.Controls.MaterialLabel();
            this.registrationView = new MaterialSkin.Controls.MaterialCheckBox();
            this.insuranceView = new MaterialSkin.Controls.MaterialCheckBox();
            this.btnResync = new MaterialSkin.Controls.MaterialRaisedButton();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Depth = 0;
            this.nameLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nameLabel.Location = new System.Drawing.Point(8, 150);
            this.nameLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(53, 19);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name:";
            // 
            // firstNameView
            // 
            this.firstNameView.Depth = 0;
            this.firstNameView.Hint = "First Name";
            this.firstNameView.Location = new System.Drawing.Point(12, 185);
            this.firstNameView.MaxLength = 32767;
            this.firstNameView.MouseState = MaterialSkin.MouseState.HOVER;
            this.firstNameView.Name = "firstNameView";
            this.firstNameView.PasswordChar = '\0';
            this.firstNameView.ReadOnly = true;
            this.firstNameView.SelectedText = "";
            this.firstNameView.SelectionLength = 0;
            this.firstNameView.SelectionStart = 0;
            this.firstNameView.Size = new System.Drawing.Size(143, 23);
            this.firstNameView.TabIndex = 1;
            this.firstNameView.TabStop = false;
            this.firstNameView.UseSystemPasswordChar = false;
            // 
            // lastNameView
            // 
            this.lastNameView.Depth = 0;
            this.lastNameView.Hint = "Last Name";
            this.lastNameView.Location = new System.Drawing.Point(12, 231);
            this.lastNameView.MaxLength = 32767;
            this.lastNameView.MouseState = MaterialSkin.MouseState.HOVER;
            this.lastNameView.Name = "lastNameView";
            this.lastNameView.PasswordChar = '\0';
            this.lastNameView.ReadOnly = true;
            this.lastNameView.SelectedText = "";
            this.lastNameView.SelectionLength = 0;
            this.lastNameView.SelectionStart = 0;
            this.lastNameView.Size = new System.Drawing.Size(143, 23);
            this.lastNameView.TabIndex = 2;
            this.lastNameView.TabStop = false;
            this.lastNameView.UseSystemPasswordChar = false;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Depth = 0;
            this.infoLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.infoLabel.Location = new System.Drawing.Point(228, 76);
            this.infoLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(91, 19);
            this.infoLabel.TabIndex = 3;
            this.infoLabel.Text = "Information:";
            // 
            // stolenView
            // 
            this.stolenView.AutoCheck = false;
            this.stolenView.AutoSize = true;
            this.stolenView.Depth = 0;
            this.stolenView.Font = new System.Drawing.Font("Roboto", 10F);
            this.stolenView.Location = new System.Drawing.Point(232, 111);
            this.stolenView.Margin = new System.Windows.Forms.Padding(0);
            this.stolenView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.stolenView.MouseState = MaterialSkin.MouseState.HOVER;
            this.stolenView.Name = "stolenView";
            this.stolenView.Ripple = true;
            this.stolenView.Size = new System.Drawing.Size(69, 30);
            this.stolenView.TabIndex = 4;
            this.stolenView.Text = "Stolen";
            this.stolenView.UseVisualStyleBackColor = true;
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(186, 75);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(15, 238);
            this.materialDivider1.TabIndex = 5;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // plateView
            // 
            this.plateView.Depth = 0;
            this.plateView.Hint = "Plate";
            this.plateView.Location = new System.Drawing.Point(12, 111);
            this.plateView.MaxLength = 32767;
            this.plateView.MouseState = MaterialSkin.MouseState.HOVER;
            this.plateView.Name = "plateView";
            this.plateView.PasswordChar = '\0';
            this.plateView.ReadOnly = true;
            this.plateView.SelectedText = "";
            this.plateView.SelectionLength = 0;
            this.plateView.SelectionStart = 0;
            this.plateView.Size = new System.Drawing.Size(143, 23);
            this.plateView.TabIndex = 7;
            this.plateView.TabStop = false;
            this.plateView.UseSystemPasswordChar = false;
            // 
            // plateLabel
            // 
            this.plateLabel.AutoSize = true;
            this.plateLabel.Depth = 0;
            this.plateLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.plateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.plateLabel.Location = new System.Drawing.Point(8, 76);
            this.plateLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.plateLabel.Name = "plateLabel";
            this.plateLabel.Size = new System.Drawing.Size(47, 19);
            this.plateLabel.TabIndex = 6;
            this.plateLabel.Text = "Plate:";
            // 
            // registrationView
            // 
            this.registrationView.AutoCheck = false;
            this.registrationView.AutoSize = true;
            this.registrationView.Depth = 0;
            this.registrationView.Font = new System.Drawing.Font("Roboto", 10F);
            this.registrationView.Location = new System.Drawing.Point(232, 167);
            this.registrationView.Margin = new System.Windows.Forms.Padding(0);
            this.registrationView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.registrationView.MouseState = MaterialSkin.MouseState.HOVER;
            this.registrationView.Name = "registrationView";
            this.registrationView.Ripple = true;
            this.registrationView.Size = new System.Drawing.Size(105, 30);
            this.registrationView.TabIndex = 8;
            this.registrationView.Text = "Registration";
            this.registrationView.UseVisualStyleBackColor = true;
            // 
            // insuranceView
            // 
            this.insuranceView.AutoCheck = false;
            this.insuranceView.AutoSize = true;
            this.insuranceView.Depth = 0;
            this.insuranceView.Font = new System.Drawing.Font("Roboto", 10F);
            this.insuranceView.Location = new System.Drawing.Point(232, 224);
            this.insuranceView.Margin = new System.Windows.Forms.Padding(0);
            this.insuranceView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.insuranceView.MouseState = MaterialSkin.MouseState.HOVER;
            this.insuranceView.Name = "insuranceView";
            this.insuranceView.Ripple = true;
            this.insuranceView.Size = new System.Drawing.Size(91, 30);
            this.insuranceView.TabIndex = 9;
            this.insuranceView.Text = "Insurance";
            this.insuranceView.UseVisualStyleBackColor = true;
            // 
            // btnResync
            // 
            this.btnResync.AutoSize = true;
            this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnResync.Depth = 0;
            this.btnResync.Icon = null;
            this.btnResync.Location = new System.Drawing.Point(12, 278);
            this.btnResync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnResync.Name = "btnResync";
            this.btnResync.Primary = true;
            this.btnResync.Size = new System.Drawing.Size(73, 36);
            this.btnResync.TabIndex = 10;
            this.btnResync.Text = "Resync";
            this.btnResync.UseVisualStyleBackColor = true;
            this.btnResync.Click += new System.EventHandler(this.OnResyncClick);
            // 
            // CivVehView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(378, 325);
            this.Controls.Add(this.btnResync);
            this.Controls.Add(this.insuranceView);
            this.Controls.Add(this.registrationView);
            this.Controls.Add(this.plateView);
            this.Controls.Add(this.plateLabel);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.stolenView);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.lastNameView);
            this.Controls.Add(this.firstNameView);
            this.Controls.Add(this.nameLabel);
            this.MaximizeBox = false;
            this.Name = "CivVehView";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Civilian Vehicle View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel nameLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField firstNameView;
        private MaterialSkin.Controls.MaterialSingleLineTextField lastNameView;
        private MaterialSkin.Controls.MaterialLabel infoLabel;
        private MaterialSkin.Controls.MaterialCheckBox stolenView;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialSingleLineTextField plateView;
        private MaterialSkin.Controls.MaterialLabel plateLabel;
        private MaterialSkin.Controls.MaterialCheckBox registrationView;
        private MaterialSkin.Controls.MaterialCheckBox insuranceView;
        private MaterialSkin.Controls.MaterialRaisedButton btnResync;
    }
}