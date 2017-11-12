using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DispatchSystem.cl.Windows;

using CloNET;
using CloNET.Callbacks;
using DispatchSystem.cl.Windows.Emergency;
using DispatchSystem.Common.DataHolders.Storage;

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
                Client.Encryption = new EncryptionOptions
                {
                    Encrypt = false,
                    Overridable = true
                };


                new Action(async () =>
                {
                    try { await Client.Connect(Config.IP.ToString(), Config.Port); }
                    catch (SocketException) { MessageBox.Show("Connection refused or failed!\nPlease contact the owner of your server", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error); Environment.Exit(-1); }
                })();

                Client.Events.Add("911alert", new NetEvent(async (peer, objects) =>
                {
                    await Task.FromResult(0);

                    mainWindow.Invoke((MethodInvoker) delegate
                    {
                        new Accept911((Civilian) objects[0], (EmergencyCall) objects[1]).Show();
                    });
                }));

                Client.Connected += delegate
                {
                    new Thread((ThreadStart)async delegate
                        {
                            while (true)
                            {
                                Thread.Sleep(100);
                                if (Client.IsConnected) continue;
                                if (mainWindow == null) continue;

                                bool executing = true;
                                new Thread(() =>
                                    {
                                        mainWindow.Invoke((MethodInvoker)delegate
                                        {
                                            while (executing)
                                            {
                                                Thread.Sleep(50);
                                            }
                                        });
                                    })
                                    { Name = "WindowFreezeThread" }.Start();

                                for (int i = 0; i < RECONNECT_COUNT; i++)
                                {
                                    try
                                    {
                                        await Client.Disconnect();
                                        await Client.Connect(Config.IP.ToString(), Config.Port);
                                    }
                                    catch (SocketException)
                                    {
                                    }

                                    Thread.Sleep(1000);
                                    if (Client.IsConnected) break;
                                }

                                if (Client.IsConnected)
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
                };

                Application.Run(mainWindow = new DispatchMain());

                new Action(async delegate { await Client.Disconnect(); })();
            }
            Environment.Exit(0);
        }
    }
}
