using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using DispatchSystem.cl.Windows;
using DispatchSystem.cl.Windows.Emergency;

using CloNET;
using CloNET.LocalCallbacks;
using DispatchSystem.Common.DataHolders.Storage;

namespace DispatchSystem.cl
{
    internal static class Program
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

            Run();
        }

        private static async void Run()
        {
            using (Client = new Client())
            {
                Client.Encryption = new EncryptionOptions
                {
                    Encrypt = true,
                    Overridable = true
                };
                Client.Compression = new CompressionOptions
                {
                    Compress = false,
                    Overridable = true
                };

                Client.LocalCallbacks.Events.Add("SendInvalidPerms", new LocalEvent(new Func<ConnectedPeer, dynamic, Task>(InvalidPerms)));
                Client.LocalCallbacks.Events.Add("911alert", new LocalEvent(new Func<ConnectedPeer, Civilian, EmergencyCall, Task>(Alert911)));
                // Server still disconnects even if event is canceled
                

                if (!Client.Connect(Config.Ip.ToString(), Config.Port).Result)
                {
                    MessageBox.Show("Connection refused or failed!\nPlease contact the owner of your server",
                        "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                }

                #region Connection Detection Thread
                var cd = new Thread(delegate ()
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (Client.IsConnected) continue;
                        if (mainWindow == null) continue;

                        bool[] executing = { true };
                        new Thread(() =>
                        {
                            mainWindow.Invoke((MethodInvoker)delegate
                            {
                                while (executing[0])
                                    Thread.Sleep(50);
                            });
                        })
                        { Name = "WindowFreezeThread" }.Start();

                        for (var i = 0; i < RECONNECT_COUNT; i++)
                        {
                            try
                            {
                                Client.Connect(Config.Ip.ToString(), Config.Port).Wait();
                            }
                            catch (SocketException)
                            {
                            }

                            Thread.Sleep(2000);
                            if (Client.IsConnected) break;
                        }

                        if (Client.IsConnected)
                        {
                            executing[0] = false;
                        }
                        else
                        {
                            MessageBox.Show($"Failed to connect to the server after {RECONNECT_COUNT} attempts",
                                "DispatchSystem",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(-1);
                        }
                    }
                })
                { Name = "ConnectionDetection" };
                cd.Start();
                #endregion

                Application.Run(mainWindow = new DispatchMain());
                cd.Abort();
                await Client.Disconnect();

                Environment.Exit(0);
            }
        }

        private static async Task InvalidPerms(ConnectedPeer peer, dynamic i)
        {
            await Task.FromResult(0);

            MessageBox.Show("You seem to have invalid permissions", "DispatchSystem", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
            Environment.Exit(-13);
        }
        private static async Task Alert911(ConnectedPeer peer, Civilian civ, EmergencyCall call)
        {
            await Task.FromResult(0);

            mainWindow.Invoke((MethodInvoker)delegate
            {
                new Accept911(civ, call).Show();
            });
        }
    }
}
