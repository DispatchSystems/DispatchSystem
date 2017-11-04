using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class BoloView : MaterialForm, ISyncable
    {
        private StorageManager<Bolo> bolos;

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public BoloView(StorageManager<Bolo> data)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
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

                foreach (Bolo t in bolos)
                {
                    ListViewItem lvi = new ListViewItem(t.Player);
                    lvi.SubItems.Add(t.Reason);
                    lvis.Add(lvi);
                }

                bolosView.Items.AddRange(lvis.ToArray());
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

            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Tuple<NetRequestResult, StorageManager<Bolo>> result = await handle.TryTriggerNetFunction<StorageManager<Bolo>>("GetBolos");
                handle.Disconnect();

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
            }

            IsCurrentlySyncing = false;
        }

        private async void OnResyncClick(object sender, EventArgs e) =>
#if DEBUG
            await Resync(true);
#else
            await Resync(false);
#endif

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
            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (bolosView.SelectedItems.Count > 0)
                {
                    await handle.TryTriggerNetEvent("RemoveBolo", 0);
                }
                else
                    MessageBox.Show("You don't have any selected items!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                handle.Disconnect();
            }

            await Resync(true);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (bolosView.FocusedItem.Bounds.Contains(e.Location))
                {
                    rightClickMenu.Show(Cursor.Position);
                }
            }
        }
    }
}
