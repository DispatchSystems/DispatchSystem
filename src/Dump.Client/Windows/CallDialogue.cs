using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using Dispatch.Common.DataHolders.Storage;

namespace DispatchSystem.Dump.Client.Windows
{
    public partial class CallDialogue : Form
    {
        public CallDialogue(IEnumerable<EmergencyCall> calls)
        {
            InitializeComponent();

            if (calls == null)
            {
                MessageBox.Show("ERROR: Call list is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var call in calls)
            {
                ListViewItem item = new ListViewItem(call?.Id.ToString() ?? "NULL");
                item.SubItems.Add(call?.SourceIP ?? "NULL");
                item.SubItems.Add(call?.PlayerName ?? "NULL");
                item.SubItems.Add(call?.Accepted.ToString() ?? "NULL");
                item.SubItems.Add(call?.Creation.ToString(CultureInfo.InvariantCulture) ?? "NULL");

                listView1.Items.Add(item);
            }
        }
    }
}
