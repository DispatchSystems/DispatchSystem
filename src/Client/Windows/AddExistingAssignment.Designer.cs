namespace DispatchSystem.Client.Windows
{
    partial class AddExistingAssignment
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
            this.assignmentsView = new MaterialSkin.Controls.MaterialListView();
            this.creation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // assignmentsView
            // 
            this.assignmentsView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.assignmentsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.creation,
            this.summary});
            this.assignmentsView.Depth = 0;
            this.assignmentsView.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.assignmentsView.FullRowSelect = true;
            this.assignmentsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.assignmentsView.Location = new System.Drawing.Point(13, 77);
            this.assignmentsView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.assignmentsView.MouseState = MaterialSkin.MouseState.OUT;
            this.assignmentsView.MultiSelect = false;
            this.assignmentsView.Name = "assignmentsView";
            this.assignmentsView.OwnerDraw = true;
            this.assignmentsView.Size = new System.Drawing.Size(760, 443);
            this.assignmentsView.TabIndex = 0;
            this.assignmentsView.UseCompatibleStateImageBehavior = false;
            this.assignmentsView.View = System.Windows.Forms.View.Details;
            this.assignmentsView.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // creation
            // 
            this.creation.Text = "Creation";
            this.creation.Width = 115;
            // 
            // summary
            // 
            this.summary.Text = "Summary";
            this.summary.Width = 1453;
            // 
            // AddExistingAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(785, 532);
            this.Controls.Add(this.assignmentsView);
            this.MaximizeBox = false;
            this.Name = "AddExistingAssignment";
            this.Sizable = false;
            this.Text = "Add To Existing";
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView assignmentsView;
        private System.Windows.Forms.ColumnHeader creation;
        private System.Windows.Forms.ColumnHeader summary;
    }
}