using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

using Config.Reader;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace DispatchSystem.Server
{
    internal class Server
    {
        public static class Log
        {
            static object _lock = new object();
            static StreamWriter writer;

            public static void Create(string fileName)
            {
                writer = new StreamWriter(fileName);
            }

            public static void WriteLine(string line)
            {
                lock(_lock)
                {
                    string formatted = $"[{DateTime.Now.ToString()}]: {line}";
                    writer.WriteLine(formatted);
                    writer.Flush();

                    Debug.WriteLine($"(DispatchSystem) {formatted}");
                }
            }
            public static void WriteLineSilent(string line)
            {
                lock (_lock)
                {
                    string formatted = $"[{DateTime.Now.ToString()}]: {line}";
                    writer.WriteLine(formatted);
                    writer.Flush();
                }
            }
        }

        TcpListener tcp;
        iniconfig cfg;
        int port;
        int Port => port;

        public Server(iniconfig cfg)
        {
            this.cfg = cfg;
            port = this.cfg.GetIntValue("server", "port", 33333);
            Log.WriteLine("Setting port to " + port.ToString());

            Log.WriteLine("Creating TCP Host");
            tcp = new TcpListener(IPAddress.Parse(cfg.GetStringValue("server", "ip", "0.0.0.0")), port);
            Log.WriteLine("Setting TCP connection IP to " + tcp.LocalEndpoint.ToString().Split(':')[0]);

            Log.WriteLine("TCP Created, Attempting to start");
            try { tcp.Start(); }
            catch
            {
                Log.WriteLine("The specified port (" + port + ") is already in use.");
                return;
            }
            Log.WriteLine("TCP Started, Listening for connections...");

            while (true)
            {
                ThreadPool.QueueUserWorkItem(x => { try { Connect(x); } catch (Exception e)
                    {
                        Log.WriteLine(e.ToString());
                    } }, tcp.AcceptSocket());
            }
        }

        private void Connect(object socket0)
        {
            Socket socket = (Socket)socket0;
            string ip = socket.RemoteEndPoint.ToString().Split(':')[0];
            Log.WriteLine($"New connection from ip");

            while (socket.Connected)
            {
                byte[] buffer = new byte[1001];
                int e = socket.Receive(buffer);
                if (e == -1) { socket.Disconnect(true); break; }
                byte tag = buffer[0];
                buffer = buffer.Skip(1).ToArray();

                switch (tag)
                {
                    // Civilian Request
                    case 1:
                        {
                            Log.WriteLine("Civilian Request Recieved");

                            string name_input = Encoding.UTF8.GetString(buffer);
                            name_input = name_input.Split('!')[0];
                            string[] split = name_input.Split('|');
                            string first, last;
                            first = split[0];
                            last = split[1];
                            Civilian civ = null;
                            foreach (var item in DispatchSystem.Civilians)
                            {
                                if (item.First.ToLower() == first.ToLower() && item.Last.ToLower() == last.ToLower())
                                {
                                    civ = item;
                                    break;
                                }
                            }
                            if (civ != null)
                            {
                                Log.WriteLine("Sending Civilian information to Client");
                                socket.Send(new byte[] { 1 }.Concat(civ.ToBytes()).ToArray());
                            }
                            else
                            {
                                Log.WriteLine("Civilian not found, sending null");
                                socket.Send(new byte[] { 2 });
                            }

                            break;
                        }
                    // Civilian Veh Request
                    case 2:
                        {
                            Log.WriteLine("Civilian Veh Request Recieved");

                            string plate_input = Encoding.UTF8.GetString(buffer);
                            plate_input = plate_input.Split('!')[0];
                            CivilianVeh civVeh = null;
                            foreach (var item in DispatchSystem.CivilianVehs)
                            {
                                if (item.Plate.ToLower() == plate_input.ToLower())
                                {
                                    civVeh = item;
                                    break;
                                }
                            }
                            if (civVeh != null)
                            {
                                Log.WriteLine("Sending Civilian Veh information to Client");
                                socket.Send(new byte[] { 3 }.Concat(civVeh.ToBytes()).ToArray());
                            }
                            else
                            {
                                Log.WriteLine("Civilian Veh not found, sending null");
                                socket.Send(new byte[] { 4 });
                            }

                            break;
                        }
                    // Bolos list request
                    case 3:
                        {
                            Log.WriteLine("Bolos list Request Recieved");

                            string outstring = string.Empty;
                            if (DispatchSystem.ActiveBolos.Count > 0)
                                for (int i = 0; i < DispatchSystem.ActiveBolos.Count; i++)
                                {
                                    var item = DispatchSystem.ActiveBolos[i];

                                    if (i != 0)
                                        outstring += '|';

                                    outstring += $"{i}\\{item.Item1}:{item.Item2}";
                                }
                            else
                                outstring = "?";
                            outstring += "^";

                            Log.WriteLine("Sending back BOLO information");
                            socket.Send(new byte[] { 5 }.Concat(Encoding.UTF8.GetBytes(outstring)).ToArray());
                            Log.WriteLine("Information Sent");

                            break;
                        }
                    // Remove bolo from list Request
                    case 4:
                        {
                            Log.WriteLine("Remove Bolo from List Request Recieved");

                            string instring = Encoding.UTF8.GetString(buffer).Split('^')[0];
                            int parse = int.Parse(instring);

                            try { DispatchSystem.ActiveBolos.RemoveAt(parse); Log.WriteLine("Removed Active BOLO from the List"); }
                            catch { Log.WriteLine("Index for BOLO not found, not removing..."); }

                            break;
                        }
                    // Add bolo to list Request
                    case 5:
                        {
                            Log.WriteLine("Add Bolo from List Request Recieved");

                            string anInstring = Encoding.UTF8.GetString(buffer).Split('^')[0];
                            string[] main = anInstring.Split('|');

                            Log.WriteLine($"Adding new Bolo for \"{main[1]}\"");
                            DispatchSystem.ActiveBolos.Add((main[0], main[1]));

                            break;
                        }
                    // Add Civilian Note
                    case 6:
                        {
                            Log.WriteLine("Add Civilian note Request Recieved");

                            string input = Encoding.UTF8.GetString(buffer).Split('^')[0];
                            Log.WriteLine(input);
                            string[] main = input.Split('|');
                            string[] name = main[0].Split(',');
                            string note = main[1];

                            Civilian civ = DispatchSystem.GetCivilianByName(name[0], name[1]);

                            if (civ != null)
                            {
                                Log.WriteLine($"Adding the note \"{note}\" to Civilian {civ.First} {civ.Last}");
                                civ.Notes.Add(note);
                            }
                            else
                                Log.WriteLine("Civilian not found, not adding note...");
                            break;
                        }
                }
            }
            Log.WriteLine($"Connection from ip broken");
        }
    }
}
