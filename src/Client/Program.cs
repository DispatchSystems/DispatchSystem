using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DispatchSystem.cl.Windows;
using System.IO;

namespace DispatchSystem.cl
{
    static class Program
    {
        internal static DispatchMain mainWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config.Create("settings.ini");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainWindow = new DispatchMain();
            try
            {
                Application.Run(mainWindow);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "An exception in BlockBa5her's code caused it to CRASH!\n" +
                    "The error has been put into a file called 'err.dmp' in your directory\n" +
                    "Either send it to BlockBa5her or contact your community owner!"
                    );
                using (var stream = new FileStream("err.dmp", FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(e.ToString());
                    stream.Write(buffer, 0, buffer.Length);
                }
                Environment.Exit(-1);
            }
        }
    }
}
