using System;
using System.Windows.Forms;

namespace DispatchSystem.Dump.Client.Windows
{
    public partial class DumpDialogue : Form
    {
        private readonly Dump information;

        public DumpDialogue(Dump information)
        {
            this.information = information;
            InitializeComponent();
        }

        private void PermissionsClick(object sender, EventArgs e)
        {
            new PermissionsDialogue(information.Permissions).Show();
        }

        private void CiviliansClick(object sender, EventArgs e)
        {
            new CivilianDialogue(information.Civilians).Show();
        }

        private void VehiclesClick(object sender, EventArgs e)
        {
            new VehicleDialogue(information.Vehicles).Show();
        }

        private void BolosClick(object sender, EventArgs e)
        {
            new BolosDialogue(information.Bolos).Show();
        }

        private void CallsClick(object sender, EventArgs e)
        {
            new CallDialogue(information.EmergencyCalls).Show();
        }

        private void OfficersClick(object sender, EventArgs e)
        {
            new OfficerDialogue(information.Officers).Show();
        }
    }
}
