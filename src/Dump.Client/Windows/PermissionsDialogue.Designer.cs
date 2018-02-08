namespace DispatchSystem.Dump.Client.Windows
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
            this.label3 = new System.Windows.Forms.Label();
            this.dispatch = new System.Windows.Forms.ListView();
            this.dispatchItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Dispatch Perms:";
            // 
            // dispatch
            // 
            this.dispatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dispatchItems});
            this.dispatch.FullRowSelect = true;
            this.dispatch.GridLines = true;
            this.dispatch.Location = new System.Drawing.Point(12, 30);
            this.dispatch.Name = "dispatch";
            this.dispatch.Size = new System.Drawing.Size(736, 531);
            this.dispatch.TabIndex = 8;
            this.dispatch.UseCompatibleStateImageBehavior = false;
            this.dispatch.View = System.Windows.Forms.View.Details;
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
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView dispatch;
        private System.Windows.Forms.ColumnHeader dispatchItems;
    }
}