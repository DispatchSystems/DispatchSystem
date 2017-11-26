namespace DumpUnloader
{
    partial class OfficerDialogue
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.ofcId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcCreation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcCallsign = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ofcId,
            this.ofcIp,
            this.ofcCreation,
            this.ofcCallsign,
            this.ofcStatus});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(765, 490);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ofcId
            // 
            this.ofcId.Text = "Id";
            this.ofcId.Width = 220;
            // 
            // ofcIp
            // 
            this.ofcIp.Text = "Source IP";
            this.ofcIp.Width = 120;
            // 
            // ofcCreation
            // 
            this.ofcCreation.Text = "Creation Date";
            this.ofcCreation.Width = 120;
            // 
            // ofcCallsign
            // 
            this.ofcCallsign.Text = "Callsign";
            this.ofcCallsign.Width = 90;
            // 
            // ofcStatus
            // 
            this.ofcStatus.Text = "Status";
            this.ofcStatus.Width = 120;
            // 
            // OfficerDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 515);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(806, 554);
            this.MinimumSize = new System.Drawing.Size(806, 554);
            this.Name = "OfficerDialogue";
            this.Text = "Officers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ofcId;
        private System.Windows.Forms.ColumnHeader ofcIp;
        private System.Windows.Forms.ColumnHeader ofcCreation;
        private System.Windows.Forms.ColumnHeader ofcCallsign;
        private System.Windows.Forms.ColumnHeader ofcStatus;
    }
}