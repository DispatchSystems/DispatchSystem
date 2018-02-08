using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DispatchSystem.Dump.Client.Windows
{
    public partial class PermissionsDialogue : Form
    {
        public PermissionsDialogue(List<string> settings)
        {
            InitializeComponent();

            if (settings == null)
            {
                MessageBox.Show("ERROR: Permissions is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings.Contains("everyone"))
            {
                dispatch.Items.Add("everyone");
            }
            else if (settings.Contains("none"))
            {
                dispatch.Items.Add("none");
            }
            else
            {
                dispatch.Items.AddRange(settings.Select(x => new ListViewItem(x)).ToArray());
            }
        }
    }
}
