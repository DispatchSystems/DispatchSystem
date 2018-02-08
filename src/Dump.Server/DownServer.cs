﻿using System;
using System.IO;
using System.Threading.Tasks;

using CloNET;
using CloNET.Callbacks;
using CloNET.LocalCallbacks;

namespace DispatchSystem.Dump.Server
{
    internal class DownServer
    {
        private const string IP = "0.0.0.0";
        private const int PORT = 52535;
        private readonly CloNET.Server server;

        public DownServer()
        {
            server = new CloNET.Server(IP, PORT)
            {
                Compression = new CompressionOptions
                {
                    Compress = false,
                    Overridable = false
                },
                Encryption = new EncryptionOptions
                {
                    Encrypt = false,
                    Overridable = false
                }
            };
            server.Connected += ConnectedClient;
            server.Disconnected += DisconnectedClient;

            AddCallbacks();

            server.Listening = true;
        }
        private void AddCallbacks()
        {
            server.LocalCallbacks.Events = new MemberDictionary<string, LocalEvent>
            {
                {"Send_3.*.*", new LocalEvent(new Func<ConnectedPeer, int, string, byte[], Task>(SendV3)) }
            };
        }

        private static async Task DisconnectedClient(ConnectedPeer arg1, DisconnectionType arg2)
        {
            await Task.FromResult(0);
            Console.WriteLine($"[{arg1.RemoteIP}] Disconnected from server {{{arg2.ToString()}}}");
        }
        private static async Task ConnectedClient(ConnectedPeer arg)
        {
            await Task.FromResult(0);
            Console.WriteLine($"[{arg.RemoteIP}] Connected to server");
        }
        private static async Task SendV3(ConnectedPeer peer, int code, string ver, byte[] info)
        {
            await Task.FromResult(0);

            int i = 0;
            string path = $"{code}.{peer.RemoteIP.Replace(".", "-")}.{ver.Replace(".", "-")}.{i}";
            while (File.Exists($"dumps/{path}.dmp"))
                path = $"{code}.{peer.RemoteIP.Replace(".", "-")}.{ver.Replace(".", "-")}.{++i}";

            try
            {
                new Saver(path, info).Save();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving files: \n" + e); // logging the problem (only available to BlockBa5her)
            }
        }
    }
}
