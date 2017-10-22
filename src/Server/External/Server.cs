using System;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using Config.Reader;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.NetCode;

using CitizenFX.Core;

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
            Start();
        }

        public void Start()
        {
            Log.WriteLine("Creating TCP Host");
            tcp = new TcpListener(IPAddress.Parse(cfg.GetStringValue("server", "ip", "0.0.0.0")), port);
            Log.WriteLine("Setting TCP connection IP to " + tcp.LocalEndpoint.ToString().Split(':')[0]);

            Log.WriteLine("TCP Created, Attempting to start");
            try { tcp.Start(); }
            catch (SocketException)
            {
                Log.WriteLine("The specified port (" + port + ") is already in use.");
                return;
            }
            Log.WriteLine("TCP Started, Listening for connections...");

            while (true)
                ThreadPool.QueueUserWorkItem(x => {
                    try { Connect(x); }
                    catch (Exception e)
                    {
                        Log.WriteLineSilent(e.ToString());
                    }
                }, tcp.AcceptSocket());
        }
        private void Connect(object socket0)
        {
            NetRequestHandler net = new NetRequestHandler((Socket)socket0);

#if DEBUG
            Log.WriteLine($"[{net.IP}] New connection");
#else
            Log.WriteLineSilent($"[{net.IP}] New connection");
#endif

            net.Functions.Add("GetCivilian", new NetFunction(GetCivilian));
            net.Functions.Add("GetCivilianVeh", new NetFunction(GetCivilianVeh));
            net.Functions.Add("GetBolos", new NetFunction(GetBolos));
            net.Functions.Add("GetOfficers", new NetFunction(GetOfficers));
            net.Functions.Add("GetOfficer", new NetFunction(GetOfficer));
            net.Functions.Add("GetAssignments", new NetFunction(GetAssignments));
            net.Functions.Add("CreateAssignment", new NetFunction(NewAssignment));
            net.Functions.Add("GetOfficerAssignment", new NetFunction(GetOfcAssignment));
            net.Events.Add("AddOfficerAssignment", new NetEvent(AddOfcAssignment));
            net.Events.Add("RemoveAssignment", new NetEvent(RemoveAssignment));
            net.Events.Add("RemoveOfficerAssignment", new NetEvent(RemoveOfcAssignment));
            net.Events.Add("SetStatus", new NetEvent(ChangeOfficerStatus));
            net.Events.Add("RemoveOfficer", new NetEvent(RemoveOfficer));
            net.Events.Add("AddBolo", new NetEvent(AddBolo));
            net.Events.Add("RemoveBolo", new NetEvent(RemoveBolo));
            net.Events.Add("AddNote", new NetEvent(AddNote));

            net.Receive().Wait();

#if DEBUG
            Log.WriteLine($"[{net.IP}] Connection broken");
#else
            Log.WriteLineSilent($"[{net.IP}] Connection broken");
#endif
        }

        private async Task<object> GetCivilian(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine("Get civilian Request Recieved");
#else
            Log.WriteLineSilent("Get civilian Request Recieved");
#endif

            string first = (string)args[0];
            string last = (string)args[1];

            Civilian civ = Common.GetCivilianByName(first, last);
            if (civ != null)
            {
#if DEBUG
                Log.WriteLine("Sending Civilian information to Client");
#else
                Log.WriteLineSilent("Sending Civilian information to Client");
#endif
                return civ;
            }
            else
            {
#if DEBUG
                Log.WriteLine("Civilian not found, sending null");
#else
                Log.WriteLineSilent("Civilian not found, sending null");
#endif
                return Civilian.Empty;
            }
        }
        private async Task<object> GetCivilianVeh(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine("Get civilian veh Request Recieved");
#else
            Log.WriteLineSilent("Get civilian veh Request Recieved");
#endif

            string plate = (string)args[0];

            CivilianVeh civVeh = Common.GetCivilianVehByPlate(plate);
            if (civVeh != null)
            {
#if DEBUG
                Log.WriteLine("Sending Civilian Veh information to Client");
#else
                Log.WriteLineSilent("Sending Civilian Veh information to Client");
#endif
                return civVeh;
            }
            else
            {
#if DEBUG
                Log.WriteLine("Civilian Veh not found, sending null");
#else
                Log.WriteLineSilent("Civilian Veh not found, sending null");
#endif
                return CivilianVeh.Empty;
            }
        }
        private async Task<object> GetBolos(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;
#if DEBUG
            Log.WriteLine("Get bolos Request Recieved");
#else
            Log.WriteLineSilent("Get bolos Request Recieved");
#endif

            return DispatchSystem.ActiveBolos;
        }
        private async Task<object> GetOfficers(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine("Get officers Request Received");
#else
            Log.WriteLineSilent("Get officers Request Received");
#endif

            return DispatchSystem.officers;
        }
        private async Task<object> GetOfficer(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

            Guid id = (Guid)args[0];

#if DEBUG
            Log.WriteLine("Get officer Request Received");
#else
            Log.WriteLineSilent("Get officer Request Received");
#endif

            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == id);
            ofc = ofc ?? Officer.Empty;
            return ofc;
        }
        private async Task<object> GetAssignments(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine("Get assignments Request Received");
#else
            Log.WriteLineSilent("Get assignments Request Received");
#endif

            return DispatchSystem.assignments.AsEnumerable();
        }
        private async Task<object> GetOfcAssignment(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine("Get officer assignments Request Received");
#else
            Log.WriteLineSilent("Get officer assignments Request Received");
#endif
            Guid id = (Guid)args[0];
            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == id);

            return Common.GetOfficerAssignment(ofc);
        }
        private async Task AddOfcAssignment(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine("Add officer assignment Request Received");
#else
            Log.WriteLineSilent("Add officer assignment Request Received");
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
        private async Task<object> NewAssignment(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return null;

#if DEBUG
            Log.WriteLine("New assignment Request Received");
#else
            Log.WriteLineSilent("New assignment Request Received");
#endif

            string summary = args[0] as string;

            Assignment assignment = new Assignment(summary);
            DispatchSystem.assignments.Add(assignment);
            return assignment.Id;
        }
        private async Task RemoveAssignment(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine("Remove assignment Request Received");
#else
            Log.WriteLineSilent("Remove assignment Request Received");
#endif

            Assignment item = args[0] as Assignment;

            Assignment item2 = DispatchSystem.assignments.ToList().Find(x => x.Id == item.Id);
            Common.RemoveAllInstancesOfAssignment(item2);
        }
        private async Task RemoveOfcAssignment(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine("Remove officer assignment Request Received");
#else
            Log.WriteLineSilent("Remove officer assignment Request Received");
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
        private async Task ChangeOfficerStatus(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine("Change officer status Request Received");
#else
            Log.WriteLineSilent("Change officer status Request Received");
#endif

            int index = 0;
            Officer ofc = (Officer)args[0];
            OfficerStatus status = (OfficerStatus)args[1];

            ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofc.Id);
            index = DispatchSystem.officers.IndexOf(ofc);
            if (index == -1)
                return;

            Officer ourOfc = DispatchSystem.officers[index];

            if (ourOfc.Status != status)
            {
                ourOfc.Status = status;
#if DEBUG
                Log.WriteLine("Setting officer status to " + status.ToString());
#else
                Log.WriteLineSilent("Setting officer status to " + status.ToString());
#endif

                DispatchSystem.Invoke(() =>
                {
                    Player p = Common.GetPlayerByIp(ourOfc.SourceIP);
                    if (p != null)
                        Common.SendMessage(p, "^8DispatchCAD", new[] { 0, 0, 0 }, 
                            string.Format("Dispatcher set status to {0}", ofc.Status == OfficerStatus.OffDuty ? "Off Duty" : ofc.Status == OfficerStatus.OnDuty ? "On Duty" : "Busy"));
                });
            }
            else
            {
#if DEBUG
                Log.WriteLine("Officer status already set to the incoming status");
#else
                Log.WriteLineSilent("Officer status already set to the incoming status");
#endif
            }
        }
        private async Task RemoveOfficer(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;

#if DEBUG
            Log.WriteLine("Remove officer Request Received");
#else
            Log.WriteLineSilent("Add bolo Request Received");
#endif

            Officer ofcGiven = (Officer)args[0];

            Officer ofc = DispatchSystem.officers.ToList().Find(x => x.Id == ofcGiven.Id);
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
                Log.WriteLine("Removed the officer from the list of officers");
#else
                Log.WriteLineSilent("Removed the officer from the list of officers");
#endif
            }
            else
            {
#if DEBUG
                Log.WriteLine("Officer in list not found, not removing");
#else
                Log.WriteLineSilent("Officer in list not found, not removing");
#endif
            }
        }
        private async Task AddBolo(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine("Add bolo Request Recieved");
#else
            Log.WriteLineSilent("Add bolo Request Recieved");
#endif

            string player = (string)args[0];
            string bolo = (string)args[1];

#if DEBUG
            Log.WriteLine($"Adding new Bolo for \"{bolo}\"");
#else
            Log.WriteLineSilent($"Adding new Bolo for \"{bolo}\"");
#endif
            DispatchSystem.ActiveBolos.Add(new Bolo(player, string.Empty, bolo));
        }
        private async Task RemoveBolo(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine("Remove bolo Request Recieved");
#else
            Log.WriteLineSilent("Remove bolo Request Recieved");
#endif

            int parse = (int)args[0];

            try
            {
                DispatchSystem.ActiveBolos.RemoveAt(parse);
#if DEBUG
                Log.WriteLine("Removed Active BOLO from the List");
#else
                Log.WriteLineSilent("Removed Active BOLO from the List");
#endif
            }
            catch (ArgumentOutOfRangeException)
            {
#if DEBUG
                Log.WriteLine("Index for BOLO not found, not removing...");
#else
                Log.WriteLineSilent("Index for BOLO not found, not removing...");
#endif
            }
        }
        private async Task AddNote(NetRequestHandler sender, object[] args)
        {
            await Task.FromResult(0);
            if (CheckAndDispose(sender))
                return;
#if DEBUG
            Log.WriteLine("Add Civilian note Request Recieved");
#else
            Log.WriteLineSilent("Add Civilian note Request Recieved");
#endif

            string[] name = new[] { (string)args[0], (string)args[1] };
            string note = (string)args[2];

            Civilian civ = Common.GetCivilianByName(name[0], name[1]);

            if (civ != null)
            {
#if DEBUG
                Log.WriteLine($"Adding the note \"{note}\" to Civilian {civ.First} {civ.Last}");
#else
                Log.WriteLineSilent($"Adding the note \"{note}\" to Civilian {civ.First} {civ.Last}");
#endif
                civ.Notes.Add(note);
            }
            else
#if DEBUG
                Log.WriteLine("Civilian not found, not adding note...");
#else
                Log.WriteLineSilent("Civilian not found, not adding note...");
#endif
        }

        private bool CheckAndDispose(NetRequestHandler sender)
        {
            if (perms.DispatchPermission == Permission.Specific) { if (!perms.DispatchContains(IPAddress.Parse(sender.IP))) { Log.WriteLine($"[{sender.IP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true; } }
            else if (perms.DispatchPermission == Permission.None) { Log.WriteLine($"[{sender.IP}] NOT ENOUGH DISPATCH PERMISSIONS"); return true; }
            return false;
        }
    }
}
