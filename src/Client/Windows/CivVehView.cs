using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (((DateTime.Now - LastSyncTime).Seconds < 5 || IsCurrentlySyncing) && !skipTime)
            {
                MessageBox.Show($"You must wait 5 seconds before the last sync time \nSeconds to wait: {5 - (DateTime.Now - LastSyncTime).Seconds}", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LastSyncTime = DateTime.Now;
            IsCurrentlySyncing = true;

            if (string.IsNullOrWhiteSpace(plateView.Text))
                return;

            CivilianVeh result = await Program.Client.Peer.RemoteCallbacks.Functions["GetCivilianVeh"]
                .Invoke<CivilianVeh>(data.Plate);
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    data = result;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("That plate doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
