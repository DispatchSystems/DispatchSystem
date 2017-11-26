using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DumpUnloader
{
    static class Program
    {
        private static DumpParser parser;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            parser = new DumpParser("dispatchsystem.dmp");

            if (parser.Result != DumpResult.Successful)
            {
                MessageBox.Show("Exiting because of unsuccessful dump read", "DumpUnloader", MessageBoxButtons.OK,
                    MessageBoxIcon.None);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DumpDialogue(parser.DumpInformation));
        }
    }
}
