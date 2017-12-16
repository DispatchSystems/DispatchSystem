namespace DumpUnloader.Windows
{
    partial class PermissionsDialogue
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.civs = new System.Windows.Forms.ListView();
            this.leo = new System.Windows.Forms.ListView();
            this.dispatch = new System.Windows.Forms.ListView();
            this.civItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.leoItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dispatchItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Civilian Perms:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "LEO Perms:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(504, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dispatch Perms:";
            // 
            // civs
            // 
            this.civs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.civItems});
            this.civs.FullRowSelect = true;
            this.civs.GridLines = true;
            this.civs.Location = new System.Drawing.Point(13, 30);
            this.civs.Name = "civs";
            this.civs.Size = new System.Drawing.Size(241, 531);
            this.civs.TabIndex = 6;
            this.civs.UseCompatibleStateImageBehavior = false;
            this.civs.View = System.Windows.Forms.View.Details;
            // 
            // leo
            // 
            this.leo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.leoItems});
            this.leo.FullRowSelect = true;
            this.leo.GridLines = true;
            this.leo.Location = new System.Drawing.Point(260, 30);
            this.leo.Name = "leo";
            this.leo.Size = new System.Drawing.Size(241, 531);
            this.leo.TabIndex = 7;
            this.leo.UseCompatibleStateImageBehavior = false;
            this.leo.View = System.Windows.Forms.View.Details;
            // 
            // dispatch
            // 
            this.dispatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dispatchItems});
            this.dispatch.FullRowSelect = true;
            this.dispatch.GridLines = true;
            this.dispatch.Location = new System.Drawing.Point(507, 30);
            this.dispatch.Name = "dispatch";
            this.dispatch.Size = new System.Drawing.Size(241, 531);
            this.dispatch.TabIndex = 8;
            this.dispatch.UseCompatibleStateImageBehavior = false;
            this.dispatch.View = System.Windows.Forms.View.Details;
            // 
            // civItems
            // 
            this.civItems.Text = "Items";
            this.civItems.Width = 241;
            // 
            // leoItems
            // 
            this.leoItems.Text = "Items";
            this.leoItems.Width = 241;
            // 
            // dispatchItems
            // 
            this.dispatchItems.Text = "Items";
            this.dispatchItems.Width = 241;
            // 
            // PermissionsDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 573);
            this.Controls.Add(this.dispatch);
            this.Controls.Add(this.leo);
            this.Controls.Add(this.civs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(775, 612);
            this.MinimumSize = new System.Drawing.Size(775, 612);
            this.Name = "PermissionsDialogue";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Permissions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView civs;
        private System.Windows.Forms.ListView leo;
        private System.Windows.Forms.ListView dispatch;
        private System.Windows.Forms.ColumnHeader civItems;
        private System.Windows.Forms.ColumnHeader leoItems;
        private System.Windows.Forms.ColumnHeader dispatchItems;
    }
}