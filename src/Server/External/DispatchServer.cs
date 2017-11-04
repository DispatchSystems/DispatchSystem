using System;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using Config.Reader;
using DispatchSystem.Common.DataHolders.Storage;

using CloNET;
using CloNET.Callbacks;
using CloNET.Interfaces;

using CitizenFX.Core;

namespace DispatchSystem.sv.External
{
    public class DispatchServer
    {
        private Server server;
        private readonly iniconfig cfg;
        private readonly Permissions perms;
        private int Port { get; }

        internal DispatchServer(iniconfig cfg)
        {
            this.cfg = cfg;
            perms = Permissions.Get;
            Port = this.cfg.GetIntValue("server", "port", 33333);
            Log.WriteLine("Setting port to " + Port);
            Start();
        }

        public void Start()
        {
            Log.WriteLine("Creating TCP Device");
            server = new Server(cfg.GetStringValue("server", "ip", "0.0.0.0"), Port);

            Log.WriteLine("TCP Created, starting TCP");
            try { server.Start(); }
            catch (SocketException)
            {
                Log.WriteLine("The specified port (" + Port + ") is already in use.");
                return;
            }
            Log.WriteLine("TCP Started, Listening for connections...");

            AddCallbacks();
            new Thread((ThreadStart) delegate
            {
                server.UserConnected += OnConnect;
                server.UserDisconnected += OnDisconnect;
            }).Start();
        }

        private void AddCallbacks()
        {
            server.Functions.Add("GetCivilian", new NetFunction(GetCivilian));
            server.Functions.Add("GetCivilianVeh", new NetFunction(GetCivilianVeh));
            server.Functions.Add("GetBolos", new NetFunction(GetBolos));
            server.Functions.Add("GetOfficers", new NetFunction(GetOfficers));
            server.Functions.Add("GetOfficer", new NetFunction(GetOfficer));
            server.Functions.Add("GetAssignments", new NetFunction(GetAssignments));
            server.Functions.Add("CreateAssignment", new NetFunction(NewAssignment));
            server.Functions.Add("GetOfficerAssignment", new NetFunction(GetOfcAssignment));
            server.Events.Add("AddOfficerAssignment", new NetEvent(AddOfcAssignment));
            server.Events.Add("RemoveAssignment", new NetEvent(RemoveAssignment));
            server.Events.Add("RemoveOfficerAssignment", new NetEvent(RemoveOfcAssignment));
            server.Events.Add("SetStatus", new NetEvent(ChangeOfficerStatus));
            server.Events.Add("RemoveOfficer", new NetEvent(RemoveOfficer));
            server.Events.Add("AddBolo", new NetEvent(AddBolo));
            server.Events.Add("RemoveBolo", new NetEvent(RemoveBolo));
            server.Events.Add("AddNote", new NetEvent(AddNote));
        }

        private static Task OnConnect(IConnectedPeer user)
        {
            return Task.Run(delegate
            {
#if DEBUG
                Log.WriteLine($"[{user.RemoteIP}] Connected");
#else
                Log.WriteLineSilent($"[{user.RemoteIP}] Connected");
#endif
            });
        }

        private static Task OnDisconnect(IConnectedPeer user)
        {
            return Task.Run(delegate
            {
#if DEBUG
                Log.WriteLine($"[{user.RemoteIP}] Disconnected");
#else
                Log.WriteLineSilent($"[{user.RemoteIP}] Disconnected");
#endif
            });
        }

        private async Task<object> GetCivilian(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetCivilianVeh(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetBolos(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetOfficers(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetOfficer(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetAssignments(IConnectedPeer sender, object[] args)
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
        private async Task<object> GetOfcAssignment(IConnectedPeer sender, object[] args)
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
        private async Task AddOfcAssignment(IConnectedPeer sender, object[] args)
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
        private async Task<object> NewAssignment(IConnectedPeer sender, object[] args)
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
        private async Task RemoveAssignment(IConnectedPeer sender, object[] args)
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
        private async Task RemoveOfcAssignment(IConnectedPeer sender, object[] args)
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
        private async Task ChangeOfficerStatus(IConnectedPeer sender, object[] args)
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
        private async Task RemoveOfficer(IConnectedPeer sender, object[] args)
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
        private async Task AddBolo(IConnectedPeer sender, object[] args)
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
        private async Task RemoveBolo(IConnectedPeer sender, object[] args)
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
        private async Task AddNote(IConnectedPeer sender, object[] args)
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

        private bool CheckAndDispose(IBaseNet sender)
        {
            if (perms.DispatchPermission == Permission.Specific) { if (!perms.DispatchContains(IPAddress.Parse(sender.RemoteIP))) { Log.WriteLine($"[{sender.RemoteIP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true; } }
            else if (perms.DispatchPermission == Permission.None) { Log.WriteLine($"[{sender.RemoteIP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true; }
            return false;
        }
    }
}
