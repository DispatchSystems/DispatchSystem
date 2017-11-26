using System;
using System.Drawing;
using System.Windows.Forms;

using CloNET;

using MaterialSkin.Controls;

namespace DispatchSystem.cl.Windows
{
    public partial class AddRemoveView : MaterialForm
    {
        public enum Type
        {
            AddBolo,
            RemoveBolo,
            AddNote,
            AddAssignment
        }

        public Type FormType { get; }
        public Guid LastGuid { get; protected set; }
        public bool OperationDone { get; private set; }
        private readonly object[] arguments;

        public AddRemoveView(Type formType, params object[] args)
        {
            Icon = Icon.ExtractAssociatedIcon("icon.ico");
            InitializeComponent();

            FormType = formType;
            arguments = args;

            switch (formType)
            {
                case Type.AddAssignment:
                    Text = "Add Assignment";
                    addRemoveBtn.Text = "Add";
                    line1.Hint = "Summary";
                    line2.Visible = false;
                    break;
                case Type.AddBolo:
                    Text = "Add BOLO";
                    addRemoveBtn.Text = "Add Bolo";
                    line1.Hint = "BOLO Reason";
                    break;
                case Type.RemoveBolo:
                    Text = "Remove BOLO";
                    addRemoveBtn.Text = "Remove Bolo";
                    line1.Hint = "BOLO Index";
                    line2.Visible = false;
                    break;
                case Type.AddNote:
                    Text = "Add Note";
                    addRemoveBtn.Text = "Add Note";
                    line1.Hint = "Note";
                    line2.Visible = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(formType), formType, null);
            }
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private async void OnBtnClick(object sender, EventArgs e)
        {
            switch (FormType)
            {
                case Type.AddBolo:
                {
                    if (!(string.IsNullOrWhiteSpace(line1.Text) || string.IsNullOrWhiteSpace(line2.Text)))
                        await Program.Client.Peer.RemoteCallbacks.Events["AddBolo"].Invoke(line2.Text, line1.Text);
                    line1.ResetText();
                    line2.ResetText();
                    break;
                }
                case Type.RemoveBolo:
                {
                    if (!int.TryParse(line1.Text, out int result)) { MessageBox.Show("The index of the BOLO must be a valid number"); return; }
                    await Program.Client.Peer.RemoteCallbacks.Events["RemoveBolo"].Invoke(result);
                    line1.ResetText();
                    break;
                }
                case Type.AddNote:
                {
                    if (!string.IsNullOrEmpty(line1.Text))
                        await Program.Client.Peer.RemoteCallbacks.Events["AddNote"]
                            .Invoke(arguments[0], line1.Text);
                    line1.ResetText();
                    break;
                }
                case Type.AddAssignment:
                {
                    if (!string.IsNullOrEmpty(line1.Text))
                    {
                        Guid result = await Program.Client.Peer.RemoteCallbacks.Functions["CreateAssignment"]
                            .Invoke<Guid>(line1.Text);
                        LastGuid = result;
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Close();
            OperationDone = true;
        }
    }
}
