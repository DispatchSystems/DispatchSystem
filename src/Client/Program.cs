using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

using DispatchSystem.cl.Windows;

using CloNET;

namespace DispatchSystem.cl
{
    static class Program
    {
        internal static Client Client;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Config.Create("settings.ini");

            using (Client = new Client())
            {
                try { Client.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection Refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                new Thread((ThreadStart)delegate
                    {
                        while (true)
                        {
                            Thread.Sleep(100);
                            if (Client.Connected) continue;
                            MessageBox.Show("The connection to the server suddenly dropped!", "DispatchSystem",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(-1);
                        }
                    })
                    { Name = "ConnectionDetection" }.Start();
                Application.Run(new DispatchMain());

                Client.Disconnect();
            }
            Environment.Exit(0);
        }
    }
}
