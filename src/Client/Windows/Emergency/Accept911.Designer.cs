namespace DispatchSystem.Client.Windows.Emergency
{
    partial class Accept911
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
            this.information = new MaterialSkin.Controls.MaterialLabel();
            this.btnAccept = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            this.SuspendLayout();
            // 
            // information
            // 
            this.information.AutoSize = true;
            this.information.Depth = 0;
            this.information.Font = new System.Drawing.Font("Roboto", 11F);
            this.information.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.information.Location = new System.Drawing.Point(13, 74);
            this.information.MouseState = MaterialSkin.MouseState.HOVER;
            this.information.Name = "information";
            this.information.Size = new System.Drawing.Size(87, 19);
            this.information.TabIndex = 0;
            this.information.Text = "default_text";
            // 
            // btnAccept
            // 
            this.btnAccept.AutoSize = true;
            this.btnAccept.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAccept.Depth = 0;
            this.btnAccept.Icon = null;
            this.btnAccept.Location = new System.Drawing.Point(363, 233);
            this.btnAccept.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Primary = true;
            this.btnAccept.Size = new System.Drawing.Size(73, 36);
            this.btnAccept.TabIndex = 1;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.OnAcceptClick);
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSize = true;
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.Icon = null;
            this.materialFlatButton1.Location = new System.Drawing.Point(443, 233);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(56, 36);
            this.materialFlatButton1.TabIndex = 2;
            this.materialFlatButton1.Text = "Deny";
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Click += new System.EventHandler(this.OnDenyClick);
            // 
            // Accept911
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(512, 284);
            this.Controls.Add(this.materialFlatButton1);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.information);
            this.MaximizeBox = false;
            this.Name = "Accept911";
            this.Sizable = false;
            this.Text = "Incoming 911 Call...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel information;
        private MaterialSkin.Controls.MaterialRaisedButton btnAccept;
        private MaterialSkin.Controls.MaterialFlatButton materialFlatButton1;
    }
}