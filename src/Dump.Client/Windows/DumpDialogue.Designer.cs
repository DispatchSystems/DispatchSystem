namespace DispatchSystem.Dump.Client.Windows
{
    partial class DumpDialogue
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
            this.civs = new System.Windows.Forms.Button();
            this.vehs = new System.Windows.Forms.Button();
            this.ofcs = new System.Windows.Forms.Button();
            this.bolos = new System.Windows.Forms.Button();
            this.calls = new System.Windows.Forms.Button();
            this.perms = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // civs
            // 
            this.civs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.civs.Location = new System.Drawing.Point(6, 15);
            this.civs.Name = "civs";
            this.civs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.civs.Size = new System.Drawing.Size(135, 47);
            this.civs.TabIndex = 1;
            this.civs.Text = "Civilians";
            this.civs.UseVisualStyleBackColor = true;
            this.civs.Click += new System.EventHandler(this.CiviliansClick);
            // 
            // vehs
            // 
            this.vehs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vehs.Location = new System.Drawing.Point(6, 68);
            this.vehs.Name = "vehs";
            this.vehs.Size = new System.Drawing.Size(133, 47);
            this.vehs.TabIndex = 2;
            this.vehs.Text = "Vehicles";
            this.vehs.UseVisualStyleBackColor = true;
            this.vehs.Click += new System.EventHandler(this.VehiclesClick);
            // 
            // ofcs
            // 
            this.ofcs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ofcs.Location = new System.Drawing.Point(6, 121);
            this.ofcs.Name = "ofcs";
            this.ofcs.Size = new System.Drawing.Size(135, 47);
            this.ofcs.TabIndex = 3;
            this.ofcs.Text = "Officers";
            this.ofcs.UseVisualStyleBackColor = true;
            this.ofcs.Click += new System.EventHandler(this.OfficersClick);
            // 
            // bolos
            // 
            this.bolos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bolos.Location = new System.Drawing.Point(6, 174);
            this.bolos.Name = "bolos";
            this.bolos.Size = new System.Drawing.Size(136, 47);
            this.bolos.TabIndex = 4;
            this.bolos.Text = "BOLOs";
            this.bolos.UseVisualStyleBackColor = true;
            this.bolos.Click += new System.EventHandler(this.BolosClick);
            // 
            // calls
            // 
            this.calls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calls.Location = new System.Drawing.Point(6, 227);
            this.calls.Name = "calls";
            this.calls.Size = new System.Drawing.Size(135, 47);
            this.calls.TabIndex = 5;
            this.calls.Text = "911 Calls";
            this.calls.UseVisualStyleBackColor = true;
            this.calls.Click += new System.EventHandler(this.CallsClick);
            // 
            // perms
            // 
            this.perms.Location = new System.Drawing.Point(6, 19);
            this.perms.Name = "perms";
            this.perms.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.perms.Size = new System.Drawing.Size(135, 47);
            this.perms.TabIndex = 6;
            this.perms.Text = "Permissions";
            this.perms.UseVisualStyleBackColor = true;
            this.perms.Click += new System.EventHandler(this.PermissionsClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.perms);
            this.groupBox1.Location = new System.Drawing.Point(163, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 295);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other Options";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.civs);
            this.groupBox2.Controls.Add(this.vehs);
            this.groupBox2.Controls.Add(this.calls);
            this.groupBox2.Controls.Add(this.ofcs);
            this.groupBox2.Controls.Add(this.bolos);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(145, 295);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "List Options";
            // 
            // DumpDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 319);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(336, 358);
            this.Name = "DumpDialogue";
            this.Text = "Dump Unloader";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button civs;
        private System.Windows.Forms.Button vehs;
        private System.Windows.Forms.Button ofcs;
        private System.Windows.Forms.Button bolos;
        private System.Windows.Forms.Button calls;
        private System.Windows.Forms.Button perms;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}