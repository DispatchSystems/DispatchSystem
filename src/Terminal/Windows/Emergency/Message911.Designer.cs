namespace DispatchSystem.Terminal.Windows.Emergency
{
    partial class Message911
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
            this.btnSend = new MaterialSkin.Controls.MaterialRaisedButton();
            this.msgBox = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.msgs = new MaterialSkin.Controls.MaterialListView();
            this.timestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.msg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.AutoSize = true;
            this.btnSend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSend.Depth = 0;
            this.btnSend.Icon = null;
            this.btnSend.Location = new System.Drawing.Point(627, 622);
            this.btnSend.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSend.Name = "btnSend";
            this.btnSend.Primary = true;
            this.btnSend.Size = new System.Drawing.Size(56, 36);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.SendMsg);
            // 
            // msgBox
            // 
            this.msgBox.Depth = 0;
            this.msgBox.Hint = "Message...";
            this.msgBox.Location = new System.Drawing.Point(13, 629);
            this.msgBox.MaxLength = 32767;
            this.msgBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.msgBox.Name = "msgBox";
            this.msgBox.PasswordChar = '\0';
            this.msgBox.ReadOnly = false;
            this.msgBox.SelectedText = "";
            this.msgBox.SelectionLength = 0;
            this.msgBox.SelectionStart = 0;
            this.msgBox.Size = new System.Drawing.Size(608, 23);
            this.msgBox.TabIndex = 1;
            this.msgBox.TabStop = false;
            this.msgBox.UseSystemPasswordChar = false;
            this.msgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendMsg);
            // 
            // msgs
            // 
            this.msgs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.msgs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestamp,
            this.name,
            this.msg});
            this.msgs.Depth = 0;
            this.msgs.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.msgs.FullRowSelect = true;
            this.msgs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.msgs.Location = new System.Drawing.Point(12, 75);
            this.msgs.MouseLocation = new System.Drawing.Point(-1, -1);
            this.msgs.MouseState = MaterialSkin.MouseState.OUT;
            this.msgs.Name = "msgs";
            this.msgs.OwnerDraw = true;
            this.msgs.Size = new System.Drawing.Size(671, 541);
            this.msgs.TabIndex = 2;
            this.msgs.UseCompatibleStateImageBehavior = false;
            this.msgs.View = System.Windows.Forms.View.Details;
            // 
            // timestamp
            // 
            this.timestamp.Text = "Time";
            this.timestamp.Width = 65;
            // 
            // name
            // 
            this.name.Text = "Sender";
            this.name.Width = 128;
            // 
            // msg
            // 
            this.msg.Text = "Text";
            this.msg.Width = 764;
            // 
            // Message911
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(695, 670);
            this.Controls.Add(this.msgs);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.btnSend);
            this.MaximizeBox = false;
            this.Name = "Message911";
            this.Sizable = false;
            this.Text = "911 Call: ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton btnSend;
        private MaterialSkin.Controls.MaterialSingleLineTextField msgBox;
        private MaterialSkin.Controls.MaterialListView msgs;
        private System.Windows.Forms.ColumnHeader timestamp;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader msg;
    }
}