using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class OfficerView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        private Officer ofc;
        private Assignment assignment;
        private AddRemoveView addBtn;
        private AddExistingAssignment addExisting;

        public OfficerView(Officer data)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");

            InitializeComponent();

            ofc = data;
            new Action(async delegate { await SetAssignment(); })();
        }

        public void UpdateCurrentInformation()
        {
            nameView.Text = ofc.Callsign;
            clockedView.Text = ofc.Creation.ToLocalTime().ToString("HH:mm:ss");
            switch (ofc.Status)
            {
                case OfficerStatus.OnDuty:
                    radioOnDuty.Checked = true;
                    break;
                case OfficerStatus.OffDuty:
                    radioOffDuty.Checked = true;
                    break;
                case OfficerStatus.Busy:
                    radioBusy.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            materialListView1.Items.Clear();
            if (!(assignment is null))
            {
                ListViewItem lvi = new ListViewItem(assignment.Creation.ToString("HH:mm:ss"));
                lvi.SubItems.Add(assignment.Summary);
                materialListView1.Items.Add(lvi);
            }
        }

        public async Task SetAssignment()
        {
            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Tuple<NetRequestResult, Assignment> result = await handle.TryTriggerNetFunction<Assignment>("GetOfficerAssignment", ofc.Id);
                handle.Disconnect();

                assignment = result.Item2;
            }

            UpdateCurrentInformation();
        }
        public async Task Resync(bool skipTime)
        {
            if (((DateTime.Now - LastSyncTime).Seconds < 15 || IsCurrentlySyncing) && !skipTime)
            {
                MessageBox.Show($"You must wait 15 seconds before the last sync time \nSeconds to wait: {15 - (DateTime.Now - LastSyncTime).Seconds}", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LastSyncTime = DateTime.Now;
            IsCurrentlySyncing = true;

            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Tuple<NetRequestResult, Officer> result = await handle.TryTriggerNetFunction<Officer>("GetOfficer", ofc.Id);
                handle.Disconnect();

                if (result.Item2 != null)
                {
                    if (result.Item2.SourceIP != string.Empty && result.Item2.Callsign != string.Empty)
                    {
                        Invoke((MethodInvoker)async delegate
                        {
                            ofc = result.Item2;
                            await SetAssignment();
                        });
                    }
                    else
                        MessageBox.Show("The officer profile has been deleted!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                    MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsCurrentlySyncing = false;
        }

        private async void StatusClick(object sender, EventArgs e)
        {
            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                do
                {
                    if (sender == radioOnDuty)
                    {
                        if (ofc.Status == OfficerStatus.OnDuty)
                        {
                            MessageBox.Show("Really? That officer is already on duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            break;
                        }

                        await handle.TryTriggerNetEvent("SetStatus", ofc, OfficerStatus.OnDuty);
                    }
                    else if (sender == radioOffDuty)
                    {
                        if (ofc.Status == OfficerStatus.OffDuty)
                        {
                            MessageBox.Show("Really? That officer is already off duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            break;
                        }

                        await handle.TryTriggerNetEvent("SetStatus", ofc, OfficerStatus.OffDuty);
                    }
                    else
                    {
                        if (ofc.Status == OfficerStatus.Busy)
                        {
                            MessageBox.Show("Really? That officer is already busy!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            break;
                        }

                        await handle.TryTriggerNetEvent("SetStatus", ofc, OfficerStatus.Busy);
                    }
                } while (false);

                handle.Disconnect();
            }

            await Resync(true);
        }

        private void OnCreateNewAssignment(object sender, EventArgs e)
        {
            if (addBtn != null)
            {
                MessageBox.Show("You cannot have 2 instanced of this window open at the same time!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Invoke((MethodInvoker)delegate
            {
                (addBtn = new AddRemoveView(AddRemoveView.Type.AddAssignment)).Show();
                addBtn.FormClosed += async delegate
                {
                    using (Client handle = new Client())
                    {
                        try { handle.Connect(Config.IP.ToString(), Config.Port); }
                        catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        await handle.TriggerNetEvent("AddOfficerAssignment", addBtn.LastGuid, ofc.Id);
                    }

                    addBtn = null;
                    await Resync(true);
                };
            });
        }

        private async void OnResyncClick(object sender, EventArgs e)
        {
#if DEBUG
            await Resync(true);
#else
            await Resync(false);
#endif
        }

        private async void OnRemoveSelectedClick(object sender, EventArgs e)
        {
            if (materialListView1.FocusedItem == null)
                return;

            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                await handle.TryTriggerNetEvent("RemoveOfficerAssignment", ofc.Id);

                handle.Disconnect();
            }

            await Resync(true);
        }

        private void OnAddToExistingClick(object sender, EventArgs e)
        {
            if (addExisting != null)
            {
                MessageBox.Show("You cannot have 2 instances of the same window open", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            (addExisting = new AddExistingAssignment(ofc)).Show();
            addExisting.FormClosed += async delegate
            {
                addExisting = null;
                await Resync(true);
            };
        }
    }
}
