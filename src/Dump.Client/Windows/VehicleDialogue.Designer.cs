namespace DispatchSystem.Dump.Client.Windows
{
    partial class VehicleDialogue
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.vehId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehCreation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehPlate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehStolen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehRegi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vehInsured = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.vehId,
            this.vehIp,
            this.vehCreation,
            this.vehPlate,
            this.vehOwner,
            this.vehStolen,
            this.vehRegi,
            this.vehInsured});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(785, 464);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // vehId
            // 
            this.vehId.Text = "Id";
            this.vehId.Width = 220;
            // 
            // vehIp
            // 
            this.vehIp.Text = "Source IP";
            this.vehIp.Width = 120;
            // 
            // vehCreation
            // 
            this.vehCreation.Text = "Creation Date";
            this.vehCreation.Width = 120;
            // 
            // vehPlate
            // 
            this.vehPlate.Text = "Plate";
            this.vehPlate.Width = 120;
            // 
            // vehOwner
            // 
            this.vehOwner.Text = "Owner Id";
            this.vehOwner.Width = 220;
            // 
            // vehStolen
            // 
            this.vehStolen.Text = "Stolen";
            this.vehStolen.Width = 70;
            // 
            // vehRegi
            // 
            this.vehRegi.Text = "Registered";
            this.vehRegi.Width = 70;
            // 
            // vehInsured
            // 
            this.vehInsured.Text = "Insurance";
            this.vehInsured.Width = 70;
            // 
            // VehicleDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 489);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(826, 528);
            this.MinimumSize = new System.Drawing.Size(826, 528);
            this.Name = "VehicleDialogue";
            this.Text = "Vehicles";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader vehId;
        private System.Windows.Forms.ColumnHeader vehIp;
        private System.Windows.Forms.ColumnHeader vehPlate;
        private System.Windows.Forms.ColumnHeader vehOwner;
        private System.Windows.Forms.ColumnHeader vehCreation;
        private System.Windows.Forms.ColumnHeader vehStolen;
        private System.Windows.Forms.ColumnHeader vehRegi;
        private System.Windows.Forms.ColumnHeader vehInsured;
    }
}