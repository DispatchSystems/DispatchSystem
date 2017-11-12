using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CloNET;
using DispatchSystem.Common.DataHolders.Storage;
using MaterialSkin.Controls;

namespace DispatchSystem.cl.Windows.Emergency
{
    public partial class Accept911 : MaterialForm
    {
        #region DLL Import
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        private readonly Civilian civ;
        private readonly EmergencyCall call;

        public Accept911(Civilian requester, EmergencyCall call)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            civ = requester;
            this.call = call;

            information.Text = $"Incoming call from {requester.First} {requester.Last} for an UNKNOWN reason...";
            SetForegroundWindow(Handle);
        }

        private async void OnAcceptClick(object sender, EventArgs e)
        {
            object item = await Program.Client.Peer.RemoteCallbacks.Functions["Accept911"].Invoke<object>(call.Id);
            if (item == null)
            {
                MessageBox.Show("Invalid request", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((bool) item)
            {
                new Message911(civ, call).Show();
            }
            else
                MessageBox.Show("It seems that another dispatcher is taking care of that, or the caller hung up.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            Close();
        }

        private void OnDenyClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
