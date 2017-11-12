namespace DispatchSystem.cl.Windows
{
    partial class MultiOfficerView
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
            this.btnResync = new MaterialSkin.Controls.MaterialFlatButton();
            this.officers = new MaterialSkin.Controls.MaterialListView();
            this.ofcCallsign = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rightClickMenu = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.viewStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setStatusStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusOnDutyStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusOffDutyStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBusyStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnResync
            // 
            this.btnResync.AutoSize = true;
            this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnResync.Depth = 0;
            this.btnResync.Icon = null;
            this.btnResync.Location = new System.Drawing.Point(688, 473);
            this.btnResync.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnResync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnResync.Name = "btnResync";
            this.btnResync.Primary = false;
            this.btnResync.Size = new System.Drawing.Size(73, 36);
            this.btnResync.TabIndex = 0;
            this.btnResync.Text = "resync";
            this.btnResync.UseVisualStyleBackColor = true;
            this.btnResync.Click += new System.EventHandler(this.OnResyncClick);
            // 
            // officers
            // 
            this.officers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.officers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ofcCallsign,
            this.ofcStatus});
            this.officers.Depth = 0;
            this.officers.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.officers.FullRowSelect = true;
            this.officers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.officers.Location = new System.Drawing.Point(13, 74);
            this.officers.MouseLocation = new System.Drawing.Point(-1, -1);
            this.officers.MouseState = MaterialSkin.MouseState.OUT;
            this.officers.MultiSelect = false;
            this.officers.Name = "officers";
            this.officers.OwnerDraw = true;
            this.officers.Size = new System.Drawing.Size(748, 390);
            this.officers.TabIndex = 1;
            this.officers.UseCompatibleStateImageBehavior = false;
            this.officers.View = System.Windows.Forms.View.Details;
            this.officers.DoubleClick += new System.EventHandler(this.ViewOfficer);
            this.officers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            // 
            // ofcCallsign
            // 
            this.ofcCallsign.Text = "Callsign";
            this.ofcCallsign.Width = 172;
            // 
            // ofcStatus
            // 
            this.ofcStatus.Text = "Status";
            this.ofcStatus.Width = 574;
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rightClickMenu.Depth = 0;
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewStripItem,
            this.setStatusStripItem,
            this.removeToolStripMenuItem});
            this.rightClickMenu.MouseState = MaterialSkin.MouseState.HOVER;
            this.rightClickMenu.Name = "rightClickMenu";
            this.rightClickMenu.Size = new System.Drawing.Size(126, 70);
            // 
            // viewStripItem
            // 
            this.viewStripItem.Name = "viewStripItem";
            this.viewStripItem.Size = new System.Drawing.Size(125, 22);
            this.viewStripItem.Text = "View";
            this.viewStripItem.Click += new System.EventHandler(this.ViewOfficer);
            // 
            // setStatusStripItem
            // 
            this.setStatusStripItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusOnDutyStripItem,
            this.statusOffDutyStripItem,
            this.statusBusyStripItem});
            this.setStatusStripItem.Name = "setStatusStripItem";
            this.setStatusStripItem.Size = new System.Drawing.Size(125, 22);
            this.setStatusStripItem.Text = "Set Status";
            // 
            // statusOnDutyStripItem
            // 
            this.statusOnDutyStripItem.Name = "statusOnDutyStripItem";
            this.statusOnDutyStripItem.Size = new System.Drawing.Size(119, 22);
            this.statusOnDutyStripItem.Text = "On Duty";
            this.statusOnDutyStripItem.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // statusOffDutyStripItem
            // 
            this.statusOffDutyStripItem.Name = "statusOffDutyStripItem";
            this.statusOffDutyStripItem.Size = new System.Drawing.Size(119, 22);
            this.statusOffDutyStripItem.Text = "Off Duty";
            this.statusOffDutyStripItem.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // statusBusyStripItem
            // 
            this.statusBusyStripItem.Name = "statusBusyStripItem";
            this.statusBusyStripItem.Size = new System.Drawing.Size(119, 22);
            this.statusBusyStripItem.Text = "Busy";
            this.statusBusyStripItem.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveOfficerClick);
            // 
            // MultiOfficerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(774, 524);
            this.Controls.Add(this.officers);
            this.Controls.Add(this.btnResync);
            this.MaximizeBox = false;
            this.Name = "MultiOfficerView";
            this.Sizable = false;
            this.Text = "View Officers";
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialFlatButton btnResync;
        private MaterialSkin.Controls.MaterialListView officers;
        private System.Windows.Forms.ColumnHeader ofcCallsign;
        private System.Windows.Forms.ColumnHeader ofcStatus;
        private MaterialSkin.Controls.MaterialContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem viewStripItem;
        private System.Windows.Forms.ToolStripMenuItem setStatusStripItem;
        private System.Windows.Forms.ToolStripMenuItem statusOnDutyStripItem;
        private System.Windows.Forms.ToolStripMenuItem statusOffDutyStripItem;
        private System.Windows.Forms.ToolStripMenuItem statusBusyStripItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}