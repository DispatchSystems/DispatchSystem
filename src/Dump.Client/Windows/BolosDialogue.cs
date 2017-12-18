using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using Dispatch.Common.DataHolders.Storage;

namespace DispatchSystem.Dump.Client.Windows
{
    public partial class BolosDialogue : Form
    {
        public BolosDialogue(IEnumerable<Bolo> bolos)
        {
            InitializeComponent();

            if (bolos == null)
            {
                MessageBox.Show("ERROR: BOLOs list is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var bolo in bolos)
            {
                ListViewItem item = new ListViewItem(bolo?.Id.ToString() ?? "NULL");
                item.SubItems.Add(bolo?.Creation.ToString(CultureInfo.InvariantCulture) ?? "NULL");
                item.SubItems.Add(bolo?.Player ?? "NULL");
                item.SubItems.Add(bolo?.Reason ?? "NULL");

                listView1.Items.Add(item);
            }
        }
    }
}
