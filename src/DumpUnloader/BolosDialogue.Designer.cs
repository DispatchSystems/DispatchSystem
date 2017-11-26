namespace DumpUnloader
{
    partial class BolosDialogue
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
            this.boloCreator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.boloDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.boloId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.boloCreation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.boloId,
            this.boloCreation,
            this.boloCreator,
            this.boloDesc});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(683, 539);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // boloCreator
            // 
            this.boloCreator.Text = "Creator";
            this.boloCreator.Width = 160;
            // 
            // boloDesc
            // 
            this.boloDesc.Text = "Description";
            this.boloDesc.Width = 888;
            // 
            // boloId
            // 
            this.boloId.Text = "Id";
            this.boloId.Width = 220;
            // 
            // boloCreation
            // 
            this.boloCreation.Text = "Creation Date";
            this.boloCreation.Width = 120;
            // 
            // BolosDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 564);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(724, 603);
            this.MinimumSize = new System.Drawing.Size(724, 603);
            this.Name = "BolosDialogue";
            this.Text = "BOLOs";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader boloCreator;
        private System.Windows.Forms.ColumnHeader boloDesc;
        private System.Windows.Forms.ColumnHeader boloId;
        private System.Windows.Forms.ColumnHeader boloCreation;
    }
}