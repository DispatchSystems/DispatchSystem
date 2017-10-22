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

using MaterialSkin;
using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.DataHolders;
using DispatchSystem.Common.NetCode;

namespace DispatchSystem.cl.Windows
{
    public partial class OfficerView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        Officer ofc;
        Assignment assignment = null;
        AddRemoveView addBtn = null;
        AddExistingAssignment addExisting = null;

        public OfficerView(Officer data)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");

            InitializeComponent();

            ofc = data;
            SetAssignment().Wait();
        }

        public void UpdateCurrentInformation()
        {
            this.nameView.Text = ofc.Callsign;
            this.clockedView.Text = ofc.Creation.ToLocalTime().ToString("HH:mm:ss");
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
            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            Tuple<NetRequestResult, Assignment> result = await handle.TryTriggerNetFunction<Assignment>("GetOfficerAssignment", ofc.Id);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            assignment = result.Item2;
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

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            Tuple<NetRequestResult, Officer> result = await handle.TryTriggerNetFunction<Officer>("GetOfficer", ofc.Id);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

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

            IsCurrentlySyncing = false;
        }

        private async void StatusClick(object sender, EventArgs e)
        {
            MaterialRadioButton _sender = (MaterialRadioButton)sender;

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            do
            {
                if (_sender == radioOnDuty)
                {
                    if (ofc.Status == OfficerStatus.OnDuty)
                    {
                        MessageBox.Show("Really? That officer is already on duty!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }

                    await handle.TryTriggerNetEvent("SetStatus", ofc, OfficerStatus.OnDuty);
                }
                else if (_sender == radioOffDuty)
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

            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();
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
                    Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try { usrSocket.Connect(Config.IP, Config.Port); }
                    catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    NetRequestHandler handle = new NetRequestHandler(usrSocket);

                    await handle.TriggerNetEvent("AddOfficerAssignment", addBtn.LastGuid, ofc.Id);

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

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            await handle.TryTriggerNetEvent("RemoveOfficerAssignment", ofc.Id);

            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

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
