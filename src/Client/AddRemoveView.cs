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
    public partial class AddRemoveView : MaterialForm
    {
        public enum Type
        {
            Add,
            Remove
        }

        public Type FormType { get; }
        Socket usrSocket;

        public AddRemoveView(Type formType)
        {
            InitializeComponent();

            FormType = formType;

            if (formType == Type.Add)
            {
                this.Text = "Add BOLO";
                addRemoveBtn.Text = "Add Bolo";
                line1.Hint = "BOLO Reason";
            }
            else if (formType == Type.Remove)
            {
                this.Text = "Remove BOLO";
                addRemoveBtn.Text = "Remove Bolo";
                line1.Hint = "BOLO Index";
                line2.Visible = false;
            }
        }

        private void OnBtnClick(object sender, EventArgs e)
        {
            usrSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (FormType == Type.Add)
            {
                if (!(string.IsNullOrWhiteSpace(line1.Text) || string.IsNullOrWhiteSpace(line2.Text)))
                    usrSocket.Send(new byte[] { 5 }.Concat(Encoding.UTF8.GetBytes($"{line2.Text}|{line1.Text}^")).ToArray());
                line1.ResetText();
                line2.ResetText();
            }
            if (FormType == Type.Remove)
            {
                if (!int.TryParse(line1.Text, out int result)) { MessageBox.Show("The index of the BOLO must be a valid number"); return; }
                result--;

                usrSocket.Send(new byte[] { 4 }.Concat(Encoding.UTF8.GetBytes($"{result}^")).ToArray());
                line1.ResetText();
            }

            this.Hide();
            usrSocket.Disconnect(false);
        }
    }
}
