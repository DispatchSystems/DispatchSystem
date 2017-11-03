using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;

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
            using (Client handle = new Client())
            {
                try { handle.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Failed\nPlease contact the owner of your Roleplay server!", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                switch (FormType)
                {
                    case Type.AddBolo:
                        {
                            if (!(string.IsNullOrWhiteSpace(line1.Text) || string.IsNullOrWhiteSpace(line2.Text)))
                                await handle.TryTriggerNetEvent("AddBolo", line2.Text, line1.Text);
                            line1.ResetText();
                            line2.ResetText();
                            break;
                        }
                    case Type.RemoveBolo:
                        {
                            if (!int.TryParse(line1.Text, out int result)) { MessageBox.Show("The index of the BOLO must be a valid number"); return; }
                            await handle.TryTriggerNetEvent("RemoveBolo", result);
                            line1.ResetText();
                            break;
                        }
                    case Type.AddNote:
                        {
                            if (!string.IsNullOrEmpty(line1.Text))
                                await handle.TryTriggerNetEvent("AddNote", arguments[0], arguments[1], line1.Text);
                            line1.ResetText();
                            break;
                        }
                    case Type.AddAssignment:
                        {
                            if (!string.IsNullOrEmpty(line1.Text))
                            {
                                Tuple<NetRequestResult, Guid> result = await handle.TryTriggerNetFunction<Guid>("CreateAssignment", line1.Text);
                                LastGuid = result.Item2;
                            }
                            break;
                        }
                }

                Close();
                OperationDone = true;

                handle.Disconnect();
            }
        }
    }
}
