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

namespace Client
{
    public partial class CivView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        Civilian data;

        public CivView(Civilian civData)
        {
            InitializeComponent();
            this.data = civData;

            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
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

            usrSocket.Send(new byte[] { 1 }.Concat(new StorableValue<Tuple<string, string>>(new Tuple<string, string>(data.First, data.Last)).Bytes).ToArray());
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    StorableValue<Civilian> item = new StorableValue<Civilian>(incoming);
                    data = item.Value;
                    UpdateCurrentInfromation();
                });
            }
            else if (tag == 2)
                MessageBox.Show("That is an Invalid name!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
