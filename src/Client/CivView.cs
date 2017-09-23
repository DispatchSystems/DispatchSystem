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
    public partial class CivView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        string data;

        string firstName;
        string lastName;
        bool wanted;
        int citations;
        string[] notes;
        (float, string)[] tickets;

        public CivView(string civData)
        {
            InitializeComponent();
            this.data = civData;

            ParseCivilian(civData);
            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
        {
            firstNameView.ResetText();
            lastNameView.ResetText();
            citationsView.ResetText();
            if (notesView.Items.Count != notes.Count())
                notesView.Items.Clear();
            if (ticketsView.Items.Count != tickets.Count())
                ticketsView.Items.Clear();

            firstNameView.Text = firstName;
            lastNameView.Text = lastName;
            wantedView.Checked = wanted;
            citationsView.Text = citations.ToString();

            if (notes[0] == "?")
                notes = new List<string>().ToArray();

            if (notes.Count() != 0 && notesView.Items.Count != notes.Count())
            {
                this.notes.ToList().ForEach(x => notesView.Items.Add(x));
            }

            if (tickets.Count() != 0 && ticketsView.Items.Count != tickets.Count())
            {
                foreach (var item in tickets)
                {
                    ListViewItem li = new ListViewItem($"${item.Item1.ToString()}");
                    li.SubItems.Add(item.Item2);
                    ticketsView.Items.Add(li);
                }
            }
        }

        private void ParseCivilian(string data)
        {
            string[] main = data.Split('|');
            string[] name = main[0].Split(',');
            bool wanted = bool.Parse(main[1]);
            int citations = int.Parse(main[2]);
            string[] notes = main[3].Split('\\');
            string[] ticketsMain = main[4].Split('\\');
            List<(float, string)> tickets = new List<(float, string)>();
            if (ticketsMain[0] != "?")
                foreach (var item in ticketsMain)
                {
                    string[] main2 = item.Split('!');
                    float amount = float.Parse(main2[0]);
                    string reason = main2[1];
                    tickets.Add((amount, reason));
                }
            this.tickets = tickets.ToArray();

            this.firstName = name[0];
            this.lastName = name[1];
            this.wanted = wanted;
            this.citations = citations;
            this.notes = notes;
        }

        private void OnAddNoteClick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                new AddRemoveView(AddRemoveView.Type.AddNote, firstName, lastName).Show();
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
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); IsCurrentlySyncing = false; return; }


            if (!(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)))
            {
                usrSocket.Send(new byte[] { 1 }.Concat(Encoding.UTF8.GetBytes(string.Join("|", new string[] { firstName.Trim(), $"{lastName.Trim()}!" }))).ToArray());
                byte[] incoming = new byte[5001];
                usrSocket.Receive(incoming);
                byte tag = incoming[0];
                incoming = incoming.Skip(1).ToArray();

                if (tag == 1)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        this.data = Encoding.UTF8.GetString(incoming).Split('^')[0];
                        ParseCivilian(data);
                        UpdateCurrentInfromation();
                    });
                }
                else if (tag == 2)
                {
                    MessageBox.Show("That is an Invalid name!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
