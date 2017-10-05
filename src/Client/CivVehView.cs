using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

using System.Net.Sockets;
using System.Net;

using DispatchSystem.Common.DataHolders;

namespace Client
{
    public partial class CivVehView : MaterialForm, ISyncable
    {
        CivilianVeh data;

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public CivVehView(CivilianVeh civVehData)
        {
            InitializeComponent();
            this.data = civVehData;
            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
        {
            this.plateView.Text = data.Plate.ToUpper();
            this.firstNameView.Text = data.Owner.First;
            this.lastNameView.Text = data.Owner.Last;
            this.stolenView.Checked = data.StolenStatus;
            this.registrationView.Checked = data.Registered;
            this.insuranceView.Checked = data.Insured;
        }

        public async Task Resync()
        {
            if ((DateTime.Now - LastSyncTime).Seconds < 15 || IsCurrentlySyncing)
            {
                MessageBox.Show($"You must wait 15 seconds before the last sync time \nSeconds to wait: {15 - (DateTime.Now - LastSyncTime).Seconds}", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LastSyncTime = DateTime.Now;
            IsCurrentlySyncing = true;

            Socket usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            usrSocket.Send(new byte[] { 2 }.Concat(new StorableValue<Tuple<string>>(new Tuple<string>(data.Plate)).Bytes).ToArray());
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    StorableValue<CivilianVeh> item = new StorableValue<CivilianVeh>(incoming);
                    data = item.Value;
                    UpdateCurrentInfromation();
                });
            }
            else if (tag == 2)
                MessageBox.Show("That is an Invalid plate!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
