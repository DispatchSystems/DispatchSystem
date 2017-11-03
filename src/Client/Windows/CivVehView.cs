using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class CivVehView : MaterialForm, ISyncable
    {
        private CivilianVeh data;

        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        public CivVehView(CivilianVeh civVehData)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            data = civVehData;
            UpdateCurrentInformation();
        }

        public void UpdateCurrentInformation()
        {
            plateView.Text = data.Plate;
            firstNameView.Text = data.Owner.First;
            lastNameView.Text = data.Owner.Last;
            stolenView.Checked = data.StolenStatus;
            registrationView.Checked = data.Registered;
            insuranceView.Checked = data.Insured;
        }

        public async Task Resync(bool skipTime)
        {
            if (((DateTime.Now - LastSyncTime).Seconds < 15 || IsCurrentlySyncing) && !skipTime)
            {
                MessageBox.Show($"You must wait 15 seconds before the last sync time \nSeconds to wait: {15 - (DateTime.Now - LastSyncTime).Seconds}", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LastSyncTime = DateTime.Now;
            IsCurrentlySyncing = true;

            if (string.IsNullOrWhiteSpace(plateView.Text))
                return;

            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                Tuple<NetRequestResult, CivilianVeh> result = await handle.TryTriggerNetFunction<CivilianVeh>("GetCivilianVeh", data.Plate);
                handle.Disconnect();

                if (result.Item2 != null)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        data = result.Item2;
                        UpdateCurrentInformation();
                    });
                }
                else
                    MessageBox.Show("That plate doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            IsCurrentlySyncing = false;
        }

        private async void OnResyncClick(object sender, EventArgs e) =>
#if DEBUG
            await Resync(true);
#else
            await Resync(false);
#endif
    }
}
