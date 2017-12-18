using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloNET;
using CloNET.Callbacks;
using CloNET.LocalCallbacks;
using Dispatch.Common.DataHolders.Storage;
using MaterialSkin.Controls;

namespace DispatchSystem.Terminal.Windows.Emergency
{
    public partial class Message911 : MaterialForm
    {
        private readonly Civilian civ;
        private readonly EmergencyCall call;

        public Message911(Civilian civ, EmergencyCall call)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            SkinManager.AddFormToManage(this);

            this.civ = civ;
            this.call = call;

            Text += $"{civ.First} {civ.Last}";

            Program.Client.LocalCallbacks.Events.Add(call.Id.ToString(), new LocalEvent(new Func<ConnectedPeer, string, Task>(Msg911)));
            Program.Client.LocalCallbacks.Events.Add("end" + call.Id, new LocalEvent(new Func<ConnectedPeer, Task>(End911)));

            Closed += async delegate
            {
                await Program.Client.Peer.RemoteCallbacks.Events["911End"].Invoke(call.Id);

                Program.Client.LocalCallbacks.Events.Remove("end" + call.Id);
                Program.Client.LocalCallbacks.Events.Remove(call.Id.ToString());
            };
        }

        private async Task Msg911(ConnectedPeer peer, string incomingMsg)
        {
            await Task.FromResult(0);

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add($"{civ.First} {civ.Last}");
            item.SubItems.Add(incomingMsg);

            Invoke((MethodInvoker)delegate
            {
                msgs.Items.Add(item);
            });
        }

        private async Task End911(ConnectedPeer peer)
        {
            await Task.FromResult(0);

            MessageBox.Show("User has ended 911 call", "DispatchSystem", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            Invoke((MethodInvoker)Close);
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private async void SendMsg(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(msgBox.Text)) return;

            await Program.Client.Peer.RemoteCallbacks.Events["911Msg"].Invoke(call.Id, msgBox.Text);

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add("You");
            item.SubItems.Add(msgBox.Text);

            Invoke((MethodInvoker) delegate
            {
                msgs.Items.Add(item);
                msgBox.Clear();
            });
        }
        private void SendMsg(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            SendMsg(sender, (EventArgs)e);
        }
    }
}
