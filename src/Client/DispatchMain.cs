using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

using MaterialSkin;
using MaterialSkin.Controls;

namespace Client
{
    public partial class DispatchMain : MaterialForm
    {
        Socket usrSocket;

        public DispatchMain()
        {
            InitializeComponent();

            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            SkinManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue400, Accent.Blue700, TextShade.WHITE);
        }

        private void OnViewCivClick(object sender, EventArgs e)
        {
            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(IPAddress.Parse(Config.IP), Config.Port); }
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (!(string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text)))
            {
                usrSocket.Send(new byte[] { 1 }.Concat(Encoding.UTF8.GetBytes(string.Join("|", new string[] { firstName.Text.Trim(), $"{lastName.Text.Trim()}!" }))).ToArray());
                byte[] incoming = new byte[1001];
                usrSocket.Receive(incoming);
                byte tag = incoming[0];
                incoming = incoming.Skip(1).ToArray();
                
                if (tag == 1)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                        CivView civView = new CivView(data);
                        civView.Show();
                    });
                }
                else if (tag == 2)
                {
                    MessageBox.Show("That is an Invalid name!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                firstName.ResetText();
                lastName.ResetText();
            }

            usrSocket.Disconnect(false);
        }

        private void OnViewCivVehClick(object sender, EventArgs e)
        {
            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(IPAddress.Parse(Config.IP), Config.Port); }
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (!string.IsNullOrWhiteSpace(plate.Text))
            {
                usrSocket.Send(new byte[] { 2 }.Concat(Encoding.UTF8.GetBytes($"{plate.Text}!")).ToArray());
                byte[] incoming = new byte[1001];
                usrSocket.Receive(incoming);
                byte tag = incoming[0];
                incoming = incoming.Skip(1).ToArray();

                if (tag == 3)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                        CivVehView civVehView = new CivVehView(data);
                        civVehView.Show();
                    });
                }
                else if (tag == 4)
                {
                    MessageBox.Show("That is an Invalid plate!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                plate.ResetText();
            }

            usrSocket.Disconnect(false);
        }
    }
}
