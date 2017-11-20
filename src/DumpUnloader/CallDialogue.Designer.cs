namespace DumpUnloader
{
    partial class CallDialogue
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
            this.callId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.callIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.callPlayer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.callAccepted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.callDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.callId,
            this.callIp,
            this.callDate,
            this.callPlayer,
            this.callAccepted});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(796, 543);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // callId
            // 
            this.callId.Text = "Id";
            this.callId.Width = 220;
            // 
            // callIp
            // 
            this.callIp.Text = "Source IP";
            this.callIp.Width = 120;
            // 
            // callPlayer
            // 
            this.callPlayer.Text = "Player";
            this.callPlayer.Width = 120;
            // 
            // callAccepted
            // 
            this.callAccepted.Text = "Accepted";
            this.callAccepted.Width = 80;
            // 
            // callDate
            // 
            this.callDate.Text = "Creation Date";
            this.callDate.Width = 120;
            // 
            // CallDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 568);
            this.Controls.Add(this.listView1);
            this.Name = "CallDialogue";
            this.Text = "Calls";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader callId;
        private System.Windows.Forms.ColumnHeader callIp;
        private System.Windows.Forms.ColumnHeader callPlayer;
        private System.Windows.Forms.ColumnHeader callAccepted;
        private System.Windows.Forms.ColumnHeader callDate;
    }
}