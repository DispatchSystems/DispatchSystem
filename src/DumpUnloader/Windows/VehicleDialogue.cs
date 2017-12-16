using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using DispatchSystem.Common.DataHolders.Storage;

namespace DumpUnloader.Windows
{
    public partial class VehicleDialogue : Form
    {
        public VehicleDialogue(IEnumerable<CivilianVeh> vehicles)
        {
            InitializeComponent();

            if (vehicles == null)
            {
                MessageBox.Show("ERROR: Vehicles list is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var veh in vehicles)
            {
                ListViewItem item = new ListViewItem(veh?.Id.ToString() ?? "NULL");
                item.SubItems.Add(veh?.SourceIP ?? "NULL");
                item.SubItems.Add(veh?.Creation.ToString(CultureInfo.InvariantCulture) ?? "NULL");
                item.SubItems.Add(veh?.Plate ?? "NULL");
                item.SubItems.Add(veh?.Owner?.Id.ToString() ?? "NULL");
                item.SubItems.Add(veh?.StolenStatus.ToString() ?? "NULL");
                item.SubItems.Add(veh?.Registered.ToString() ?? "NULL");
                item.SubItems.Add(veh?.Insured.ToString() ?? "NULL");

                listView1.Items.Add(item);
            }
        }
    }
}
