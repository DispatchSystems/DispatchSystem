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

using System.Net;
using System.Net.Sockets;

using DispatchSystem.Common.DataHolders;
using DispatchSystem.Common.DataHolders.Storage;

namespace Client
{
    public partial class BoloView : MaterialForm, ISyncable
    {
        List<Bolo> bolos = new List<Bolo>();

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public BoloView(List<Bolo> data)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();
            bolos = data;

            UpdateCurrentInformation();
        }

        void UpdateCurrentInformation()
        {
            List<ListViewItem> lvis = new List<ListViewItem>();

            if (bolosView.Items.Count != bolos.Count)
            {
                bolosView.Items.Clear();

                for (int i = 0; i < bolos.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem(i.ToString());
                    lvi.SubItems.Add(bolos[i].Player);
                    lvi.SubItems.Add(bolos[i].Reason);
                    lvis.Add(lvi);
                }

                bolosView.Items.AddRange(lvis.ToArray());
            }
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

            usrSocket.Send(new byte[] { 3 });
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    bolos = new StorableValue<List<Bolo>>(incoming).Value;
                    UpdateCurrentInformation();
                });
            }
            if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnReyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
