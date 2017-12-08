using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.Client.Windows
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

            SkinManager.AddFormToManage(this);
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
            if (assignment is null) return;

            ListViewItem lvi = new ListViewItem(assignment.Creation.ToString("HH:mm:ss"));
            lvi.SubItems.Add(assignment.Summary);
            materialListView1.Items.Add(lvi);
        }

        public async Task SetAssignment()
        {
            assignment = await Program.Client.Peer.RemoteCallbacks.Functions["GetOfficerAssignment"]
                .Invoke<Assignment>(ofc.Id);

            UpdateCurrentInformation();
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

            Officer result = await Program.Client.Peer.RemoteCallbacks.Functions["GetOfficer"].Invoke<Officer>(ofc.Id);
            if (result != null)
            {
                if (result.SourceIP != string.Empty && result.Callsign != string.Empty)
                {
                    Invoke((MethodInvoker)async delegate
                    {
                        ofc = result;
                        await SetAssignment();
                    });
                }
                else
                    MessageBox.Show("The officer profile has been deleted!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            IsCurrentlySyncing = false;
        }

        private async void StatusClick(object sender, EventArgs e)
        {
            do
            {
                if (sender == radioOnDuty)
                {
                    if (ofc.Status == OfficerStatus.OnDuty)
                    {
                        MessageBox.Show("Really? That officer is already on duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await Program.Client.Peer.RemoteCallbacks.Events["SetStatus"].Invoke(ofc.Id, OfficerStatus.OnDuty);
                }
                else if (sender == radioOffDuty)
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
                    await Program.Client.Peer.RemoteCallbacks.Events["AddOfficerAssignment"]
                        .Invoke(addBtn.LastGuid, ofc.Id);

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

            await Program.Client.Peer.RemoteCallbacks.Events["RemoveOfficerAssignment"].Invoke(ofc.Id);

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
