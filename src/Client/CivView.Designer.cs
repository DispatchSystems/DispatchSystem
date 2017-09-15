namespace Client
{
    partial class CivView
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
            System.Windows.Forms.ColumnHeader noteColumn;
            this.nameLabel = new MaterialSkin.Controls.MaterialLabel();
            this.firstNameView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lastNameView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.wantedLabel = new MaterialSkin.Controls.MaterialLabel();
            this.wantedView = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.materialDivider2 = new MaterialSkin.Controls.MaterialDivider();
            this.citationsLabel = new MaterialSkin.Controls.MaterialLabel();
            this.citationsView = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialDivider3 = new MaterialSkin.Controls.MaterialDivider();
            this.notesView = new MaterialSkin.Controls.MaterialListView();
            noteColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Depth = 0;
            this.nameLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nameLabel.Location = new System.Drawing.Point(8, 76);
            this.nameLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(53, 19);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name:";
            // 
            // firstNameView
            // 
            this.firstNameView.Depth = 0;
            this.firstNameView.Hint = "First Name";
            this.firstNameView.Location = new System.Drawing.Point(12, 111);
            this.firstNameView.MaxLength = 32767;
            this.firstNameView.MouseState = MaterialSkin.MouseState.HOVER;
            this.firstNameView.Name = "firstNameView";
            this.firstNameView.PasswordChar = '\0';
            this.firstNameView.ReadOnly = true;
            this.firstNameView.SelectedText = "";
            this.firstNameView.SelectionLength = 0;
            this.firstNameView.SelectionStart = 0;
            this.firstNameView.Size = new System.Drawing.Size(143, 23);
            this.firstNameView.TabIndex = 1;
            this.firstNameView.TabStop = false;
            this.firstNameView.UseSystemPasswordChar = false;
            // 
            // lastNameView
            // 
            this.lastNameView.Depth = 0;
            this.lastNameView.Hint = "Last Name";
            this.lastNameView.Location = new System.Drawing.Point(12, 157);
            this.lastNameView.MaxLength = 32767;
            this.lastNameView.MouseState = MaterialSkin.MouseState.HOVER;
            this.lastNameView.Name = "lastNameView";
            this.lastNameView.PasswordChar = '\0';
            this.lastNameView.ReadOnly = true;
            this.lastNameView.SelectedText = "";
            this.lastNameView.SelectionLength = 0;
            this.lastNameView.SelectionStart = 0;
            this.lastNameView.Size = new System.Drawing.Size(143, 23);
            this.lastNameView.TabIndex = 2;
            this.lastNameView.TabStop = false;
            this.lastNameView.UseSystemPasswordChar = false;
            // 
            // wantedLabel
            // 
            this.wantedLabel.AutoSize = true;
            this.wantedLabel.Depth = 0;
            this.wantedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.wantedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wantedLabel.Location = new System.Drawing.Point(206, 76);
            this.wantedLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.wantedLabel.Name = "wantedLabel";
            this.wantedLabel.Size = new System.Drawing.Size(63, 18);
            this.wantedLabel.TabIndex = 3;
            this.wantedLabel.Text = "Wanted:";
            // 
            // wantedView
            // 
            this.wantedView.AutoCheck = false;
            this.wantedView.AutoSize = true;
            this.wantedView.Depth = 0;
            this.wantedView.Font = new System.Drawing.Font("Roboto", 10F);
            this.wantedView.Location = new System.Drawing.Point(210, 111);
            this.wantedView.Margin = new System.Windows.Forms.Padding(0);
            this.wantedView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.wantedView.MouseState = MaterialSkin.MouseState.HOVER;
            this.wantedView.Name = "wantedView";
            this.wantedView.Ripple = true;
            this.wantedView.Size = new System.Drawing.Size(77, 30);
            this.wantedView.TabIndex = 4;
            this.wantedView.Text = "Wanted";
            this.wantedView.UseVisualStyleBackColor = true;
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(175, 75);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(15, 123);
            this.materialDivider1.TabIndex = 5;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // materialDivider2
            // 
            this.materialDivider2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider2.Depth = 0;
            this.materialDivider2.Location = new System.Drawing.Point(307, 75);
            this.materialDivider2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider2.Name = "materialDivider2";
            this.materialDivider2.Size = new System.Drawing.Size(15, 123);
            this.materialDivider2.TabIndex = 6;
            this.materialDivider2.Text = "materialDivider2";
            // 
            // citationsLabel
            // 
            this.citationsLabel.AutoSize = true;
            this.citationsLabel.Depth = 0;
            this.citationsLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.citationsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.citationsLabel.Location = new System.Drawing.Point(344, 76);
            this.citationsLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.citationsLabel.Name = "citationsLabel";
            this.citationsLabel.Size = new System.Drawing.Size(74, 19);
            this.citationsLabel.TabIndex = 7;
            this.citationsLabel.Text = "Citations:";
            // 
            // citationsView
            // 
            this.citationsView.Depth = 0;
            this.citationsView.Hint = "First Name";
            this.citationsView.Location = new System.Drawing.Point(348, 111);
            this.citationsView.MaxLength = 32767;
            this.citationsView.MouseState = MaterialSkin.MouseState.HOVER;
            this.citationsView.Name = "citationsView";
            this.citationsView.PasswordChar = '\0';
            this.citationsView.ReadOnly = true;
            this.citationsView.SelectedText = "";
            this.citationsView.SelectionLength = 0;
            this.citationsView.SelectionStart = 0;
            this.citationsView.Size = new System.Drawing.Size(70, 23);
            this.citationsView.TabIndex = 8;
            this.citationsView.TabStop = false;
            this.citationsView.UseSystemPasswordChar = false;
            // 
            // materialDivider3
            // 
            this.materialDivider3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider3.Depth = 0;
            this.materialDivider3.Location = new System.Drawing.Point(12, 213);
            this.materialDivider3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider3.Name = "materialDivider3";
            this.materialDivider3.Size = new System.Drawing.Size(412, 14);
            this.materialDivider3.TabIndex = 9;
            this.materialDivider3.Text = "materialDivider3";
            // 
            // noteColumn
            // 
            noteColumn.Text = "Notes";
            noteColumn.Width = 500;
            // 
            // notesView
            // 
            this.notesView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.notesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            noteColumn});
            this.notesView.Depth = 0;
            this.notesView.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.notesView.FullRowSelect = true;
            this.notesView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.notesView.Location = new System.Drawing.Point(13, 247);
            this.notesView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.notesView.MouseState = MaterialSkin.MouseState.OUT;
            this.notesView.Name = "notesView";
            this.notesView.OwnerDraw = true;
            this.notesView.Size = new System.Drawing.Size(405, 258);
            this.notesView.TabIndex = 10;
            this.notesView.UseCompatibleStateImageBehavior = false;
            this.notesView.View = System.Windows.Forms.View.Details;
            // 
            // CivView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(436, 517);
            this.Controls.Add(this.notesView);
            this.Controls.Add(this.materialDivider3);
            this.Controls.Add(this.citationsView);
            this.Controls.Add(this.citationsLabel);
            this.Controls.Add(this.materialDivider2);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.wantedView);
            this.Controls.Add(this.wantedLabel);
            this.Controls.Add(this.lastNameView);
            this.Controls.Add(this.firstNameView);
            this.Controls.Add(this.nameLabel);
            this.MaximizeBox = false;
            this.Name = "CivView";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Civilian View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel nameLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField firstNameView;
        private MaterialSkin.Controls.MaterialSingleLineTextField lastNameView;
        private MaterialSkin.Controls.MaterialLabel wantedLabel;
        private MaterialSkin.Controls.MaterialCheckBox wantedView;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialDivider materialDivider2;
        private MaterialSkin.Controls.MaterialLabel citationsLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField citationsView;
        private MaterialSkin.Controls.MaterialDivider materialDivider3;
        private MaterialSkin.Controls.MaterialListView notesView;
    }
}