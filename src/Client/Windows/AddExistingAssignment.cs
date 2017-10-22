using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.NetCode;

namespace DispatchSystem.cl.Windows
{
    public partial class AddExistingAssignment : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; }

        Officer ofc;
        IEnumerable<Assignment> assignments;

        public AddExistingAssignment(Officer ofc)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            this.ofc = ofc;

            ThreadPool.QueueUserWorkItem(async x =>
            {
                await Resync(true);
                Invoke((MethodInvoker)delegate
                {
                    UpdateCurrentInformation();
                });
            });
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

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            Tuple<NetRequestResult, IEnumerable<Assignment>> result = await handle.TryTriggerNetFunction<IEnumerable<Assignment>>("GetAssignments");
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            if (result.Item2 != null)
            {
                while (!this.IsHandleCreated)
                    await Task.Delay(50);
                Invoke((MethodInvoker)delegate
                {
                    assignments = result.Item2;
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

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            await handle.TriggerNetEvent("AddOfficerAssignment", assignment.Id, ofc.Id);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            Close();
        }
    }
}
