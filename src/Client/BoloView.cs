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

namespace Client
{
    public partial class BoloView : MaterialForm, ISyncable
    {
        Dictionary<int, (string, string)> bolos = new Dictionary<int, (string, string)>();

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public BoloView(string data)
        {
            InitializeComponent();

            ParseInformation(data);
            UpdateInformation();
        }

        void UpdateInformation()
        {
            List<ListViewItem> lvis = new List<ListViewItem>();

            if (bolosView.Items.Count != bolos.Count)
            {
                bolosView.Items.Clear();

                foreach (var item in bolos)
                {
                    ListViewItem lvi = new ListViewItem(item.Key.ToString());
                    lvi.SubItems.Add(item.Value.Item1.ToString());
                    lvi.SubItems.Add(item.Value.Item2.ToString());
                    lvis.Add(lvi);
                }

                bolosView.Items.AddRange(lvis.ToArray());
            }
        }

        void ParseInformation(string data)
        {
            string[] main = data.Split('|');

            if (main[0] == "?")
                return;

            bolos.Clear();
            foreach (var item in main)
            {
                string[] other = item.Split('\\');

                int index = int.Parse(other[0]) + 1;
                string[] other2 = other[1].Split(':');
                string name = other2[0];
                string desc = other2[1];

                bolos.Add(index, (name, desc));
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
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            usrSocket.Send(new byte[] { 3 });
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 5)
            {
                Invoke((MethodInvoker)delegate
                {
                    string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                    ParseInformation(data);
                    UpdateInformation();
                });
            }

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnReyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
