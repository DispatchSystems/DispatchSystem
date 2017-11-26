using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DispatchSystem.Common.DataHolders.Storage;

namespace DumpUnloader
{
    public partial class OfficerDialogue : Form
    {
        public OfficerDialogue(IEnumerable<Officer> officers)
        {
            InitializeComponent();

            if (officers == null)
            {
                MessageBox.Show("ERROR: Officers list is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var officer in officers)
            {
                ListViewItem item = new ListViewItem(officer?.Id.ToString() ?? "NULL");
                item.SubItems.Add(officer?.SourceIP ?? "NULL");
                item.SubItems.Add(officer?.Creation.ToString(CultureInfo.InvariantCulture) ?? "NULL");
                item.SubItems.Add(officer?.Callsign ?? "NULL");
                item.SubItems.Add(officer?.Status.ToString() ?? "NULL");

                listView1.Items.Add(item);
            }
        }
    }
}
