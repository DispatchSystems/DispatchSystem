using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloNET;
using CloNET.Callbacks;
using DispatchSystem.Common.DataHolders.Storage;
using MaterialSkin.Controls;

namespace DispatchSystem.cl.Windows.Emergency
{
    public partial class Message911 : MaterialForm
    {
        private readonly Civilian civ;
        private readonly EmergencyCall call;

        public Message911(Civilian civ, EmergencyCall call)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            this.civ = civ;
            this.call = call;

            Text += $"{civ.First} {civ.Last}";

            Program.Client.Events.Add(call.Id.ToString(), new NetEvent(Msg911));
            Program.Client.Events.Add("end" + call.Id, new NetEvent(End911));

            Closed += async delegate
            {
                await Program.Client.TryTriggerNetEvent("911End", call.Id);

                Program.Client.Events.Remove("end" + call.Id);
                Program.Client.Events.Remove(call.Id.ToString());
            };
        }

        private async Task Msg911(ConnectedPeer peer, object[] data)
        {
            await Task.FromResult(0);

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add($"{civ.First} {civ.Last}");
            item.SubItems.Add((string)data[0]);

            Invoke((MethodInvoker)delegate
            {
                msgs.Items.Add(item);
            });
        }

        private async Task End911(ConnectedPeer peer, object[] data)
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
            if (!string.IsNullOrWhiteSpace(msgBox.Text))
            {
                await Program.Client.TryTriggerNetEvent("911Msg", call.Id, msgBox.Text);

                ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
                item.SubItems.Add("You");
                item.SubItems.Add(msgBox.Text);

                Invoke((MethodInvoker) delegate
                {
                    msgs.Items.Add(item);
                    msgBox.Clear();
                });
            }
        }
        private void SendMsg(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendMsg(sender, (EventArgs)e);
        }
    }
}
