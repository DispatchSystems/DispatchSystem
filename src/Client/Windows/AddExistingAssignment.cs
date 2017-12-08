using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.Client.Windows
{
    public partial class AddExistingAssignment : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; }

        private readonly Officer ofc;
        private IEnumerable<Assignment> assignments;

        public AddExistingAssignment(Officer ofc)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            this.ofc = ofc;

            SkinManager.AddFormToManage(this);

            ThreadPool.QueueUserWorkItem(async x =>
            {
                await Resync(true);
                Invoke((MethodInvoker)UpdateCurrentInformation);
            });
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

            IEnumerable<Assignment> result = await Program.Client.Peer.RemoteCallbacks.Properties["Assignments"].Get<IEnumerable<Assignment>>();
            if (result != null)
            {
                while (!IsHandleCreated)
                    await Task.Delay(50);
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
        public void UpdateCurrentInformation()
        {
            assignmentsView.Items.Clear();
            foreach (var item in assignments)
            {
                ListViewItem lvi = new ListViewItem(item.Creation.ToString("HH:mm:ss"));
                lvi.SubItems.Add(item.Summary);
                assignmentsView.Items.Add(lvi);
            }
        }

        private async void OnDoubleClick(object sender, EventArgs e)
        {
            if (assignmentsView.FocusedItem == null)
                return;

            int index = assignmentsView.Items.IndexOf(assignmentsView.FocusedItem);
            Assignment assignment = assignments.ToList()[index];

            await Program.Client.Peer.RemoteCallbacks.Events["AddOfficerAssignment"].Invoke(assignment.Id, ofc.Id);

            Close();
        }
    }
}
