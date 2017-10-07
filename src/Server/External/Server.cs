#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;

using Config.Reader;
using CitizenFX.Core;
using CitizenFX.Core.Native;

using DispatchSystem.Common.DataHolders;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.DataHolders.Requesting;
using DispatchSystem.Common.NetCode;
using DispatchSystem.Common;

namespace DispatchSystem.sv.External
{
    public class Server
    {
        TcpListener tcp;
        iniconfig cfg;
        Permissions perms;
        int port;
        int Port => port;

        internal Server(iniconfig cfg)
        {
            this.cfg = cfg;
            perms = Permissions.Get;
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
                        Log.WriteLineSilent(e.ToString());
                    } }, tcp.AcceptSocket());
            }
        }

        private void Connect(object socket0)
        {
            Socket socket = (Socket)socket0;
            string ip = socket.RemoteEndPoint.ToString().Split(':')[0];
#if DEBUG
            Log.WriteLine($"New connection from ip");
#else
            Log.WriteLineSilent($"New connection from ip");
#endif

            Action<string> cancelInteraction = new Action<string>(msg =>
            {
                socket.Send(new byte[] { 3 });
                socket.Disconnect(false);
                Log.WriteLine(msg);
            });

            if (perms.DispatchPermission == Permission.Specific) { if (!perms.DispatchContains(IPAddress.Parse(ip))) { cancelInteraction($"[{ip}] NOT ENOUGH DISPATCH PERMISSIONS"); return; } }
            else if (perms.DispatchPermission == Permission.None) { cancelInteraction($"[{ip}] NOT ENOUGH DISPATCH PERMISSIONS"); return; }

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
#if DEBUG
                            Log.WriteLine("Civilian Request Recieved");
#else
                            Log.WriteLineSilent("Civilian Request Recieved");
#endif

                            StorableValue<CivilianRequest> item = new StorableValue<CivilianRequest>(buffer);
                            Civilian civ = DispatchSystem.GetCivilianByName(item.Value.First, item.Value.Last);
                            if (civ != null)
                            {
#if DEBUG
                                Log.WriteLine("Sending Civilian information to Client");
#else
                                Log.WriteLineSilent("Sending Civilian information to Client");
#endif
                                socket.Send(new byte[] { 1 }.Concat(new StorableValue<Civilian>(civ).Bytes).ToArray());
                            }
                            else
                            {
#if DEBUG
                                Log.WriteLine("Civilian not found, sending null");
#else
                                Log.WriteLineSilent("Civilian not found, sending null");
#endif
                                socket.Send(new byte[] { 2 });
                            }

                            break;
                        }
                    // Civilian Veh Request
                    case 2:
                        {
#if DEBUG
                            Log.WriteLine("Civilian Veh Request Recieved");
#else
                            Log.WriteLineSilent("Civilian Veh Request Recieved");
#endif

                            StorableValue<CivilianVehRequest> plate_input = new StorableValue<CivilianVehRequest>(buffer);
                            CivilianVeh civVeh = DispatchSystem.GetCivilianVehByPlate(plate_input.Value.Plate);
                            if (civVeh != null)
                            {
#if DEBUG
                                Log.WriteLine("Sending Civilian Veh information to Client");
#else
                                Log.WriteLineSilent("Sending Civilian Veh information to Client");
#endif
                                socket.Send(new byte[] { 1 }.Concat(new StorableValue<CivilianVeh>(civVeh).Bytes).ToArray());
                            }
                            else
                            {
#if DEBUG
                                Log.WriteLine("Civilian Veh not found, sending null");
#else
                                Log.WriteLineSilent("Civilian Veh not found, sending null");
#endif
                                socket.Send(new byte[] { 2 });
                            }

                            break;
                        }
                    // Bolos list request
                    case 3:
                        {
#if DEBUG
                            Log.WriteLine("Bolos list Request Recieved");
#else
                            Log.WriteLineSilent("Bolos list Request Recieved");
#endif
#if DEBUG
                            Log.WriteLine("Sending back BOLO information");

                            Log.WriteLine("Creating storage");
                            StorableValue<List<Bolo>> storage = new StorableValue<List<Bolo>>(DispatchSystem.ActiveBolos);
                            Log.WriteLine("Sending Bytes");
                            socket.Send(new byte[] { 1 }.Concat(storage.Bytes).ToArray());
                            Log.WriteLine("Information Sent");
#else
                            Log.WriteLineSilent("Sending back BOLO information");
                            socket.Send(new byte[] { 1 }.Concat(new StorableValue<List<Bolo>>(DispatchSystem.ActiveBolos).Bytes).ToArray());
                            Log.WriteLineSilent("Information Sent");
#endif

                            break;
                        }
                    // Remove bolo from list Request
                    case 4:
                        {
                            Log.WriteLine("Remove Bolo from List Request Recieved");

                            string instring = (string)new StorableValue<DataRequest>(buffer).Value.Data[0];
                            int parse = int.Parse(instring);

                            try { DispatchSystem.ActiveBolos.RemoveAt(parse); Log.WriteLine("Removed Active BOLO from the List"); }
                            catch (ArgumentOutOfRangeException) { Log.WriteLine("Index for BOLO not found, not removing..."); }

                            break;
                        }
                    // Add bolo to list Request
                    case 5:
                        {
                            Log.WriteLine("Add Bolo from List Request Recieved");

                            object[] main = new StorableValue<DataRequest>(buffer).Value.Data;

                            Log.WriteLineSilent($"Adding new Bolo for \"{main[1]}\"");
                            DispatchSystem.ActiveBolos.Add(new Bolo((string)main[0], (string)main[1]));

                            break;
                        }
                    // Add Civilian Note
                    case 6:
                        {
                            Log.WriteLine("Add Civilian note Request Recieved");

                            object[] main = new StorableValue<DataRequest>(buffer).Value.Data;
                            string[] name = new[] { (string)main[0], (string)main[1] };
                            string note = (string)main[2];

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
#if DEBUG
            Log.WriteLine($"Connection from ip broken");
#else
            Log.WriteLineSilent($"Connection from ip broken");
#endif
        }
    }
}
