using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

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

        public void OnViewCivClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text))
                return;

            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            usrSocket.Send(new byte[] { 1 }.Concat(Encoding.UTF8.GetBytes(string.Join("|", new string[] { firstName.Text.Trim(), $"{lastName.Text.Trim()}!" }))).ToArray());
            byte[] incoming = new byte[5001];
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
                MessageBox.Show("That is an Invalid name!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            firstName.ResetText();
            lastName.ResetText();

            usrSocket.Disconnect(false);
        }

        private void OnViewCivVehClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(plate.Text))
                return;

            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            usrSocket.Send(new byte[] { 2 }.Concat(Encoding.UTF8.GetBytes($"{plate.Text}!")).ToArray());
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                    CivVehView civVehView = new CivVehView(data);
                    civVehView.Show();
                });
            }
            else if (tag == 2)
                MessageBox.Show("That is an Invalid plate!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            plate.ResetText();

            usrSocket.Disconnect(false);
        }

        private void OnViewBolosClick(object sender, EventArgs e)
        {
            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            usrSocket.Send(new byte[] { 3 });
            byte[] incoming = new byte[5001];
            usrSocket.Receive(incoming);
            byte tag = incoming[0];
            incoming = incoming.Skip(1).ToArray();

            if (tag == 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    string data = Encoding.UTF8.GetString(incoming).Split('^')[0];

                    BoloView boloView = new BoloView(data);
                    boloView.Show();
                });
            }
            if (tag == 3)
                MessageBox.Show("Socket not accepted by the server\nChange the permissions on the server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            usrSocket.Disconnect(false);
        }

        private void OnRemoveBoloClick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                AddRemoveView boloView = new AddRemoveView(AddRemoveView.Type.RemoveBolo);
                boloView.Show();
            });
        }

        private void OnAddBoloClick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                AddRemoveView boloView = new AddRemoveView(AddRemoveView.Type.AddBolo);
                boloView.Show();
            });
        }
    }
}
