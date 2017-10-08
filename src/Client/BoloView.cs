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
using DispatchSystem.Common.NetCode;

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

        public void UpdateCurrentInformation()
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

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            Tuple<NetRequestResult, List<Bolo>> result = await handle.TryTriggerNetFunction<List<Bolo>>("GetBolos");

            if (result.Item2 != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    bolos = result.Item2;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("FATAL: Invalid", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            usrSocket.Disconnect(true);
            IsCurrentlySyncing = false;
        }

        private void OnReyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
