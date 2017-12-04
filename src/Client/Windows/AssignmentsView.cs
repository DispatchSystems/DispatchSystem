using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

namespace DispatchSystem.Client.Windows
{
    public partial class AssignmentsView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; }
        private AddRemoveView addBtn;

        private IEnumerable<Assignment> assignments;

        public AssignmentsView(IEnumerable<Assignment> data)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            assignments = data;
            UpdateCurrentInformation();
        }

        public void UpdateCurrentInformation()
        {
            theAssignments.Items.Clear();
            foreach (var item in assignments)
            {
                ListViewItem lvi = new ListViewItem(item.Creation.ToLocalTime().ToString("HH:mm:ss"));
                lvi.SubItems.Add(item.Summary);
                theAssignments.Items.Add(lvi);
            }
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

            var result = await Program.Client.Peer.RemoteCallbacks.Properties["Assignments"]
                .Get<StorageManager<Assignment>>();
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    assignments = result;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            IsCurrentlySyncing = false;
        }

        private void OnAddAssignmentClick(object sender, EventArgs e)
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

        private void OnAssignmentsClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (theAssignments.FocusedItem.Bounds.Contains(e.Location))
            {
                rightClickMenu.Show(Cursor.Position);
            }
        }

        private async void OnRightClickRemove(object sender, EventArgs e)
        {
            int index = theAssignments.Items.IndexOf(theAssignments.FocusedItem);
            Assignment assignment = assignments.ToList()[index];

            await Program.Client.Peer.RemoteCallbacks.Events["RemoveAssignment"].Invoke(assignment.Id);

            await Resync(true);
        }
    }
}
