using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using Config.Reader;
using DispatchSystem.Common.DataHolders.Storage;

using CloNET;

using CitizenFX.Core;
using CloNET.LocalCallbacks;

namespace DispatchSystem.sv.External
{
    public class DispatchServer
    {
        private Server server;
        private readonly iniconfig cfg;
        private readonly Permissions perms;
        private int Port { get; }

        public ConnectedPeer[] ConnectedDispatchers => server.ConnectedPeers.Where(x => x.IsConnected).ToArray();

        internal Dictionary<Guid, string> Calls;

        internal DispatchServer(iniconfig cfg)
        {
            Calls = new Dictionary<Guid, string>();
            this.cfg = cfg;
            perms = Permissions.Get;
            Port = this.cfg.GetIntValue("server", "port", 33333);
            Log.WriteLine("Setting port to " + Port);
            Start();
        }

        private void Start()
        {
            Log.WriteLine("Creating TCP Device");
            server = new Server(cfg.GetStringValue("server", "ip", "0.0.0.0"), Port)
            {
                Encryption = new EncryptionOptions
                {
                    Encrypt = false,
                    Overridable = false
                }
            };

            server.Connected += OnConnect;
            server.Disconnected += OnDisconnect;
            Log.WriteLine("TCP Created, starting TCP");
            try { server.Listening = true; }
            catch (SocketException)
            {
                Log.WriteLine("The specified port (" + Port + ") is already in use.");
                return;
            }
            Log.WriteLine("TCP Started, Listening for connections...");

            AddCallbacks();
        }

        private void AddCallbacks()
        {
            server.LocalCallbacks.Functions.Add("GetCivilian", new LocalFunction(GetCivilian));
            server.LocalCallbacks.Functions.Add("GetCivilianVeh", new LocalFunction(GetCivilianVeh));
            server.LocalCallbacks.Functions.Add("GetBolos", new LocalFunction(GetBolos));
            server.LocalCallbacks.Functions.Add("GetOfficers", new LocalFunction(GetOfficers));
            server.LocalCallbacks.Functions.Add("GetOfficer", new LocalFunction(GetOfficer));
            server.LocalCallbacks.Functions.Add("GetAssignments", new LocalFunction(GetAssignments));
            server.LocalCallbacks.Functions.Add("CreateAssignment", new LocalFunction(NewAssignment));
            server.LocalCallbacks.Functions.Add("GetOfficerAssignment", new LocalFunction(GetOfcAssignment));
            server.LocalCallbacks.Functions.Add("Accept911", new LocalFunction(AcceptEmergency));
            server.LocalCallbacks.Events.Add("911End", new LocalEvent(EndEmergency));
            server.LocalCallbacks.Events.Add("911Msg", new LocalEvent(MessageEmergency));
            server.LocalCallbacks.Events.Add("AddOfficerAssignment", new LocalEvent(AddOfcAssignment));
            server.LocalCallbacks.Events.Add("RemoveAssignment", new LocalEvent(RemoveAssignment));
            server.LocalCallbacks.Events.Add("RemoveOfficerAssignment", new LocalEvent(RemoveOfcAssignment));
            server.LocalCallbacks.Events.Add("SetStatus", new LocalEvent(ChangeOfficerStatus));
            server.LocalCallbacks.Events.Add("RemoveOfficer", new LocalEvent(RemoveOfficer));
            server.LocalCallbacks.Events.Add("AddBolo", new LocalEvent(AddBolo));
            server.LocalCallbacks.Events.Add("RemoveBolo", new LocalEvent(RemoveBolo));
            server.LocalCallbacks.Events.Add("AddNote", new LocalEvent(AddNote));
        }

        private static async Task OnConnect(ConnectedPeer user)
        {
            await Task.Run(delegate
            {
#if DEBUG
                Log.WriteLine($"[{user.RemoteIP}] Connected");
#else
                Log.WriteLineSilent($"[{user.RemoteIP}] Connected");
#endif
            });
        }

        private static async Task OnDisconnect(ConnectedPeer user)
        {
            await Task.Run(delegate
            {
#if DEBUG
                Log.WriteLine($"[{user.RemoteIP}] Disconnected");
#else
                Log.WriteLineSilent($"[{user.RemoteIP}] Disconnected");
#endif
            });
        }

        private async Task<object> GetCivilian(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get civilian Request Recieved");
#else
            Log.WriteLineSilent("[{sender.RemoteIP}] Get civilian Request Recieved");
#endif

            string first = (string)args[0];
            string last = (string)args[1];

            Civilian civ = Common.GetCivilianByName(first, last);
            if (civ != null)
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Sending Civilian information to Client");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Sending Civilian information to Client");
#endif
                return civ;
            }
            else
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Civilian not found, sending null");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Civilian not found, sending null");
#endif
                return Civilian.Empty;
            }
        }
        private async Task<object> GetCivilianVeh(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get civilian veh Request Recieved");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get civilian veh Request Recieved");
#endif

            string plate = (string)args[0];

            CivilianVeh civVeh = Common.GetCivilianVehByPlate(plate);
            if (civVeh != null)
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Sending Civilian Veh information to Client");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Sending Civilian Veh information to Client");
#endif
                return civVeh;
            }
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Civilian Veh not found, sending null");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Civilian Veh not found, sending null");
#endif
            return CivilianVeh.Empty;
        }
        private async Task<object> GetBolos(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get bolos Request Recieved");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get bolos Request Recieved");
#endif

            return DispatchSystem.ActiveBolos;
        }
        private async Task<object> GetOfficers(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get officers Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get officers Request Received");
#endif

            return DispatchSystem.officers;
        }
        private async Task<object> GetOfficer(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

            Guid id = (Guid)args[0];

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get officer Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get officer Request Received");
#endif

            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == id);
            ofc = ofc ?? Officer.Empty;
            return ofc;
        }
        private async Task<object> GetAssignments(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get assignments Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get assignments Request Received");
#endif

            return DispatchSystem.assignments.AsEnumerable();
        }
        private async Task<object> GetOfcAssignment(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Get officer assignments Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Get officer assignments Request Received");
#endif
            Guid id = (Guid)args[0];
            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == id);

            return Common.GetOfficerAssignment(ofc);
        }
        private async Task<object> AcceptEmergency(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Accept emergency Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Accept emergency Request Received");
#endif

            Guid id = (Guid)args[0];
            EmergencyCall acceptedCall = DispatchSystem.currentCalls.Find(x => x.Id == id);
            if (Calls.ContainsKey(id) || acceptedCall == null) return false; // Checking null and accepted in same expression

            Calls.Add(id, sender.RemoteIP);
            DispatchSystem.Invoke(delegate
            {
                Player p = Common.GetPlayerByIp(acceptedCall.SourceIP);
                if (p != null)
                {
                    Common.SendMessage(p, "Dispatch911", new [] {255,0,0}, "Your 911 call has been accepted by a Dispatcher");
                }
            });
            return true;
        }
        private async Task EndEmergency(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] End emergency Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] End emergency Request Received");
#endif

            Guid id = (Guid) args[0];
            Calls.Remove(id);

            EmergencyCall call = DispatchSystem.currentCalls.Find(x => x.Id == id);

            if (DispatchSystem.currentCalls.Remove(call))
            {
                DispatchSystem.Invoke(delegate
                {
                    Player p = Common.GetPlayerByIp(call?.SourceIP);

                    if (p != null)
                    {
                        Common.SendMessage(p, "Dispatch911", new [] {255,0,0}, "Your 911 call was ended by a Dispatcher");
                    }
                });
            }
        }
        private async Task MessageEmergency(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Message emergency Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Message emergency Request Received");
#endif

            Guid id = (Guid) args[0];
            string msg = args[1] as string;

            EmergencyCall call = DispatchSystem.currentCalls.Find(x => x.Id == id);

            DispatchSystem.Invoke(() =>
            {
                Player p = Common.GetPlayerByIp(call?.SourceIP);

                if (p != null)
                {
                    Common.SendMessage(p, "Dispatcher", new [] {0x0,0xff,0x0}, msg);
                }
            });
        }
        private async Task AddOfcAssignment(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Add officer assignment Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Add officer assignment Request Received");
#endif

            Guid id = (Guid)args[0];
            Guid ofcId = (Guid)args[1];

            Assignment assignment = DispatchSystem.assignments.Find(x => x.Id == id);
            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofcId);
            if (assignment is null || ofc is null)
                return;
            if (DispatchSystem.ofcAssignments.ContainsKey(ofc))
                return;

            DispatchSystem.ofcAssignments.Add(ofc, assignment);

            ofc.Status = OfficerStatus.OffDuty;

            DispatchSystem.Invoke(() =>
            {
                Player p = Common.GetPlayerByIp(ofc.SourceIP);
                if (p != null)
                    Common.SendMessage(p, "^8DispatchCAD", new[] { 0, 0, 0 }, $"New assignment added: \"{assignment.Summary}\"");
            });
        } 
        private async Task<object> NewAssignment(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] New assignment Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] New assignment Request Received");
#endif

            string summary = args[0] as string;

            Assignment assignment = new Assignment(summary);
            DispatchSystem.assignments.Add(assignment);
            return assignment.Id;
        }
        private async Task RemoveAssignment(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Remove assignment Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Remove assignment Request Received");
#endif

            Guid item = (Guid) args[0];

            Assignment item2 = DispatchSystem.assignments.ToList().Find(x => x.Id == item);
            Common.RemoveAllInstancesOfAssignment(item2);
        }
        private async Task RemoveOfcAssignment(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Remove officer assignment Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Remove officer assignment Request Received");
#endif

            Guid ofcId = (Guid)args[0];
            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofcId);
            if (ofc == null) return;

            if (!DispatchSystem.ofcAssignments.ContainsKey(ofc)) return;
            DispatchSystem.ofcAssignments.Remove(ofc);

            ofc.Status = OfficerStatus.OnDuty;

            DispatchSystem.Invoke(() =>
            {
                Player p = Common.GetPlayerByIp(ofc.SourceIP);
                if (p != null)
                    Common.SendMessage(p, "^8DispatchCAD", new[] { 0, 0, 0 }, "Your assignment has been removed by a dispatcher");
            });
        }
        private async Task ChangeOfficerStatus(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Change officer status Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Change officer status Request Received");
#endif

            Officer ofc = (Officer)args[0];
            OfficerStatus status = (OfficerStatus)args[1];

            ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofc.Id);
            var index = DispatchSystem.officers.IndexOf(ofc);
            if (index == -1)
                return;

            Officer ourOfc = DispatchSystem.officers[index];

            if (ourOfc.Status != status)
            {
                ourOfc.Status = status;
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Setting officer status to " + status.ToString());
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Setting officer status to " + status.ToString());
#endif

                DispatchSystem.Invoke(() =>
                {
                    Player p = Common.GetPlayerByIp(ourOfc.SourceIP);
                    if (p != null)
                        Common.SendMessage(p, "^8DispatchCAD", new[] { 0, 0, 0 },
                            $"Dispatcher set status to {(ofc.Status == OfficerStatus.OffDuty ? "Off Duty" : ofc.Status == OfficerStatus.OnDuty ? "On Duty" : "Busy")}");
                });
            }
            else
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Officer status already set to the incoming status");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Officer status already set to the incoming status");
#endif
            }
        }
        private async Task RemoveOfficer(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Remove officer Request Received");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Add bolo Request Received");
#endif

            Guid ofcGiven = (Guid)args[0];

            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofcGiven);
            if (ofc != null)
            {
                DispatchSystem.Invoke(delegate
                {
                    Player p = Common.GetPlayerByIp(ofc.SourceIP);

                    if (p != null)
                        Common.SendMessage(p, "^8DispatchCAD", new[] { 0, 0, 0 }, "You have been removed from your officer role by a dispatcher");
                });

                DispatchSystem.officers.Remove(ofc);

#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Removed the officer from the list of officers");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Removed the officer from the list of officers");
#endif
            }
            else
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Officer in list not found, not removing");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Officer in list not found, not removing");
#endif
            }
        }
        private async Task AddBolo(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Add bolo Request Recieved");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Add bolo Request Recieved");
#endif

            string player = (string)args[0];
            string bolo = (string)args[1];

#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Adding new Bolo for \"{bolo}\"");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Adding new Bolo for \"{bolo}\"");
#endif
            DispatchSystem.ActiveBolos.Add(new Bolo(player, string.Empty, bolo));
        }
        private async Task RemoveBolo(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Remove bolo Request Recieved");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Remove bolo Request Recieved");
#endif

            int parse = (int)args[0];

            try
            {
                DispatchSystem.ActiveBolos.RemoveAt(parse);
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Removed Active BOLO from the List");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Removed Active BOLO from the List");
#endif
            }
            catch (ArgumentOutOfRangeException)
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Index for BOLO not found, not removing...");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Index for BOLO not found, not removing...");
#endif
            }
        }
        private async Task AddNote(ConnectedPeer sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine($"[{sender.RemoteIP}] Add Civilian note Request Recieved");
#else
            Log.WriteLineSilent($"[{sender.RemoteIP}] Add Civilian note Request Recieved");
#endif

            string[] name = { (string)args[0], (string)args[1] };
            string note = (string)args[2];

            Civilian civ = Common.GetCivilianByName(name[0], name[1]);

            if (civ != null)
            {
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Adding the note \"{note}\" to Civilian {civ.First} {civ.Last}");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Adding the note \"{note}\" to Civilian {civ.First} {civ.Last}");
#endif
                civ.Notes.Add(note);
            }
            else
#if DEBUG
                Log.WriteLine($"[{sender.RemoteIP}] Civilian not found, not adding note...");
#else
                Log.WriteLineSilent($"[{sender.RemoteIP}] Civilian not found, not adding note...");
#endif
        }

        private bool CheckAndDispose(ConnectedPeer sender)
        {
            switch (perms.DispatchPermission)
            {
                case Permission.Specific:
                    if (!perms.DispatchContains(IPAddress.Parse(sender.RemoteIP))) { Log.WriteLine($"[{sender.RemoteIP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true; }
                    break;
                case Permission.None:
                    Log.WriteLine($"[{sender.RemoteIP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true;
            }
            return false;
        }
    }
}
