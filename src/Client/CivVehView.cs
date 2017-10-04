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

namespace Client
{
    public partial class CivVehView : MaterialForm, ISyncable
    {
        string plate;
        string firstName;
        string lastName;
        bool stolen;
        bool registered;
        bool insured;

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public CivVehView(string civVehData)
        {
            InitializeComponent();

            ParseCivilian(civVehData);
            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
        {
            this.plateView.Text = plate;
            this.firstNameView.Text = firstName;
            this.lastNameView.Text = lastName;
            this.stolenView.Checked = stolen;
            this.registrationView.Checked = registered;
            this.insuranceView.Checked = insured;
        }

        private void ParseCivilian(string data)
        {
            string[] main = data.Split('|');
            string plate = main[0].ToUpper();
            string[] name = main[1].Split(',');
            bool stolen = bool.Parse(main[2]);
            bool registered = bool.Parse(main[3]);
            bool insured = bool.Parse(main[4]);

            this.plate = plate;
            this.firstName = name[0];
            this.lastName = name[1];
            this.stolen = stolen;
            this.registered = registered;
            this.insured = insured;

            if (!this.registered)
            {
                firstName = "None";
                lastName = "None";
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
            catch (SocketException) { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); IsCurrentlySyncing = false; return; }

            if (!string.IsNullOrWhiteSpace(plate))
            {
                usrSocket.Send(new byte[] { 2 }.Concat(Encoding.UTF8.GetBytes($"{plate}!")).ToArray());
                byte[] incoming = new byte[5001];
                usrSocket.Receive(incoming);
                byte tag = incoming[0];
                incoming = incoming.Skip(1).ToArray();

                if (tag == 3)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                        ParseCivilian(data);
                        UpdateCurrentInfromation();
                    });
                }
                else if (tag == 4)
                {
                    MessageBox.Show("That is an Invalid plate!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            usrSocket.Disconnect(false);
            IsCurrentlySyncing = false;
            await this.Delay(0);
        }

        private void OnResyncClick(object sender, EventArgs e) => new Task(async () => await Resync()).Start();
    }
}
