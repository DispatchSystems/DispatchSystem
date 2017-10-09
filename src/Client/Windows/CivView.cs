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
    public partial class CivView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        Civilian data;

        public CivView(Civilian civData)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();


            this.data = civData;

            UpdateCurrentInformation();
        }

        public void UpdateCurrentInformation()
        {
            firstNameView.ResetText();
            lastNameView.ResetText();
            citationsView.ResetText();
            if (notesView.Items.Count != data.Notes.Count())
                notesView.Items.Clear();
            if (ticketsView.Items.Count != data.Tickets.Count())
                ticketsView.Items.Clear();

            firstNameView.Text = data.First;
            lastNameView.Text = data.Last;
            wantedView.Checked = data.WarrantStatus;
            citationsView.Text = data.CitationCount.ToString();

            if (data.Notes.Count != 0 && notesView.Items.Count != data.Notes.Count)
            {
                data.Notes.ToList().ForEach(x => notesView.Items.Add(x));
            }

            if (data.Tickets.Count != 0 && ticketsView.Items.Count != data.Tickets.Count())
            {
                foreach (var item in data.Tickets)
                {
                    ListViewItem li = new ListViewItem($"${item.Item2.ToString()}");
                    li.SubItems.Add(item.Item1);
                    ticketsView.Items.Add(li);
                }
            }
        }

        private void OnAddNoteClick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                new AddRemoveView(AddRemoveView.Type.AddNote, data.First, data.Last).Show();
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

            if (string.IsNullOrWhiteSpace(firstNameView.Text) || string.IsNullOrWhiteSpace(lastNameView.Text))
                return;

            Socket usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);
            Tuple<NetRequestResult, Civilian> result = await handle.TryTriggerNetFunction<Civilian>("GetCivilian", data.First, data.Last);
            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();

            if (result.Item2 != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    data = result.Item2;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("That name doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            IsCurrentlySyncing = false;
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async () => await Resync(false)).Start();
    }
}
