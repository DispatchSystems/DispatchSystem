using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DispatchSystem.cl.Windows;

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

            Application.Run(mainWindow);
        }
    }
}
