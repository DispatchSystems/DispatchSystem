using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class MultiOfficerView : MaterialForm, ISyncable
    {
        private StorageManager<Officer> data;

        public MultiOfficerView(StorageManager<Officer> input)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            data = input;
            UpdateCurrentInformation();
        }

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public void UpdateCurrentInformation()
        {
            List<ListViewItem> lvis = new List<ListViewItem>();

            officers.Items.Clear();

            foreach (Officer ofc in data)
            {
                ListViewItem lvi = new ListViewItem(ofc.Callsign);
                lvi.SubItems.Add(ofc.Status == OfficerStatus.OffDuty ? "Off Duty" : ofc.Status == OfficerStatus.OnDuty ? "On Duty" : "Busy");
                lvis.Add(lvi);
            }

            officers.Items.AddRange(lvis.ToArray());
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

            StorageManager<Officer> result = await Program.Client.Peer.RemoteCallbacks.Functions["GetOfficers"].Invoke<StorageManager<Officer>>();
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    data = result;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            IsCurrentlySyncing = false;
        }

        private async void OnResyncClick(object sender, EventArgs e) =>
#if DEBUG
            await Resync(true);
#else
            await Resync(false);
#endif

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (officers.FocusedItem.Bounds.Contains(e.Location))
            {
                rightClickMenu.Show(Cursor.Position);
            }
        }
        private async void OnSelectStatusClick(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            Officer ofc = data[index];

            do
            {
                if (sender == statusOnDutyStripItem)
                {
                    if (ofc.Status == OfficerStatus.OnDuty)
                    {
                        MessageBox.Show("Really? That officer is already on duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc, OfficerStatus.OnDuty);
                }
                else if (sender == statusOffDutyStripItem)
                {
                    if (ofc.Status == OfficerStatus.OffDuty)
                    {
                        MessageBox.Show("Really? That officer is already off duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc, OfficerStatus.OffDuty);
                }
                else
                {
                    if (ofc.Status == OfficerStatus.Busy)
                    {
                        MessageBox.Show("Really? That officer is already busy!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc, OfficerStatus.Busy);
                }
            } while (false);

            await Resync(true); 
        }

        private void ViewOfficer(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            Officer ofc = data[index];

            new OfficerView(ofc).Show();
        }

        private async void OnRemoveOfficerClick(object sender, EventArgs e)
        {
            ListViewItem focusesItem = officers.FocusedItem;
            int index = officers.Items.IndexOf(focusesItem);
            Officer ofc = data[index];

            await Program.Client.Peer.RemoteCallbacks.Events["RemoveOfficer"].Invoke(ofc.Id);

            await Resync(true);
        }
    }
}
