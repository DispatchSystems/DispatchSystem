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

        public Timer Timer { get; }

        public BoloView(string data)
        {
            InitializeComponent();

            Timer = new Timer
            {
                Interval = 15000
            };
            Timer.Tick += OnTick;
            Timer.Start();

            ParseInformation(data);
            UpdateInformation();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (this.IsHandleCreated && this.Visible)
            {
                new Task(async () => await Resync()).Start();
            }
            else
                Timer.Dispose();
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
            Socket usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            IsCurrentlySyncing = true;

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
    }
}
