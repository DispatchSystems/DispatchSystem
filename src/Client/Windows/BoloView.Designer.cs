namespace DispatchSystem.cl.Windows
{
    partial class BoloView
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
            this.bolosView = new MaterialSkin.Controls.MaterialListView();
            this.author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bolo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResync = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnAddBolo = new MaterialSkin.Controls.MaterialRaisedButton();
            this.rightClickMenu = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.btnRemoveSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // bolosView
            // 
            this.bolosView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bolosView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.author,
            this.bolo});
            this.bolosView.Depth = 0;
            this.bolosView.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.bolosView.FullRowSelect = true;
            this.bolosView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.bolosView.Location = new System.Drawing.Point(13, 77);
            this.bolosView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.bolosView.MouseState = MaterialSkin.MouseState.OUT;
            this.bolosView.Name = "bolosView";
            this.bolosView.OwnerDraw = true;
            this.bolosView.Size = new System.Drawing.Size(968, 429);
            this.bolosView.TabIndex = 0;
            this.bolosView.UseCompatibleStateImageBehavior = false;
            this.bolosView.View = System.Windows.Forms.View.Details;
            this.bolosView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            // 
            // author
            // 
            this.author.Text = "Sender";
            this.author.Width = 191;
            // 
            // bolo
            // 
            this.bolo.Text = "Description";
            this.bolo.Width = 976;
            // 
            // btnResync
            // 
            this.btnResync.AutoSize = true;
            this.btnResync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnResync.Depth = 0;
            this.btnResync.Icon = null;
            this.btnResync.Location = new System.Drawing.Point(907, 515);
            this.btnResync.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnResync.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnResync.Name = "btnResync";
            this.btnResync.Primary = true;
            this.btnResync.Size = new System.Drawing.Size(73, 36);
            this.btnResync.TabIndex = 1;
            this.btnResync.Text = "Resync";
            this.btnResync.UseVisualStyleBackColor = true;
            this.btnResync.Click += new System.EventHandler(this.OnResyncClick);
            // 
            // btnAddBolo
            // 
            this.btnAddBolo.AutoSize = true;
            this.btnAddBolo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddBolo.Depth = 0;
            this.btnAddBolo.Icon = null;
            this.btnAddBolo.Location = new System.Drawing.Point(13, 514);
            this.btnAddBolo.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddBolo.Name = "btnAddBolo";
            this.btnAddBolo.Primary = true;
            this.btnAddBolo.Size = new System.Drawing.Size(86, 36);
            this.btnAddBolo.TabIndex = 2;
            this.btnAddBolo.Text = "Add Bolo";
            this.btnAddBolo.UseVisualStyleBackColor = true;
            this.btnAddBolo.Click += new System.EventHandler(this.OnAddBoloClick);
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rightClickMenu.Depth = 0;
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoveSelected});
            this.rightClickMenu.MouseState = MaterialSkin.MouseState.HOVER;
            this.rightClickMenu.Name = "rightClickMenu";
            this.rightClickMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(117, 22);
            this.btnRemoveSelected.Text = "Remove";
            this.btnRemoveSelected.Click += new System.EventHandler(this.OnRemoveSelectedClick);
            // 
            // BoloView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 564);
            this.Controls.Add(this.btnAddBolo);
            this.Controls.Add(this.btnResync);
            this.Controls.Add(this.bolosView);
            this.MaximizeBox = false;
            this.Name = "BoloView";
            this.Sizable = false;
            this.Text = "Active BOLOs";
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView bolosView;
        private System.Windows.Forms.ColumnHeader author;
        private System.Windows.Forms.ColumnHeader bolo;
        private MaterialSkin.Controls.MaterialFlatButton btnResync;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddBolo;
        private MaterialSkin.Controls.MaterialContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveSelected;
    }
}