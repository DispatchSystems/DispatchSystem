using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.Core;

using Config.Reader;

using Dispatch.Common.DataHolders;
using Dispatch.Common.DataHolders.Storage;

using DispatchSystem.Server.External;
using DispatchSystem.Server.RequestHandling;

using EZDatabase;

namespace DispatchSystem.Server.Main
{
    public static class Core
    {
        #region Variables
        internal static ServerConfig Config; // config
        internal static List<string> DispatchPerms; // permissions
        internal static DispatchServer Server; // server for client+server transactions
        internal static Database Data; // database for saving

        internal static StorageManager<Bolo> Bolos; // active bolos
        internal static StorageManager<Civilian> Civilians; // civilians
        internal static StorageManager<CivilianVeh> CivilianVehs; // civilian vehicles
        internal static StorageManager<Officer> Officers; // current officers
        internal static StorageManager<Assignment> Assignments; // active assignments
        internal static Dictionary<Officer, Assignment> OfficerAssignments; // assignments attached to officers
        internal static StorageManager<EmergencyCall> CurrentCalls; // 911 calls
        internal static RequestHandler RequestHandler;
        #endregion

        #region Events
        #region General Events
        public static RequestData DispatchReset(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);
            var deletedProfiles = new List<EventArgument>();

            var civ = Common.GetCivilian(p.Handle);
            var veh = Common.GetCivilianVeh(p.Handle);
            var ofc = Common.GetOfficer(p.Handle);

            if (civ != null)
            {
#if DEBUG
                Common.SendMessage(p, "", new [] {0,0,0}, "Removing Civilian Profile...");
#endif
                Civilians.Remove(civ); // removing instance of civilian
                deletedProfiles.Add(civ.ToArray());
            }
            if (veh != null)
            {
#if DEBUG
                Common.SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Civilian Vehicle Profile...");
#endif
                CivilianVehs.Remove(veh); // removing instance of vehicle
                deletedProfiles.Add(veh.ToArray());
            }
            if (ofc != null)
            {
#if DEBUG
                Common.SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Officer Profile...");
#endif
                Officers.Remove(ofc); // removing instance of officer
                deletedProfiles.Add(ofc.ToArray());
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), deletedProfiles.ToArray()});
        }
        #endregion
        #region Set Requests
        public static RequestData SetDispatchPerms(object[] args)
        {
            DispatchPerms = args.Select(x => (string) x).ToList();
            return new RequestData(null, DispatchPerms.Select(x => (EventArgument) x).ToArray());
        }
        #endregion
        #region Request Types
        public static RequestData RequestCivilian(string handle)
        {
            Civilian civ = Common.GetCivilian(handle); // finding the civ
            return civ != null ? new RequestData(null, civ.ToArray()) : new RequestData("civ_not_exist");
        }
        public static RequestData RequestCivilianVeh(string handle)
        {
            CivilianVeh civVeh = Common.GetCivilianVeh(handle); // finding the vehicle
            return civVeh != null ? new RequestData(null, civVeh.ToArray()) : new RequestData("veh_not_exist");
        }
        public static RequestData RequestCivilianByName(string first, string last)
        {
            Civilian civ = Common.GetCivilianByName(first, last); // finding the civ
            return civ != null ? new RequestData(null, civ.ToArray()) : new RequestData("civ_not_exist");
        }
        public static RequestData RequestCivilianVehByPlate(string plate)
        {
            CivilianVeh civVeh = Common.GetCivilianVehByPlate(plate); // finding the vehicle
            return civVeh != null ? new RequestData(null, civVeh.ToArray()) : new RequestData("veh_not_exist");
        }
        public static RequestData RequestOfficer(string handle)
        {
            Officer ofc = Common.GetOfficer(handle);
            return ofc != null ? new RequestData(null, ofc.ToArray()) : new RequestData("leo_not_exist");
        }
        public static RequestData RequestOfficerByCallsign(string callsign)
        {
            Officer ofc = Officers.First(x => x.Callsign == callsign);
            return ofc != null ? new RequestData(null, ofc.ToArray()) : new RequestData("leo_not_exist");
        }
        public static RequestData RequestOfficerAssignment(string handle)
        {
            Officer ofc = Common.GetOfficer(handle);
            if (ofc == null)
                return new RequestData("leo_not_exist");
            Assignment a = OfficerAssignments.ContainsKey(ofc) ? OfficerAssignments[ofc] : null;
            return a != null
                ? new RequestData(null, new EventArgument[] {ofc.ToArray(), a.ToArray()})
                : new RequestData("leo_assignment_not_exist");
        }
        public static RequestData RequestBolos()
        {
            // ReSharper disable once CoVariantArrayConversion
            return new RequestData(null, Bolos.Select(x => (EventArgument)x.ToArray()).ToArray());
        }
        #endregion
        #region Civilian Events
        public static RequestData SetName(string handle, string first, string last)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var civ = Common.GetCivilian(handle);
            var oldVeh = Common.GetCivilianVeh(handle);

            if (Common.GetCivilianByName(first, last) != null && Common.GetPlayerByIp(civ?.SourceIP) != p) // checking if the name already exists in the system
            {
                return new RequestData("civ_name_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }
            if (oldVeh != null)
            {
                // deletes vehicle from list of vehicles
                CivilianVehs.Remove(oldVeh);
            }

            // old civilian information, for the event
            EventArgument[] oldCivInfo = null;
            // checking if the civilian already has a civ in the system
            if (Common.GetCivilian(handle) != null)
            {
                int index = Civilians.IndexOf(Common.GetCivilian(handle)); // finding the index of the existing civ

                civ = new Civilian(p.Identifiers["ip"]) { First = first, Last = last }; // setting the index to an instance of a new civilian
                oldCivInfo = Civilians[index].ToArray();
                Civilians[index] = civ;
            }
            else // if the civ doesn't exist
            {
                civ = new Civilian(p.Identifiers["ip"]) {First = first, Last = last};
                Civilians.Add(civ); // add a new civilian to the system

#if DEBUG
                Common.SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new civilian profile...");
#endif
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), civ.ToArray(), oldCivInfo});
        }
        public static RequestData ToggleWarrant(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);
            var civ = Common.GetCivilian(handle);

            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var old = civ.ToArray();
            // setting the warrant status of the opposite of before
            civ.WarrantStatus = !civ.WarrantStatus;

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), civ.ToArray(), old});
        }
        public static RequestData SetCitations(string handle, int count)
        {
            Player p = Common.GetPlayerByHandle(handle);
            var civ = Common.GetCivilian(handle);

            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var old = civ.ToArray();
            civ.CitationCount = count; // setting the count of the citations

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), civ.ToArray(), old});
        }
        public static async Task<RequestData> InitializeEmergency(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);
            // ReSharper disable once IdentifierTypo
            var emerCall = Common.GetEmergencyCall(handle);

            // checking for an existing emergency
            if (emerCall != null) return new RequestData("civ_911_exist", new EventArgument[] {Common.GetPlayerId(p)});

            Civilian civ = Common.GetCivilian(handle); // finding the civ
            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            // checking how many active dispatchers there are
            if (Server.ConnectedDispatchers.Length == 0)
                return new RequestData("civ_911_no_dispatchers", new EventArgument[] {Common.GetPlayerId(p)});

            emerCall = new EmergencyCall(p.Identifiers["ip"],
                $"{civ.First} {civ.Last}");
            CurrentCalls.Add(emerCall); // adding and creating the instance of the emergency
            foreach (var peer in Server.ConnectedDispatchers)
            {
                await peer.RemoteCallbacks.Events["911alert"]
                    .Invoke(civ, emerCall); // notifying the dispatchers of the 911 call
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p)});
        }
        public static async Task<RequestData> MessageEmergency(string handle, string msg)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var call = Common.GetEmergencyCall(handle);
            if (call?.Accepted ?? true) // checking if null and if accepted in the same check
            {
                return new RequestData("civ_911_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }

            var dispatcherIp = Server.Calls[call.Id]; // getting the dispatcher ip from the calls
            var peer = Server.ConnectedDispatchers.First(x => x.RemoteIP == dispatcherIp); // finding the peer from the given IP
            await peer.RemoteCallbacks.Events[call.Id.ToString()].Invoke(msg); // invoking the remote event for the message in the peer

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), msg});
        }
        public static async Task<RequestData> EndEmergency(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            EmergencyCall call = Common.GetEmergencyCall(handle);
            // checking for call null
            if (call == null)
            {
                return new RequestData("civ_911_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }

#if DEBUG
            Common.SendMessage(p, "", new[] { 0, 0, 0 }, "Ending 911 call...");
#endif

            // finding the dispatcher ip
            var dispatcherIp = Server.Calls.ContainsKey(call.Id) ? Server.Calls[call.Id] : null;
            var peer = Server.ConnectedDispatchers.FirstOrDefault(x => x.RemoteIP == dispatcherIp); // finding the peer from the ip
            var task = peer?.RemoteCallbacks.Events?["end" + call.Id].Invoke(); // creating the task from the events

            // removing the call from the calls list
            CurrentCalls.Remove(call);

            if (!(task is null)) await task; // await the task

            return new RequestData(null, new EventArgument[] { Common.GetPlayerId(p) });
        }
        #endregion
        #region Vehicle Events
        public static RequestData SetVehicle(string handle, string plate)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var civ = Common.GetCivilian(handle);
            var veh = Common.GetCivilianVeh(handle);

            // if no civilian exists
            if (civ == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }
            // checking if the plate already exists in the system
            if (Common.GetCivilianVehByPlate(plate) != null && Common.GetPlayerByIp(veh.SourceIP) != p)
            {
                return new RequestData("veh_plate_exist", new EventArgument[] {Common.GetPlayerId(p), plate});
            }

            EventArgument[] old = null;
            // checking if player already owns a vehicle
            if (veh != null)
            {
                int index = CivilianVehs.IndexOf(Common.GetCivilianVeh(handle)); // finding the existing index
                old = CivilianVehs[index].ToArray();
                // setting the index to a new vehicle item
                veh = new CivilianVeh(p.Identifiers["ip"]) {Plate = plate, Owner = Common.GetCivilian(handle)};
                CivilianVehs[index] = veh;
            }
            else
            {
                veh = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = Common.GetCivilian(handle) }; // creating the new vehicle
                CivilianVehs.Add(veh); // adding the new vehicle to the list of vehicles
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), veh.ToArray(), old});
        }
        public static RequestData ToggleVehicleStolen(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var civ = Common.GetCivilian(handle);
            var veh = Common.GetCivilianVeh(handle);
            
            // checking if player has a name
            if (civ == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }

            // checking if vehicle exists
            if (veh == null) return new RequestData("veh_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var old = veh.ToArray();
            veh.StolenStatus = !veh.StolenStatus; // toggle stolen

            if (veh.StolenStatus) // checking if it is stolen
            {
                civ = Civilian.CreateRandomCivilian(); // creating a new random civ
                veh.Owner = civ; // setting the vehicle owner to the civ
                Civilians.Add(civ); // adding the civ to the database
            }
            else
            {
                Civilians.Remove(civ); // removing the civ from the database
                veh.Owner = civ; // setting the owner to the person
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), veh.ToArray(), old});
        }
        public static RequestData ToggleVehicleRegistration(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var civ = Common.GetCivilian(handle);
            var veh = Common.GetCivilianVeh(handle);

            // checking for existing civ
            if (civ == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }

            // checking if the vehicle exists
            if (veh == null) return new RequestData("veh_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var old = veh.ToArray();
            veh.Registered = !veh.Registered; // setting the registration
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), veh.ToArray(), old});
        }
        public static RequestData ToggleVehicleInsurance(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var civ = Common.GetCivilian(handle);
            var veh = Common.GetCivilianVeh(handle);

            // checking for existing civ
            if (civ == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(p)});
            }

            // checking if the vehicle exists
            if (veh == null) return new RequestData("veh_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var old = veh.ToArray();
            veh.Insured = !veh.Insured; // toggle insurance
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), veh.ToArray(), old});
        }
        #endregion
        #region Police Events
        public static RequestData AddOfficer(string handle, string callsign)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var ofc = Common.GetOfficer(handle);
            EventArgument[] old = null;

            // check for officer existing
            if (ofc == null)
            {
                ofc = new Officer(p.Identifiers["ip"], callsign);
                Officers.Add(ofc); // adding new officer
#if DEBUG
                Common.SendMessage(p, "", new[] {0, 0, 0}, "Creating new Officer profile...");
#endif
            }
            else
            {
                int index = Officers.IndexOf(ofc); // finding the index
                old = ofc.ToArray();
                ofc = new Officer(p.Identifiers["ip"], callsign);
                Officers[index] = ofc; // setting the index to the specified officer
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray(), old});
        }
        public static RequestData ToggleOnDuty(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var ofc = Common.GetOfficer(handle);

            // checking if the officer exists
            if (ofc == null)
                return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            if (ofc.Status == OfficerStatus.OnDuty)
            {
                return new RequestData("leo_status_prev", new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray()});
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.OnDuty; // setting status
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray(), old});
        }
        public static RequestData ToggleOffDuty(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var ofc = Common.GetOfficer(handle);

            // checking if the officer exists
            if (ofc == null)
                return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            if (ofc.Status == OfficerStatus.OffDuty)
            {
                return new RequestData("leo_status_prev", new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray()});
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.OffDuty; // setting status
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray(), old});
        }
        public static RequestData ToggleBusy(string handle)
        {
            Player p = Common.GetPlayerByHandle(handle);

            var ofc = Common.GetOfficer(handle);

            // checking if the officer exists
            if (ofc == null)
                return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            if (ofc.Status == OfficerStatus.Busy)
            {
                return new RequestData("leo_status_prev", new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray()});
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.Busy; // setting status
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), ofc.ToArray(), old});
        }
        public static RequestData AddCivilianNote(string invokerHandle, string first, string last, string note)
        {
            Player invoker = Common.GetPlayerByHandle(invokerHandle); // getting the invoker
            var civ = Common.GetCivilianByName(first, last); // finding the civ
            var ofc = Common.GetOfficer(invokerHandle);

            // checking leo
            if (ofc == null) return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(invoker)});
            // checking civ 
            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(invoker)});

            civ.Notes.Add(note); // adding the note to the civ
            return new RequestData(null, new EventArgument[]
            {
                Common.GetPlayerId(invoker),
                civ.ToArray(),
                note,
                civ.Notes.Select(x => (EventArgument)x).ToArray()
            });
        }
        public static RequestData TicketCivilian(string invokerHandle, string first, string last, string reason, float amount)
        {
            Player invoker = Common.GetPlayerByHandle(invokerHandle); // getting the invoker
            var civ = Common.GetCivilianByName(first, last); // finding the civ
            var ofc = Common.GetOfficer(invokerHandle);

            // checking for leo
            if (ofc == null) return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(invoker)});
            // checking for civ
            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {Common.GetPlayerId(invoker)});

            Player p = Common.GetPlayerByIp(civ.SourceIP); // finding the player that the civ owns
            civ.CitationCount++; // adding 1 to the citations
            Ticket ticket = new Ticket(reason, amount);
            civ.Tickets.Add(ticket); // adding a ticket to the existing tickets
            // msgs for the civs
            return new RequestData(null, new EventArgument[]
            {
                Common.GetPlayerId(p),
                civ.ToArray(),
                ticket.ToArray(),
                civ.Tickets.Select(x => (EventArgument)x.ToArray()).ToArray()
            });
        }
        public static RequestData AddBolo(string handle, string reason)
        {
            Player p = Common.GetPlayerByHandle(handle); // getting the invoker
            var ofc = Common.GetOfficer(handle);

            // checking for leo
            if (ofc == null) return new RequestData("leo_not_exist", new EventArgument[] {Common.GetPlayerId(p)});

            var bolo = new Bolo(p.Name, p.Identifiers["ip"], reason);// creating new bolo
            Bolos.Add(bolo); // adding teh bolos
            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(p), bolo.ToArray()});
        }
        #endregion
        #endregion
    }
}
