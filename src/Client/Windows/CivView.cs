using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin.Controls;

using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

namespace DispatchSystem.cl.Windows
{
    public partial class CivView : MaterialForm, ISyncable
    {
        public bool IsCurrentlySyncing { get; private set; }
        public DateTime LastSyncTime { get; private set; } = DateTime.Now;

        private Civilian data;

        public CivView(Civilian civData)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            data = civData;

            UpdateCurrentInformation();
        }

        public void UpdateCurrentInformation()
        {
            firstNameView.ResetText();
            lastNameView.ResetText();
            citationsView.ResetText();
            if (notesView.Items.Count != data.Notes.Count())
                notesView.Items.Clear();
            if (ticketsView.Items.Count != data.Tickets.Count())
                ticketsView.Items.Clear();

            firstNameView.Text = data.First;
            lastNameView.Text = data.Last;
            wantedView.Checked = data.WarrantStatus;
            citationsView.Text = data.CitationCount.ToString();

            if (data.Notes.Count != 0 && notesView.Items.Count != data.Notes.Count)
            {
                data.Notes.ToList().ForEach(x => notesView.Items.Add(x));
            }

            if (data.Tickets.Count != 0 && ticketsView.Items.Count != data.Tickets.Count())
            {
                foreach (var item in data.Tickets)
                {
                    ListViewItem li = new ListViewItem($"${item.Amount}");
                    li.SubItems.Add(item.Reason);
                    ticketsView.Items.Add(li);
                }
            }
        }

        private void OnAddNoteClick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                AddRemoveView view;
                (view = new AddRemoveView(AddRemoveView.Type.AddNote, data.Id)).Show();
                view.FormClosed += async delegate
                {
                    await Resync(true);
                };
            });
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

            if (string.IsNullOrWhiteSpace(firstNameView.Text) || string.IsNullOrWhiteSpace(lastNameView.Text))
                return;

            var result = await Program.Client.Peer.RemoteCallbacks.Functions["GetCivilian"]
                .Invoke<Civilian>(data.First, data.Last);
            if (result != null)
            {
                Invoke((MethodInvoker)delegate
                {
                    data = result;
                    UpdateCurrentInformation();
                });
            }
            else
                MessageBox.Show("That name doesn't exist in the system!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            IsCurrentlySyncing = false;
        }

        private async void OnResyncClick(object sender, EventArgs e) =>
#if DEBUG
            await Resync(true);
#else
            await Resync(true);
#endif
    }
}
