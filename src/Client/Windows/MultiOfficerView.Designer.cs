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
            this.pName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofcStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.pName,
            this.ofcStatus});
            this.officers.Depth = 0;
            this.officers.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.officers.FullRowSelect = true;
            this.officers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.officers.Location = new System.Drawing.Point(13, 74);
            this.officers.MouseLocation = new System.Drawing.Point(-1, -1);
            this.officers.MouseState = MaterialSkin.MouseState.OUT;
            this.officers.Name = "officers";
            this.officers.OwnerDraw = true;
            this.officers.Size = new System.Drawing.Size(748, 390);
            this.officers.TabIndex = 1;
            this.officers.UseCompatibleStateImageBehavior = false;
            this.officers.View = System.Windows.Forms.View.Details;
            // 
            // pName
            // 
            this.pName.Text = "Name";
            this.pName.Width = 172;
            // 
            // ofcStatus
            // 
            this.ofcStatus.Text = "Status";
            this.ofcStatus.Width = 575;
            // 
            // MultiOfficerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 524);
            this.Controls.Add(this.officers);
            this.Controls.Add(this.btnResync);
            this.MaximizeBox = false;
            this.Name = "MultiOfficerView";
            this.Sizable = false;
            this.Text = "View Officers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialFlatButton btnResync;
        private MaterialSkin.Controls.MaterialListView officers;
        private System.Windows.Forms.ColumnHeader pName;
        private System.Windows.Forms.ColumnHeader ofcStatus;
    }
}