namespace DispatchSystem.Client.Windows
{
    partial class AssignmentsView
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
            this.theAssignments = new MaterialSkin.Controls.MaterialListView();
            this.creation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemSummary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResync = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnAddAssignment = new MaterialSkin.Controls.MaterialRaisedButton();
            this.rightClickMenu = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // theAssignments
            // 
            this.theAssignments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.theAssignments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.creation,
            this.itemSummary});
            this.theAssignments.Depth = 0;
            this.theAssignments.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.theAssignments.FullRowSelect = true;
            this.theAssignments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.theAssignments.Location = new System.Drawing.Point(13, 73);
            this.theAssignments.MouseLocation = new System.Drawing.Point(-1, -1);
            this.theAssignments.MouseState = MaterialSkin.MouseState.OUT;
            this.theAssignments.Name = "theAssignments";
            this.theAssignments.OwnerDraw = true;
            this.theAssignments.Size = new System.Drawing.Size(776, 408);
            this.theAssignments.TabIndex = 0;
            this.theAssignments.UseCompatibleStateImageBehavior = false;
            this.theAssignments.View = System.Windows.Forms.View.Details;
            this.theAssignments.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnAssignmentsClick);
            // 
            // creation
            // 
            this.creation.Text = "Creation Date";
            this.creation.Width = 163;
            // 
            // itemSummary
            // 
            this.itemSummary.Text = "Summary";
            this.itemSummary.Width = 867;
            // 
            // btnResync
            // 
            this.btnResync.AutoSize = true;
            this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnResync.Depth = 0;
            this.btnResync.Icon = null;
            this.btnResync.Location = new System.Drawing.Point(716, 490);
            this.btnResync.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnResync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnResync.Name = "btnResync";
            this.btnResync.Primary = false;
            this.btnResync.Size = new System.Drawing.Size(73, 36);
            this.btnResync.TabIndex = 1;
            this.btnResync.Text = "Resync";
            this.btnResync.UseVisualStyleBackColor = true;
            this.btnResync.Click += new System.EventHandler(this.OnResyncClick);
            // 
            // btnAddAssignment
            // 
            this.btnAddAssignment.AutoSize = true;
            this.btnAddAssignment.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddAssignment.Depth = 0;
            this.btnAddAssignment.Icon = null;
            this.btnAddAssignment.Location = new System.Drawing.Point(13, 490);
            this.btnAddAssignment.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddAssignment.Name = "btnAddAssignment";
            this.btnAddAssignment.Primary = true;
            this.btnAddAssignment.Size = new System.Drawing.Size(138, 36);
            this.btnAddAssignment.TabIndex = 2;
            this.btnAddAssignment.Text = "Add Assignment";
            this.btnAddAssignment.UseVisualStyleBackColor = true;
            this.btnAddAssignment.Click += new System.EventHandler(this.OnAddAssignmentClick);
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rightClickMenu.Depth = 0;
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.rightClickMenu.MouseState = MaterialSkin.MouseState.HOVER;
            this.rightClickMenu.Name = "rightClickMenu";
            this.rightClickMenu.Size = new System.Drawing.Size(153, 48);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnRightClickRemove);
            // 
            // AssignmentsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(801, 538);
            this.Controls.Add(this.btnAddAssignment);
            this.Controls.Add(this.btnResync);
            this.Controls.Add(this.theAssignments);
            this.MaximizeBox = false;
            this.Name = "AssignmentsView";
            this.Sizable = false;
            this.Text = "Assignments";
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView theAssignments;
        private MaterialSkin.Controls.MaterialFlatButton btnResync;
        private System.Windows.Forms.ColumnHeader creation;
        private System.Windows.Forms.ColumnHeader itemSummary;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddAssignment;
        private MaterialSkin.Controls.MaterialContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}