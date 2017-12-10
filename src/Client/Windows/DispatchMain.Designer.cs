namespace DispatchSystem.Client.Windows
{
    partial class DispatchMain
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
            this.civLabel = new MaterialSkin.Controls.MaterialLabel();
            this.firstName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lastName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btnViewCiv = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnViewCivVeh = new MaterialSkin.Controls.MaterialRaisedButton();
            this.assignments = new MaterialSkin.Controls.MaterialListView();
            this.creation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemSummary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.assignmentsRightClick = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.civvehLabel = new MaterialSkin.Controls.MaterialLabel();
            this.toggleDark = new MaterialSkin.Controls.MaterialCheckBox();
            this.plate = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.tabSelector = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabs = new MaterialSkin.Controls.MaterialTabControl();
            this.tab1 = new System.Windows.Forms.TabPage();
            this.btnAssignmentAdd = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tab2 = new System.Windows.Forms.TabPage();
            this.officers = new MaterialSkin.Controls.MaterialListView();
            this.ofcCallsign = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.officersRightClick = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.viewStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setStatusStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickOfficerOnDuty = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickOfficerOffDuty = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickOfficerBusy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tab3 = new System.Windows.Forms.TabPage();
            this.btnBoloAdd = new MaterialSkin.Controls.MaterialRaisedButton();
            this.bolos = new MaterialSkin.Controls.MaterialListView();
            this.author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bolo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bolosRightClick = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.btnRemoveSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.btnResync = new MaterialSkin.Controls.MaterialFlatButton();
            this.assignmentsRightClick.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.officersRightClick.SuspendLayout();
            this.tab3.SuspendLayout();
            this.bolosRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // civLabel
            // 
            this.civLabel.AutoSize = true;
            this.civLabel.Depth = 0;
            this.civLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.civLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.civLabel.Location = new System.Drawing.Point(12, 77);
            this.civLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.civLabel.Name = "civLabel";
            this.civLabel.Size = new System.Drawing.Size(62, 19);
            this.civLabel.TabIndex = 0;
            this.civLabel.Text = "Civilian:";
            // 
            // firstName
            // 
            this.firstName.Depth = 0;
            this.firstName.Hint = "First Name";
            this.firstName.Location = new System.Drawing.Point(16, 108);
            this.firstName.MaxLength = 32767;
            this.firstName.MouseState = MaterialSkin.MouseState.HOVER;
            this.firstName.Name = "firstName";
            this.firstName.PasswordChar = '\0';
            this.firstName.ReadOnly = false;
            this.firstName.SelectedText = "";
            this.firstName.SelectionLength = 0;
            this.firstName.SelectionStart = 0;
            this.firstName.Size = new System.Drawing.Size(129, 23);
            this.firstName.TabIndex = 1;
            this.firstName.TabStop = false;
            this.firstName.UseSystemPasswordChar = false;
            this.firstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnFirstNameKeyPress);
            // 
            // lastName
            // 
            this.lastName.Depth = 0;
            this.lastName.Hint = "Last Name";
            this.lastName.Location = new System.Drawing.Point(151, 108);
            this.lastName.MaxLength = 32767;
            this.lastName.MouseState = MaterialSkin.MouseState.HOVER;
            this.lastName.Name = "lastName";
            this.lastName.PasswordChar = '\0';
            this.lastName.ReadOnly = false;
            this.lastName.SelectedText = "";
            this.lastName.SelectionLength = 0;
            this.lastName.SelectionStart = 0;
            this.lastName.Size = new System.Drawing.Size(129, 23);
            this.lastName.TabIndex = 2;
            this.lastName.TabStop = false;
            this.lastName.UseSystemPasswordChar = false;
            this.lastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnLastNameKeyPress);
            // 
            // btnViewCiv
            // 
            this.btnViewCiv.AutoSize = true;
            this.btnViewCiv.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewCiv.Depth = 0;
            this.btnViewCiv.Icon = null;
            this.btnViewCiv.Location = new System.Drawing.Point(286, 108);
            this.btnViewCiv.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewCiv.Name = "btnViewCiv";
            this.btnViewCiv.Primary = true;
            this.btnViewCiv.Size = new System.Drawing.Size(113, 36);
            this.btnViewCiv.TabIndex = 3;
            this.btnViewCiv.Text = "View Civilian";
            this.btnViewCiv.UseVisualStyleBackColor = true;
            this.btnViewCiv.Click += new System.EventHandler(this.OnViewCivClick);
            // 
            // btnViewCivVeh
            // 
            this.btnViewCivVeh.AutoSize = true;
            this.btnViewCivVeh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewCivVeh.BackColor = System.Drawing.SystemColors.Control;
            this.btnViewCivVeh.Depth = 0;
            this.btnViewCivVeh.Icon = null;
            this.btnViewCivVeh.Location = new System.Drawing.Point(1125, 108);
            this.btnViewCivVeh.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnViewCivVeh.Name = "btnViewCivVeh";
            this.btnViewCivVeh.Primary = true;
            this.btnViewCivVeh.Size = new System.Drawing.Size(171, 36);
            this.btnViewCivVeh.TabIndex = 6;
            this.btnViewCivVeh.Text = "View Civilian Vehicle";
            this.btnViewCivVeh.UseVisualStyleBackColor = false;
            this.btnViewCivVeh.Click += new System.EventHandler(this.OnViewCivVehClick);
            // 
            // assignments
            // 
            this.assignments.BackColor = System.Drawing.SystemColors.Control;
            this.assignments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.assignments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.creation,
            this.itemSummary});
            this.assignments.ContextMenuStrip = this.assignmentsRightClick;
            this.assignments.Depth = 0;
            this.assignments.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.assignments.FullRowSelect = true;
            this.assignments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.assignments.Location = new System.Drawing.Point(6, 6);
            this.assignments.MouseLocation = new System.Drawing.Point(-1, -1);
            this.assignments.MouseState = MaterialSkin.MouseState.OUT;
            this.assignments.Name = "assignments";
            this.assignments.OwnerDraw = true;
            this.assignments.Size = new System.Drawing.Size(1263, 497);
            this.assignments.TabIndex = 9;
            this.assignments.UseCompatibleStateImageBehavior = false;
            this.assignments.View = System.Windows.Forms.View.Details;
            // 
            // creation
            // 
            this.creation.Text = "Creation Date";
            this.creation.Width = 163;
            // 
            // itemSummary
            // 
            this.itemSummary.Text = "Summary";
            this.itemSummary.Width = 1200;
            // 
            // assignmentsRightClick
            // 
            this.assignmentsRightClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.assignmentsRightClick.Depth = 0;
            this.assignmentsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.assignmentsRightClick.MouseState = MaterialSkin.MouseState.HOVER;
            this.assignmentsRightClick.Name = "rightClickMenu";
            this.assignmentsRightClick.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnAssignmentRemoveClick);
            // 
            // civvehLabel
            // 
            this.civvehLabel.AutoSize = true;
            this.civvehLabel.Depth = 0;
            this.civvehLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.civvehLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.civvehLabel.Location = new System.Drawing.Point(940, 77);
            this.civvehLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.civvehLabel.Name = "civvehLabel";
            this.civvehLabel.Size = new System.Drawing.Size(116, 19);
            this.civvehLabel.TabIndex = 10;
            this.civvehLabel.Text = "Civilian Vehicle:";
            // 
            // toggleDark
            // 
            this.toggleDark.AutoSize = true;
            this.toggleDark.Depth = 0;
            this.toggleDark.Font = new System.Drawing.Font("Roboto", 10F);
            this.toggleDark.Location = new System.Drawing.Point(20, 806);
            this.toggleDark.Margin = new System.Windows.Forms.Padding(0);
            this.toggleDark.MouseLocation = new System.Drawing.Point(-1, -1);
            this.toggleDark.MouseState = MaterialSkin.MouseState.HOVER;
            this.toggleDark.Name = "toggleDark";
            this.toggleDark.Ripple = true;
            this.toggleDark.Size = new System.Drawing.Size(97, 30);
            this.toggleDark.TabIndex = 12;
            this.toggleDark.Text = "Dark Mode";
            this.toggleDark.UseVisualStyleBackColor = true;
            this.toggleDark.CheckedChanged += new System.EventHandler(this.OnToggleDark);
            // 
            // plate
            // 
            this.plate.Depth = 0;
            this.plate.Hint = "Number Plate";
            this.plate.Location = new System.Drawing.Point(944, 108);
            this.plate.MaxLength = 32767;
            this.plate.MouseState = MaterialSkin.MouseState.HOVER;
            this.plate.Name = "plate";
            this.plate.PasswordChar = '\0';
            this.plate.ReadOnly = false;
            this.plate.SelectedText = "";
            this.plate.SelectionLength = 0;
            this.plate.SelectionStart = 0;
            this.plate.Size = new System.Drawing.Size(175, 23);
            this.plate.TabIndex = 13;
            this.plate.TabStop = false;
            this.plate.UseSystemPasswordChar = false;
            this.plate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnPlateKeyPress);
            // 
            // tabSelector
            // 
            this.tabSelector.BaseTabControl = this.tabs;
            this.tabSelector.Depth = 0;
            this.tabSelector.Location = new System.Drawing.Point(16, 155);
            this.tabSelector.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabSelector.Name = "tabSelector";
            this.tabSelector.Size = new System.Drawing.Size(1280, 52);
            this.tabSelector.TabIndex = 14;
            this.tabSelector.Text = "Selector";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tab1);
            this.tabs.Controls.Add(this.tab2);
            this.tabs.Controls.Add(this.tab3);
            this.tabs.Depth = 0;
            this.tabs.Location = new System.Drawing.Point(16, 211);
            this.tabs.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1283, 577);
            this.tabs.TabIndex = 15;
            // 
            // tab1
            // 
            this.tab1.Controls.Add(this.btnAssignmentAdd);
            this.tab1.Controls.Add(this.assignments);
            this.tab1.Location = new System.Drawing.Point(4, 22);
            this.tab1.Name = "tab1";
            this.tab1.Padding = new System.Windows.Forms.Padding(3);
            this.tab1.Size = new System.Drawing.Size(1275, 551);
            this.tab1.TabIndex = 0;
            this.tab1.Text = "Assignments";
            this.tab1.UseVisualStyleBackColor = true;
            // 
            // btnAssignmentAdd
            // 
            this.btnAssignmentAdd.AutoSize = true;
            this.btnAssignmentAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAssignmentAdd.Depth = 0;
            this.btnAssignmentAdd.Icon = null;
            this.btnAssignmentAdd.Location = new System.Drawing.Point(6, 509);
            this.btnAssignmentAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAssignmentAdd.Name = "btnAssignmentAdd";
            this.btnAssignmentAdd.Primary = true;
            this.btnAssignmentAdd.Size = new System.Drawing.Size(138, 36);
            this.btnAssignmentAdd.TabIndex = 10;
            this.btnAssignmentAdd.Text = "Add Assignment";
            this.btnAssignmentAdd.UseVisualStyleBackColor = true;
            this.btnAssignmentAdd.Click += new System.EventHandler(this.OnAddAssignmentClick);
            // 
            // tab2
            // 
            this.tab2.Controls.Add(this.officers);
            this.tab2.Location = new System.Drawing.Point(4, 22);
            this.tab2.Name = "tab2";
            this.tab2.Padding = new System.Windows.Forms.Padding(3);
            this.tab2.Size = new System.Drawing.Size(1275, 551);
            this.tab2.TabIndex = 1;
            this.tab2.Text = "Officers";
            this.tab2.UseVisualStyleBackColor = true;
            // 
            // officers
            // 
            this.officers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.officers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ofcCallsign,
            this.ofcStatus});
            this.officers.ContextMenuStrip = this.officersRightClick;
            this.officers.Depth = 0;
            this.officers.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.officers.FullRowSelect = true;
            this.officers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.officers.Location = new System.Drawing.Point(6, 6);
            this.officers.MouseLocation = new System.Drawing.Point(-1, -1);
            this.officers.MouseState = MaterialSkin.MouseState.OUT;
            this.officers.MultiSelect = false;
            this.officers.Name = "officers";
            this.officers.OwnerDraw = true;
            this.officers.Size = new System.Drawing.Size(1263, 539);
            this.officers.TabIndex = 2;
            this.officers.UseCompatibleStateImageBehavior = false;
            this.officers.View = System.Windows.Forms.View.Details;
            this.officers.DoubleClick += new System.EventHandler(this.ViewOfficer);
            // 
            // ofcCallsign
            // 
            this.ofcCallsign.Text = "Callsign";
            this.ofcCallsign.Width = 172;
            // 
            // ofcStatus
            // 
            this.ofcStatus.Text = "Status";
            this.ofcStatus.Width = 1200;
            // 
            // officersRightClick
            // 
            this.officersRightClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.officersRightClick.Depth = 0;
            this.officersRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewStripItem,
            this.setStatusStripItem,
            this.toolStripMenuItem1});
            this.officersRightClick.MouseState = MaterialSkin.MouseState.HOVER;
            this.officersRightClick.Name = "rightClickMenu";
            this.officersRightClick.Size = new System.Drawing.Size(126, 70);
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
            this.rightClickOfficerOnDuty,
            this.rightClickOfficerOffDuty,
            this.rightClickOfficerBusy});
            this.setStatusStripItem.Name = "setStatusStripItem";
            this.setStatusStripItem.Size = new System.Drawing.Size(125, 22);
            this.setStatusStripItem.Text = "Set Status";
            // 
            // rightClickOfficerOnDuty
            // 
            this.rightClickOfficerOnDuty.Name = "rightClickOfficerOnDuty";
            this.rightClickOfficerOnDuty.Size = new System.Drawing.Size(119, 22);
            this.rightClickOfficerOnDuty.Text = "On Duty";
            this.rightClickOfficerOnDuty.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // rightClickOfficerOffDuty
            // 
            this.rightClickOfficerOffDuty.Name = "rightClickOfficerOffDuty";
            this.rightClickOfficerOffDuty.Size = new System.Drawing.Size(119, 22);
            this.rightClickOfficerOffDuty.Text = "Off Duty";
            this.rightClickOfficerOffDuty.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // rightClickOfficerBusy
            // 
            this.rightClickOfficerBusy.Name = "rightClickOfficerBusy";
            this.rightClickOfficerBusy.Size = new System.Drawing.Size(119, 22);
            this.rightClickOfficerBusy.Text = "Busy";
            this.rightClickOfficerBusy.Click += new System.EventHandler(this.OnSelectStatusClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.toolStripMenuItem1.Text = "Remove";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.OnRemoveOfficerClick);
            // 
            // tab3
            // 
            this.tab3.Controls.Add(this.btnBoloAdd);
            this.tab3.Controls.Add(this.bolos);
            this.tab3.Location = new System.Drawing.Point(4, 22);
            this.tab3.Name = "tab3";
            this.tab3.Padding = new System.Windows.Forms.Padding(3);
            this.tab3.Size = new System.Drawing.Size(1275, 551);
            this.tab3.TabIndex = 2;
            this.tab3.Text = "BOLOs";
            this.tab3.UseVisualStyleBackColor = true;
            // 
            // btnBoloAdd
            // 
            this.btnBoloAdd.AutoSize = true;
            this.btnBoloAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBoloAdd.Depth = 0;
            this.btnBoloAdd.Icon = null;
            this.btnBoloAdd.Location = new System.Drawing.Point(6, 509);
            this.btnBoloAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBoloAdd.Name = "btnBoloAdd";
            this.btnBoloAdd.Primary = true;
            this.btnBoloAdd.Size = new System.Drawing.Size(86, 36);
            this.btnBoloAdd.TabIndex = 2;
            this.btnBoloAdd.Text = "Add Bolo";
            this.btnBoloAdd.UseVisualStyleBackColor = true;
            this.btnBoloAdd.Click += new System.EventHandler(this.OnAddBoloClick);
            // 
            // bolos
            // 
            this.bolos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bolos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.author,
            this.bolo});
            this.bolos.ContextMenuStrip = this.bolosRightClick;
            this.bolos.Depth = 0;
            this.bolos.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.bolos.FullRowSelect = true;
            this.bolos.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.bolos.Location = new System.Drawing.Point(6, 6);
            this.bolos.MouseLocation = new System.Drawing.Point(-1, -1);
            this.bolos.MouseState = MaterialSkin.MouseState.OUT;
            this.bolos.Name = "bolos";
            this.bolos.OwnerDraw = true;
            this.bolos.Size = new System.Drawing.Size(1263, 497);
            this.bolos.TabIndex = 1;
            this.bolos.UseCompatibleStateImageBehavior = false;
            this.bolos.View = System.Windows.Forms.View.Details;
            // 
            // author
            // 
            this.author.Text = "Sender";
            this.author.Width = 191;
            // 
            // bolo
            // 
            this.bolo.Text = "Description";
            this.bolo.Width = 1200;
            // 
            // bolosRightClick
            // 
            this.bolosRightClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bolosRightClick.Depth = 0;
            this.bolosRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoveSelected});
            this.bolosRightClick.MouseState = MaterialSkin.MouseState.HOVER;
            this.bolosRightClick.Name = "rightClickMenu";
            this.bolosRightClick.Size = new System.Drawing.Size(118, 26);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(117, 22);
            this.btnRemoveSelected.Text = "Remove";
            this.btnRemoveSelected.Click += new System.EventHandler(this.OnBoloRemoveClick);
            // 
            // btnResync
            // 
            this.btnResync.AutoSize = true;
            this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnResync.Depth = 0;
            this.btnResync.Icon = null;
            this.btnResync.Location = new System.Drawing.Point(1222, 797);
            this.btnResync.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnResync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnResync.Name = "btnResync";
            this.btnResync.Primary = false;
            this.btnResync.Size = new System.Drawing.Size(73, 36);
            this.btnResync.TabIndex = 10;
            this.btnResync.Text = "Resync";
            this.btnResync.UseVisualStyleBackColor = true;
            this.btnResync.Click += new System.EventHandler(this.OnResyncClick);
            // 
            // DispatchMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1308, 845);
            this.Controls.Add(this.btnResync);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.tabSelector);
            this.Controls.Add(this.plate);
            this.Controls.Add(this.toggleDark);
            this.Controls.Add(this.civvehLabel);
            this.Controls.Add(this.btnViewCivVeh);
            this.Controls.Add(this.btnViewCiv);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.firstName);
            this.Controls.Add(this.civLabel);
            this.MaximizeBox = false;
            this.Name = "DispatchMain";
            this.Sizable = false;
            this.Text = "Terminal";
            this.assignmentsRightClick.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.tab2.ResumeLayout(false);
            this.officersRightClick.ResumeLayout(false);
            this.tab3.ResumeLayout(false);
            this.tab3.PerformLayout();
            this.bolosRightClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel civLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField firstName;
        private MaterialSkin.Controls.MaterialSingleLineTextField lastName;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewCiv;
        private MaterialSkin.Controls.MaterialRaisedButton btnViewCivVeh;
        private MaterialSkin.Controls.MaterialListView assignments;
        private System.Windows.Forms.ColumnHeader creation;
        private System.Windows.Forms.ColumnHeader itemSummary;
        private MaterialSkin.Controls.MaterialLabel civvehLabel;
        private MaterialSkin.Controls.MaterialCheckBox toggleDark;
        private MaterialSkin.Controls.MaterialSingleLineTextField plate;
        private MaterialSkin.Controls.MaterialTabSelector tabSelector;
        private MaterialSkin.Controls.MaterialTabControl tabs;
        private System.Windows.Forms.TabPage tab1;
        private System.Windows.Forms.TabPage tab2;
        private System.Windows.Forms.TabPage tab3;
        private MaterialSkin.Controls.MaterialListView officers;
        private System.Windows.Forms.ColumnHeader ofcCallsign;
        private System.Windows.Forms.ColumnHeader ofcStatus;
        private MaterialSkin.Controls.MaterialListView bolos;
        private System.Windows.Forms.ColumnHeader author;
        private System.Windows.Forms.ColumnHeader bolo;
        private MaterialSkin.Controls.MaterialFlatButton btnResync;
        private MaterialSkin.Controls.MaterialRaisedButton btnAssignmentAdd;
        private MaterialSkin.Controls.MaterialRaisedButton btnBoloAdd;
        private MaterialSkin.Controls.MaterialContextMenuStrip assignmentsRightClick;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private MaterialSkin.Controls.MaterialContextMenuStrip bolosRightClick;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveSelected;
        private MaterialSkin.Controls.MaterialContextMenuStrip officersRightClick;
        private System.Windows.Forms.ToolStripMenuItem viewStripItem;
        private System.Windows.Forms.ToolStripMenuItem setStatusStripItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rightClickOfficerOnDuty;
        private System.Windows.Forms.ToolStripMenuItem rightClickOfficerOffDuty;
        private System.Windows.Forms.ToolStripMenuItem rightClickOfficerBusy;
    }
}