using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class DispatchMain : MaterialForm
    {
        private BoloView boloWindow;
        private MultiOfficerView officersWindow;
        private AssignmentsView assignmentsWindow;

        public DispatchMain()
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            SkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple500, Primary.DeepPurple700, Primary.DeepPurple400, Accent.DeepPurple400, TextShade.WHITE);
        }

        public async void OnViewCivClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text))
                return;

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetCivilian"]
                .Invoke<Civilian>(firstName.Text, lastName.Text);
            if (result != null)
            {
                if (!(string.IsNullOrEmpty(result.First) || string.IsNullOrEmpty(result.Last))) // Checking if the civilian is empty bc for some reason == and .Equals are not working for this situation
                {
                    Invoke((MethodInvoker)delegate
                    {
                        new CivView(result).Show();
                    });
                }
                else
                    MessageBox.Show("That name doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("Invalid request", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            firstName.ResetText();
            lastName.ResetText();
        }

        private async void OnViewCivVehClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(plate.Text))
                return;

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetCivilianVeh"]
                .Invoke<CivilianVeh>(plate.Text);
            if (result != null)
            {
                if (!(string.IsNullOrEmpty(result.Plate)))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        new CivVehView(result).Show();
                    });
                }
                else
                    MessageBox.Show("That plate doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("Invalid Request", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            plate.ResetText();
        }

        private async void OnViewBolosClick(object sender, EventArgs e)
        {
            if (boloWindow != null)
            {
                MessageBox.Show("You cannot have 2 instances of the Bolos window open at the same time!\n" +
                    "Try pressing the Resync button inside your bolo window.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetBolos"].Invoke<StorageManager<Bolo>>();
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    (boloWindow = new BoloView(result)).Show();
                    boloWindow.FormClosed += delegate { boloWindow = null; };
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void OnViewOfficersClick(object sender, EventArgs e)
        {
            if (officersWindow != null)
            {
                MessageBox.Show("You cannot have 2 instances of the Officers window open at the same time!\n" +
                    "Try pressing the Resync button inside your officers window.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetOfficers"].Invoke<StorageManager<Officer>>();
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    (officersWindow = new MultiOfficerView(result)).Show();
                    officersWindow.FormClosed += delegate { officersWindow = null; };
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async void OnViewAssignmentsClick(object sender, EventArgs e)
        {
            if (assignmentsWindow != null)
            {
                MessageBox.Show("You cannot have 2 instances of the Assignments window open at the same time!\n" +
                    "Try pressing the Resync button inside your officers window.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetAssignments"]
                .Invoke<IEnumerable<Assignment>>();
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    (assignmentsWindow = new AssignmentsView(result)).Show();
                    assignmentsWindow.FormClosed += delegate { assignmentsWindow = null; };
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnFirstNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void OnLastNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void OnPlateKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != ' ') || (plate.Text.Length >= 8 && !e.KeyChar.Equals('\b')))
            {
                e.Handled = true;
                
            }
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
        }
    }
}
