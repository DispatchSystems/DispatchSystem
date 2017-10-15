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

        public OfficerView(Officer data)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");

            InitializeComponent();

            ofc = data;
            UpdateCurrentInformation();
        }

        public void UpdateCurrentInformation()
        {
            this.nameView.Text = ofc.Callsign;

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

            Tuple<NetRequestResult, Officer> result = await handle.TryTriggerNetFunction<Officer>("GetOfficer", ofc.SourceIP);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            if (result.Item2 != null)
            {
                if (result.Item2.SourceIP != string.Empty && result.Item2.Callsign != string.Empty)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        ofc = result.Item2;
                        UpdateCurrentInformation();
                    });
                }
                else
                    MessageBox.Show("The officer profile has been deleted!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void OnResyncClick(object sender, EventArgs e)
        {
#if DEBUG
            await Resync(true);
#else
            await Resync(false);
#endif
        }
    }
}
