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

namespace DispatchSystem.cl.Windows
{
    public partial class BoloView : MaterialForm, ISyncable
    {
        StorageManager<Bolo> bolos = new StorageManager<Bolo>();

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public BoloView(StorageManager<Bolo> data)
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
                    ListViewItem lvi = new ListViewItem(bolos[i].Player);
                    lvi.SubItems.Add(bolos[i].Reason);
                    lvis.Add(lvi);
                }

                bolosView.Items.AddRange(lvis.ToArray());
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
            Tuple<NetRequestResult, StorageManager<Bolo>> result = await handle.TryTriggerNetFunction<StorageManager<Bolo>>("GetBolos");
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

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

            IsCurrentlySyncing = false;
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async delegate { await Resync(false); }).Start();

        private void OnAddBoloClick(object sender, EventArgs e)
        {
            new Task(delegate
            {
                AddRemoveView window = null;

                Invoke((MethodInvoker)delegate
                {
                    (window = new AddRemoveView(AddRemoveView.Type.AddBolo)).Show();
                });
                window.FormClosed += async delegate
                {
                    await Resync(true);
                };
            }).Start();
        }

        private async void OnRemoveSelectedClick(object sender, EventArgs e)
        {
            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            if (bolosView.SelectedItems.Count > 0)
            {
                await handle.TryTriggerNetEvent("RemoveBolo", 0);
            }
            else
                MessageBox.Show("You don't have any selected items!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            await Resync(true);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (bolosView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    rightClickMenu.Show(Cursor.Position);
                }
            }
        }
    }
}
