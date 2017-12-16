namespace DumpUnloader.Windows
{
    partial class CivilianDialogue
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
            this.a = new System.Windows.Forms.ListView();
            this.civId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civFirst = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civLast = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civWarrant = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civCreation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.civTickets = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // a
            // 
            this.a.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.civId,
            this.civIp,
            this.civFirst,
            this.civLast,
            this.civWarrant,
            this.civCreation,
            this.civNotes,
            this.civTickets});
            this.a.FullRowSelect = true;
            this.a.GridLines = true;
            this.a.Location = new System.Drawing.Point(13, 13);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(785, 464);
            this.a.TabIndex = 0;
            this.a.UseCompatibleStateImageBehavior = false;
            this.a.View = System.Windows.Forms.View.Details;
            // 
            // civId
            // 
            this.civId.Text = "Id";
            this.civId.Width = 220;
            // 
            // civIp
            // 
            this.civIp.Text = "Source IP";
            this.civIp.Width = 120;
            // 
            // civFirst
            // 
            this.civFirst.Text = "First";
            this.civFirst.Width = 90;
            // 
            // civLast
            // 
            this.civLast.Text = "Last";
            this.civLast.Width = 90;
            // 
            // civWarrant
            // 
            this.civWarrant.Text = "Warrant";
            this.civWarrant.Width = 70;
            // 
            // civCreation
            // 
            this.civCreation.Text = "Creation";
            this.civCreation.Width = 120;
            // 
            // civNotes
            // 
            this.civNotes.Text = "Notes";
            this.civNotes.Width = 430;
            // 
            // civTickets
            // 
            this.civTickets.Text = "Tickets";
            this.civTickets.Width = 430;
            // 
            // CivilianDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 489);
            this.Controls.Add(this.a);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(826, 528);
            this.MinimumSize = new System.Drawing.Size(826, 528);
            this.Name = "CivilianDialogue";
            this.Text = "Civilians";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView a;
        private System.Windows.Forms.ColumnHeader civId;
        private System.Windows.Forms.ColumnHeader civIp;
        private System.Windows.Forms.ColumnHeader civFirst;
        private System.Windows.Forms.ColumnHeader civLast;
        private System.Windows.Forms.ColumnHeader civWarrant;
        private System.Windows.Forms.ColumnHeader civCreation;
        private System.Windows.Forms.ColumnHeader civNotes;
        private System.Windows.Forms.ColumnHeader civTickets;
    }
}