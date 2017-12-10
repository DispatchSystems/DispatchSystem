using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

namespace DispatchSystem.Client.Windows
{
    public partial class DispatchMain : MaterialForm, ISyncable
    {
        private StorageManager<Assignment> a;
        private StorageManager<Bolo> b;
        private StorageManager<Officer> o;

        private AddRemoveView addBtn;

        public DateTime LastSyncTime { get; private set; } = DateTime.Now;
        public bool IsCurrentlySyncing { get; private set; }

        public DispatchMain()
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            SkinManager.AddFormToManage(this);
            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            SkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple500, Primary.DeepPurple700,
                Primary.DeepPurple400, Accent.DeepPurple200, TextShade.WHITE);

            // Starting Timer
            var timer = new Timer();

            void Sync(object sender, EventArgs args)
            {
#pragma warning disable 4014 // Non-awaited task warning
                Resync(true);
#pragma warning restore 4014
            }
            timer.Tick += Sync;

            Sync(this, EventArgs.Empty);

            timer.Interval = 15000;
            timer.Start();
        }

        private void UpdateAssignmentInformation()
        {
            assignments.Items.Clear();
            foreach (var item in a)
            {
                ListViewItem lvi = new ListViewItem(item.Creation.ToLocalTime().ToString("HH:mm:ss"));
                lvi.SubItems.Add(item.Summary);
                assignments.Items.Add(lvi);
            }
        }
        private void UpdateOfficerInformation()
        {
            officers.Items.Clear();
            foreach (Officer ofc in o)
            {
                ListViewItem lvi = new ListViewItem(ofc.Callsign);
                lvi.SubItems.Add(ofc.Status == OfficerStatus.OffDuty ? "Off Duty" : ofc.Status == OfficerStatus.OnDuty ? "On Duty" : "Busy");
                officers.Items.Add(lvi);
            }
        }
        private void UpdateBoloInformation()
        {
            bolos.Items.Clear();
            foreach (Bolo t in b)
            {
                ListViewItem lvi = new ListViewItem(t.Player);
                lvi.SubItems.Add(t.Reason);
                bolos.Items.Add(lvi);
            }
        }
        public void UpdateCurrentInformation()
        {
            UpdateAssignmentInformation();

            UpdateOfficerInformation();

            UpdateBoloInformation();
        }
        private async Task SyncAssignments()
        {
            var result = await Program.Client.Peer.RemoteCallbacks.Properties["Assignments"]
                .Get<StorageManager<Assignment>>();
            if (result != null)
            {
                a = result;
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async Task SyncBolos()
        {
            var result = await Program.Client.Peer.RemoteCallbacks.Properties["Bolos"]
                .Get<StorageManager<Bolo>>();
            if (result != null)
            {
                b = result;
            }
            else
                MessageBox.Show("FATAL: Invalid BOLOs list", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async Task SyncOfficers()
        {
            var result = await Program.Client.Peer.RemoteCallbacks.Properties["Officers"].Get<StorageManager<Officer>>();
            if (result != null)
            {
                o = result;
            }
            else
                MessageBox.Show("FATAL: Invalid officers list", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public async Task Resync(bool skipTime)
        {
            if (((DateTime.Now - LastSyncTime).Seconds < 5 || IsCurrentlySyncing) && !skipTime)
            {
                MessageBox.Show($"You must wait 5 seconds before the last sync time \nSeconds to wait: {5 - (DateTime.Now - LastSyncTime).Seconds}", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LastSyncTime = DateTime.Now;
            IsCurrentlySyncing = true;

            await SyncAssignments();
            await SyncBolos();
            await SyncOfficers();

            UpdateCurrentInformation();

            IsCurrentlySyncing = false;
        }

        public async void OnViewCivClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text))
                return;

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetCivilian"]
                .Invoke<Civilian>(firstName.Text, lastName.Text);
            if (result != null)
            {
                Invoke((MethodInvoker) delegate
                {
                    CivView civ = new CivView(result);
                    civ.Show();
                });
            }
            else
                MessageBox.Show("That name doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

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
                Invoke((MethodInvoker)delegate
                {
                    new CivVehView(result).Show();
                });
            }
            else
                MessageBox.Show("That plate doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            plate.ResetText();
        }

        private void OnFirstNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = firstName.Text.Length == 0 ? char.ToUpper(e.KeyChar) : char.ToLower(e.KeyChar);
        }

        private void OnLastNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = lastName.Text.Length == 0 ? char.ToUpper(e.KeyChar) : char.ToLower(e.KeyChar);
                
        }

        private void OnPlateKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != ' ' || plate.Text.Length >= 8 && !e.KeyChar.Equals('\b'))
            {
                e.Handled = true;
                
            }
            if (char.IsLetter(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void OnToggleDark(object sender, EventArgs e)
        {
            SkinManager.Theme = SkinManager.Theme == MaterialSkinManager.Themes.DARK
                ? MaterialSkinManager.Themes.LIGHT
                : MaterialSkinManager.Themes.DARK;
        }

        private async void OnResyncClick(object sender, EventArgs e) => await Resync(true);

        private void OnAddAssignmentClick(object sender, EventArgs e)
        {
            if (addBtn != null)
            {
                MessageBox.Show("You cannot have 2 instanced of this window open at the same time!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            (addBtn = new AddRemoveView(AddRemoveView.Type.AddAssignment)).Show();
            addBtn.FormClosed += async delegate
            {
                addBtn = null;
                await SyncAssignments();

                Invoke((MethodInvoker) UpdateAssignmentInformation);
            };
        }
        private void OnAddBoloClick(object sender, EventArgs e)
        {
            if (addBtn != null)
            {
                MessageBox.Show("You cannot have 2 instanced of this window open at the same time!", "DispatchSystem",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            (addBtn = new AddRemoveView(AddRemoveView.Type.AddBolo)).Show();
            addBtn.FormClosed += async delegate
            {
                addBtn = null;
                await SyncBolos();

                Invoke((MethodInvoker) UpdateBoloInformation);
            };
        }

        // Assignments Right Click Menu
        private async void OnAssignmentRemoveClick(object sender, EventArgs e)
        {
            int index = assignments.Items.IndexOf(assignments.FocusedItem);
            if (index == -1)
                return;

            Assignment assignment = a[index];

            await Program.Client.Peer.RemoteCallbacks.Events["RemoveAssignment"].Invoke(assignment.Id);

            await SyncAssignments();
            Invoke((MethodInvoker) UpdateAssignmentInformation);
        }

        // BOLOs Right Click Menu
        private async void OnBoloRemoveClick(object sender, EventArgs e)
        {
            if (bolos.SelectedItems.Count > 0)
            {
                await Program.Client.Peer.RemoteCallbacks.Events["RemoveBolo"]
                    .Invoke(bolos.Items.IndexOf(bolos.SelectedItems[0]));

                await SyncBolos();
                Invoke((MethodInvoker) UpdateBoloInformation);
            }
        }

        // Officers Right Click Menu
        private async void OnSelectStatusClick(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            if (index == -1)
                return;
            Officer ofc = o[index];

            do
            {
                if (sender == rightClickOfficerOnDuty)
                {
                    if (ofc.Status == OfficerStatus.OnDuty)
                    {
                        MessageBox.Show("Really? That officer is already on duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc.Id, OfficerStatus.OnDuty);
                }
                else if (sender == rightClickOfficerOffDuty)
                {
                    if (ofc.Status == OfficerStatus.OffDuty)
                    {
                        MessageBox.Show("Really? That officer is already off duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc.Id, OfficerStatus.OffDuty);
                }
                else
                {
                    if (ofc.Status == OfficerStatus.Busy)
                    {
                        MessageBox.Show("Really? That officer is already busy!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc.Id, OfficerStatus.Busy);
                }
            } while (false);

            await SyncOfficers();
            Invoke((MethodInvoker) UpdateOfficerInformation);
        }
        private void ViewOfficer(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            if (index == -1)
                return;
            Officer ofc = o[index];

            new OfficerView(ofc).Show();
        }
        private async void OnRemoveOfficerClick(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            if (index == -1)
                return;
            Officer ofc = o[index];

            await Program.Client.Peer.RemoteCallbacks.Events["RemoveOfficer"].Invoke(ofc.Id);

            await SyncOfficers();
            Invoke((MethodInvoker) UpdateOfficerInformation);
        }
    }
}
