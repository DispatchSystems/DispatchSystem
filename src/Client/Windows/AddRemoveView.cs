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
using DispatchSystem.Common.DataHolders.Requesting;
using DispatchSystem.Common.NetCode;

namespace DispatchSystem.cl.Windows
{
    public partial class AddRemoveView : MaterialForm
    {
        public enum Type
        {
            AddBolo,
            RemoveBolo,
            AddNote
        }

        public Type FormType { get; }
        public bool OperationDone { get; private set; } = false;
        private object[] arguments;
        Socket usrSocket;

        public AddRemoveView(Type formType, params object[] args)
        {
            this.Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            FormType = formType;
            arguments = args;

            if (formType == Type.AddBolo)
            {
                this.Text = "Add BOLO";
                addRemoveBtn.Text = "Add Bolo";
                line1.Hint = "BOLO Reason";
            }
            else if (formType == Type.RemoveBolo)
            {
                this.Text = "Remove BOLO";
                addRemoveBtn.Text = "Remove Bolo";
                line1.Hint = "BOLO Index";
                line2.Visible = false;
            }
            else if (formType == Type.AddNote)
            {
                this.Text = "Add Note";
                addRemoveBtn.Text = "Add Note";
                line1.Hint = "Note";
                line2.Visible = false;
            }
        }

        private async void OnBtnClick(object sender, EventArgs e)
        {
            usrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { usrSocket.Connect(Config.IP, Config.Port); }
            catch (SocketException) { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            NetRequestHandler handle = new NetRequestHandler(usrSocket);

            switch (FormType)
            {
                case Type.AddBolo:
                    {
                        if (!(string.IsNullOrWhiteSpace(line1.Text) || string.IsNullOrWhiteSpace(line2.Text)))
                            await handle.TryTriggerNetEvent("AddBolo", line2.Text, line1.Text);
                        line1.ResetText();
                        line2.ResetText();
                        break;
                    }
                case Type.RemoveBolo:
                    {
                        if (!int.TryParse(line1.Text, out int result)) { MessageBox.Show("The index of the BOLO must be a valid number"); return; }
                        await handle.TryTriggerNetEvent("RemoveBolo", result);
                        line1.ResetText();
                        break;
                    }
                case Type.AddNote:
                    {
                        if (!string.IsNullOrEmpty(line1.Text))
                            await handle.TryTriggerNetEvent("AddNote", arguments[0], arguments[1], line1.Text);
                        line1.ResetText();
                        break;
                    }
            }

            this.Hide();
            this.OperationDone = true;

            usrSocket.Shutdown(SocketShutdown.Both);
            usrSocket.Close();
        }
    }
}
