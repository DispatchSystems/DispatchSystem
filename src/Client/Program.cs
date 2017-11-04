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
        private static DispatchMain mainWindow;

        private const ushort RECONNECT_COUNT = 3;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Config.Create("settings.ini");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Client = new Client())
            {
                try { Client.Connect(Config.IP.ToString(), Config.Port); }
                catch (SocketException) { MessageBox.Show("Connection refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                new Thread((ThreadStart)delegate
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (Client.Connected) continue;
                        if (mainWindow == null) continue;

                        bool executing = true;
                        new Thread(() =>
                        {
                            mainWindow.Invoke((MethodInvoker) delegate
                            {
                                while (executing)
                                {
                                    Thread.Sleep(50);
                                }
                            });
                        }) {Name = "WindowFreezeThread"}.Start();

                        for (int i = 0; i < RECONNECT_COUNT; i++)
                        {
                            try
                            {
                                Client.Disconnect();
                                Client.Connect(Config.IP.ToString(), Config.Port);
                            }
                            catch (SocketException)
                            {
                            }

                            Thread.Sleep(100);
                            if (Client.Connected) break;
                        }

                        if (Client.Connected)
                        {
                            executing = false;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to connect to the server after {RECONNECT_COUNT} attempts", "DispatchSystem",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(-1);
                        }
                    }
                })
                { Name = "ConnectionDetection" }.Start();

                Application.Run(mainWindow = new DispatchMain());

                Client.Disconnect();
            }
            Environment.Exit(0);
        }
    }
}
