namespace DispatchSystem.cl.Windows
{
    partial class AddRemoveView
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
            this.addRemoveBtn = new MaterialSkin.Controls.MaterialRaisedButton();
            this.line1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.line2 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.SuspendLayout();
            // 
            // addRemoveBtn
            // 
            this.addRemoveBtn.AutoSize = true;
            this.addRemoveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addRemoveBtn.Depth = 0;
            this.addRemoveBtn.Icon = null;
            this.addRemoveBtn.Location = new System.Drawing.Point(13, 185);
            this.addRemoveBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.addRemoveBtn.Name = "addRemoveBtn";
            this.addRemoveBtn.Primary = true;
            this.addRemoveBtn.Size = new System.Drawing.Size(16, 36);
            this.addRemoveBtn.TabIndex = 0;
            this.addRemoveBtn.UseVisualStyleBackColor = true;
            this.addRemoveBtn.Click += new System.EventHandler(this.OnBtnClick);
            // 
            // line1
            // 
            this.line1.Depth = 0;
            this.line1.Hint = "";
            this.line1.Location = new System.Drawing.Point(13, 85);
            this.line1.MaxLength = 32767;
            this.line1.MouseState = MaterialSkin.MouseState.HOVER;
            this.line1.Name = "line1";
            this.line1.PasswordChar = '\0';
            this.line1.ReadOnly = false;
            this.line1.SelectedText = "";
            this.line1.SelectionLength = 0;
            this.line1.SelectionStart = 0;
            this.line1.Size = new System.Drawing.Size(275, 23);
            this.line1.TabIndex = 1;
            this.line1.TabStop = false;
            this.line1.UseSystemPasswordChar = false;
            // 
            // line2
            // 
            this.line2.Depth = 0;
            this.line2.Hint = "Dispatcher Name";
            this.line2.Location = new System.Drawing.Point(13, 130);
            this.line2.MaxLength = 32767;
            this.line2.MouseState = MaterialSkin.MouseState.HOVER;
            this.line2.Name = "line2";
            this.line2.PasswordChar = '\0';
            this.line2.ReadOnly = false;
            this.line2.SelectedText = "";
            this.line2.SelectionLength = 0;
            this.line2.SelectionStart = 0;
            this.line2.Size = new System.Drawing.Size(275, 23);
            this.line2.TabIndex = 2;
            this.line2.TabStop = false;
            this.line2.UseSystemPasswordChar = false;
            // 
            // AddRemoveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(300, 234);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.addRemoveBtn);
            this.MaximizeBox = false;
            this.Name = "AddRemoveView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton addRemoveBtn;
        private MaterialSkin.Controls.MaterialSingleLineTextField line1;
        private MaterialSkin.Controls.MaterialSingleLineTextField line2;
    }
}