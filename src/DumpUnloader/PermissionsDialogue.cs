using System.Windows.Forms;
using DispatchSystem.Common;

namespace DumpUnloader
{
    public partial class PermissionsDialogue : Form
    {
        public PermissionsDialogue(Permissions settings)
        {
            InitializeComponent();

            if (settings == null)
            {
                MessageBox.Show("ERROR: Permissions is null! This is an immediate issue please contact BlockBa5her",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (settings?.CivilianPermission)
            {
                case Permission.Everyone:
                    civs.Items.Add("Everyone");
                    break;
                case Permission.None:
                    civs.Items.Add("None");
                    break;
                case null:
                    civs.Items.Add("NULL");
                    break;
                default:
                    foreach (var item in settings.CivilianData)
                    {
                        civs.Items.Add(item?.ToString() ?? "NULL");
                    }
                    break;
            }

            switch (settings?.DispatchPermission)
            {
                case Permission.Everyone:
                    dispatch.Items.Add("Everyone");
                    break;
                case Permission.None:
                    dispatch.Items.Add("None");
                    break;
                case null:
                    dispatch.Items.Add("NULL");
                    break;
                default:
                    foreach (var item in settings.DispatchData)
                    {
                        dispatch.Items.Add(item?.ToString() ?? "NULL");
                    }
                    break;
            }

            switch (settings?.LeoPermission)
            {
                case Permission.Everyone:
                    leo.Items.Add("Everyone");
                    break;
                case Permission.None:
                    leo.Items.Add("None");
                    break;
                case null:
                    leo.Items.Add("NULL");
                    break;
                default:
                    foreach (var item in settings.LeoData)
                    {
                        leo.Items.Add(item?.ToString() ?? "NULL");
                    }
                    break;
            }
        }
    }
}
