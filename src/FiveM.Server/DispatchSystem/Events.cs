using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;

using CloNET;
using Dispatch.Common.DataHolders;
using Dispatch.Common.DataHolders.Storage;
using DispatchSystem.Server.RequestHandling;
using static DispatchSystem.Server.Common;

namespace DispatchSystem.Server
{
    public partial class DispatchSystem
    {
        public static RequestData DispatchReset(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            int deletedProfiles = 0;

            if (GetCivilian(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new [] {0,0,0}, "Removing Civilian Profile...");
#endif
                Civs.Remove(GetCivilian(p.Handle)); // removing instance of civilian
                deletedProfiles++;
            }
            if (GetCivilianVeh(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Civilian Vehicle Profile...");
#endif
                CivVehs.Remove(GetCivilianVeh(p.Handle)); // removing instance of vehicle
                deletedProfiles++;
            }
            if (GetOfficer(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Officer Profile...");
#endif
                Officers.Remove(GetOfficer(p.Handle)); // removing instance of officer
                deletedProfiles++;
            }

            return new RequestData(null, new EventArgument[] {GetPlayerId(p), deletedProfiles});
        }

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
            Civilian civ = GetCivilian(handle); // finding the civ
            return civ != null ? new RequestData(null, civ.ToArray()) : new RequestData("civ_not_exist");
        }
        public static RequestData RequestCivilianVeh(string handle)
        {
            CivilianVeh civVeh = GetCivilianVeh(handle); // finding the vehicle
            return civVeh != null ? new RequestData(null, civVeh.ToArray()) : new RequestData("veh_not_exist");
        }
        public static RequestData RequestCivilianByName(string first, string last)
        {
            Civilian civ = GetCivilianByName(first, last); // finding the civ
            return civ != null ? new RequestData(null, civ.ToArray()) : new RequestData("civ_not_exist");
        }
        public static RequestData RequestCivilianVehByPlate(string plate)
        {
            CivilianVeh civVeh = GetCivilianVehByPlate(plate); // finding the vehicle
            return civVeh != null ? new RequestData(null, civVeh.ToArray()) : new RequestData("veh_not_exist");
        }
        public static RequestData RequestOfficer(string handle)
        {
            Officer ofc = GetOfficer(handle);
            return ofc != null ? new RequestData(null, ofc.ToArray()) : new RequestData("leo_not_exist");
        }
        public static RequestData RequestOfficerByCallsign(string callsign)
        {
            Officer ofc = Officers.First(x => x.Callsign == callsign);
            return ofc != null ? new RequestData(null, ofc.ToArray()) : new RequestData("leo_not_exist");
        }
        public static RequestData RequestOfficerAssignment(string handle)
        {
            Officer ofc = GetOfficer(handle);
            if (ofc == null)
                return new RequestData("leo_not_exist");
            Assignment a = OfcAssignments.ContainsKey(ofc) ? OfcAssignments[ofc] : null;
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
            Player p = GetPlayerByHandle(handle);

            if (GetCivilianByName(first, last) != null && GetPlayerByIp(GetCivilian(handle).SourceIP) != p) // checking if the name already exists in the system
            {
                return new RequestData("civ_name_exist", new EventArgument[] {GetPlayerId(p)});
            }
            if (GetCivilianVeh(handle) != null)
            {
                // below basically resets the vehicle if it exists
                int index = CivVehs.IndexOf(GetCivilianVeh(handle));
                CivVehs[index] = new CivilianVeh(p.Identifiers["ip"]);
            }

            Civilian civ;
            EventArgument[] old = null;
            // checking if the civilian already has a civ in the system
            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // finding the index of the existing civ

                civ = new Civilian(p.Identifiers["ip"]) { First = first, Last = last }; // setting the index to an instance of a new civilian
                old = Civs[index].ToArray();
                Civs[index] = civ;
            }
            else // if the civ doesn't exist
            {
                civ = new Civilian(p.Identifiers["ip"]) {First = first, Last = last};
                Civs.Add(civ); // add a new civilian to the system

#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new civilian profile...");
#endif
            }

            return new RequestData(null, new EventArgument[] {GetPlayerId(p), civ.ToArray(), old});
        }
        public static RequestData ToggleWarrant(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null) return new RequestData("civ_not_exist", new EventArgument[] {GetPlayerId(p)});

            int index = Civs.IndexOf(GetCivilian(handle)); // finding the index
            var old = Civs[index].ToArray();
            Civs[index].WarrantStatus =
                !Civs[index].WarrantStatus; // setting the warrant status of the opposite of before
            return new RequestData(null, new EventArgument[] {GetPlayerId(p), Civs[index].ToArray(), old});
        }
        public static RequestData SetCitations(string handle, int count)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null) return new RequestData("civ_not_exist", new EventArgument[] {GetPlayerId(p)});

            int index = Civs.IndexOf(GetCivilian(handle)); // again finding index
            var old = Civs[index].ToArray();
            Civs[index].CitationCount = count; // setting the count of the citations

            return new RequestData(null, new EventArgument[] {GetPlayerId(p), Civs[index].ToArray(), old});
        }
        public static async Task<RequestData> InitializeEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for an existing emergency
            if (GetEmergencyCall(handle) != null)
            {
                return new RequestData("civ_911_exist", new EventArgument[] {GetPlayerId(p)});
            }

            if (GetCivilian(handle) == null) return new RequestData("civ_not_exist", new EventArgument[] {GetPlayerId(p)});

            Civilian civ = GetCivilian(handle); // finding the civ
            // checking how many active dispatchers there are
            if (Server.ConnectedDispatchers.Length == 0)
            {
                return new RequestData("civ_911_no_dispatchers", new EventArgument[] {GetPlayerId(p)});
            }

            EmergencyCall call;
            CurrentCalls.Add(call =
                new EmergencyCall(p.Identifiers["ip"],
                    $"{civ.First} {civ.Last}")); // adding and creating the instance of the emergency
            foreach (var peer in Server.ConnectedDispatchers)
            {
                await peer.RemoteCallbacks.Events["911alert"]
                    .Invoke(civ, call); // notifying the dispatchers of the 911 call
            }
            return new RequestData(null, new EventArgument[] {GetPlayerId(p)});
        }
        public static async Task<RequestData> MessageEmergency(string handle, string msg)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            if (call?.Accepted ?? true) // checking if null and if accepted in the same check
            {
                return new RequestData("civ_911_not_exist", new EventArgument[] { GetPlayerId(p) });
            }

            string dispatcherIp = Server.Calls[call.Id]; // getting the dispatcher ip from the calls
            ConnectedPeer peer = Server.ConnectedDispatchers.First(x => x.RemoteIP == dispatcherIp); // finding the peer from the given IP
            await peer.RemoteCallbacks.Events[call.Id.ToString()].Invoke(msg); // invoking the remote event for the message in the peer

            return new RequestData(null, new EventArgument[] { GetPlayerId(p), msg});
        }
        public static async Task<RequestData> EndEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            // checking for call null
            if (call == null)
            {
                return new RequestData("civ_911_not_exist", new EventArgument[] { GetPlayerId(p) });
            }

#if DEBUG
            SendMessage(p, "", new[] { 0, 0, 0 }, "Ending 911 call...");
#endif

            // finding the dispatcher ip
            string dispatcherIp = Server.Calls.ContainsKey(call.Id) ? Server.Calls[call.Id] : null;
            ConnectedPeer peer = Server.ConnectedDispatchers.FirstOrDefault(x => x.RemoteIP == dispatcherIp); // finding the peer from the ip
            var task = peer?.RemoteCallbacks.Events?["end" + call.Id].Invoke(); // creating the task from the events

            // removing the call from the calls list
            CurrentCalls.Remove(call);

            if (!(task is null)) await task; // await the task

            return new RequestData(null, new EventArgument[] { GetPlayerId(p) });
        }
        #endregion

        #region Vehicle Events
        public static RequestData SetVehicle(string handle, string plate)
        {
            Player p = GetPlayerByHandle(handle);

            // if no civilian exists
            if (GetCivilian(handle) == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] { GetPlayerId(p) });
            }
            // checking if the plate already exists in the system
            if (GetCivilianVehByPlate(plate) != null && GetPlayerByIp(GetCivilianVeh(handle).SourceIP) != p)
            {
                return new RequestData("veh_plate_exist", new EventArgument[] { GetPlayerId(p), plate});
            }

            CivilianVeh veh = GetCivilianVeh(handle);
            EventArgument[] old = null;
            // checking if player already owns a vehicle
            if (veh != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the existing index
                old = CivVehs[index].ToArray();
                // setting the index to a new vehicle item
                veh = new CivilianVeh(p.Identifiers["ip"]) {Plate = plate, Owner = GetCivilian(handle)};
                CivVehs[index] = veh;
            }
            else
            {
                veh = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = GetCivilian(handle) }; // creating the new vehicle
                CivVehs.Add(veh); // adding the new vehicle to the list of vehicles
            }

            return new RequestData(null, new EventArgument[] { GetPlayerId(p), veh.ToArray(), old });
        }
        public static RequestData ToggleVehicleStolen(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            
            // checking if player has a name
            if (GetCivilian(handle) == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] { GetPlayerId(p) });
            }

            // checking if vehicle exists
            if (GetCivilianVeh(handle) == null) return new RequestData("veh_not_exist", new EventArgument[] {GetPlayerId(p)});

            int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding index of vehicle
            var old = CivVehs[index].ToArray();
            CivVehs[index].StolenStatus = !CivVehs[index].StolenStatus; // toggle stolen

            if (CivVehs[index].StolenStatus) // checking if it is stolen
            {
                Civilian civ = Civilian.CreateRandomCivilian(); // creating a new random civ
                CivVehs[index].Owner = civ; // setting the vehicle owner to the civ
                Civs.Add(civ); // adding the civ to the database
            }
            else
            {
                Civilian civ = CivVehs[index].Owner; // finding the existing civ
                Civs.Remove(civ); // removing the civ from the database
                CivVehs[index].Owner = GetCivilian(handle); // setting the owner to the person
            }

            return new RequestData(null, new EventArgument[] { GetPlayerId(p), CivVehs[index].ToArray(), old});
        }
        public static RequestData ToggleVehicleRegistration(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] { GetPlayerId(p) });
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) == null) return new RequestData("veh_not_exist", new EventArgument[] {GetPlayerId(p)});

            int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index of the vehicle
            var old = CivVehs[index].ToArray();
            CivVehs[index].Registered = !CivVehs[index].Registered; // setting the registration
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), CivVehs[index].ToArray(), old});
        }
        public static RequestData ToggleVehicleInsurance(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                return new RequestData("civ_not_exist", new EventArgument[] { GetPlayerId(p) });
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) == null) return new RequestData("veh_not_exist", new EventArgument[] {GetPlayerId(p)});

            int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index
            var old = CivVehs[index].ToArray();
            CivVehs[index].Insured = !CivVehs[index].Insured; // toggle insurance
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), CivVehs[index].ToArray(), old});
        }
        #endregion

        #region Police Events
        public static RequestData AddOfficer(string handle, string callsign)
        {
            Player p = GetPlayerByHandle(handle);

            Officer ofc = GetOfficer(handle);
            EventArgument[] old = null;

            // check for officer existing
            if (ofc == null)
            {
                ofc = new Officer(p.Identifiers["ip"], callsign);
                Officers.Add(ofc); // adding new officer
#if DEBUG
                SendMessage(p, "", new[] {0, 0, 0}, "Creating new Officer profile...");
#endif
            }
            else
            {
                int index = Officers.IndexOf(GetOfficer(handle)); // finding the index
                old = Officers[index].ToArray();
                ofc = new Officer(p.Identifiers["ip"], callsign);
                Officers[index] = ofc; // setting the index to the specified officer
            }

            return new RequestData(null, new EventArgument[] {GetPlayerId(p), ofc.ToArray(), old});
        }
        public static RequestData ToggleOnDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if the officer exists
            if (GetOfficer(handle) == null)
                return new RequestData("leo_not_exist", new EventArgument[] {GetPlayerId(p)});

            Officer ofc = GetOfficer(handle); // finding the officer

            if (ofc.Status == OfficerStatus.OnDuty)
            {
                return new RequestData("leo_status_prev", new EventArgument[] { GetPlayerId(p), ofc.ToArray() });
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.OnDuty; // setting status
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), ofc.ToArray(), old });
        }
        public static RequestData ToggleOffDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if the officer exists
            if (GetOfficer(handle) == null)
                return new RequestData("leo_not_exist", new EventArgument[] { GetPlayerId(p) });

            Officer ofc = GetOfficer(handle); // finding the officer

            if (ofc.Status == OfficerStatus.OffDuty)
            {
                return new RequestData("leo_status_prev", new EventArgument[] { GetPlayerId(p), ofc.ToArray() });
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.OffDuty; // setting status
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), ofc.ToArray(), old });
        }
        public static RequestData ToggleBusy(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if the officer exists
            if (GetOfficer(handle) == null)
                return new RequestData("leo_not_exist", new EventArgument[] { GetPlayerId(p) });

            Officer ofc = GetOfficer(handle); // finding the officer

            if (ofc.Status == OfficerStatus.Busy)
            {
                return new RequestData("leo_status_prev", new EventArgument[] { GetPlayerId(p), ofc.ToArray() });
            }

            var old = ofc.ToArray();
            ofc.Status = OfficerStatus.Busy; // setting status
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), ofc.ToArray(), old });
        }
        public static RequestData AddCivilianNote(string invokerHandle, string first, string last, string note)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking civ 
            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] { GetPlayerId(invoker) });

            int index = Civs.IndexOf(civ); // finding the index
            Civs[index].Notes.Add(note); // adding the note to the civ
            return new RequestData(null, new EventArgument[]
            {
                GetPlayerId(invoker),
                civ.ToArray(),
                note,
                civ.Notes.Select(x => (EventArgument)x).ToArray()
            });
        }
        public static RequestData TicketCivilian(string invokerHandle, string first, string last, string reason, float amount)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking for civ
            if (civ == null) return new RequestData("civ_not_exist", new EventArgument[] {GetPlayerId(invoker)});

            int index = Civs.IndexOf(civ); // finding the index of the civ
            Player p = GetPlayerByIp(Civs[index].SourceIP); // finding the player that the civ owns
            Civs[index].CitationCount++; // adding 1 to the citations
            Ticket ticket = new Ticket(reason, amount);
            Civs[index].Tickets.Add(ticket); // adding a ticket to the existing tickets
            // msgs for the civs
            return new RequestData(null, new EventArgument[]
            {
                GetPlayerId(p),
                civ.ToArray(),
                ticket.ToArray(),
                civ.Tickets.Select(x => (EventArgument)x.ToArray()).ToArray()
            });
        }
        public static RequestData AddBolo(string handle, string reason)
        {
            Player p = GetPlayerByHandle(handle); // getting the invoker
            var bolo = new Bolo(p.Name, p.Identifiers["ip"], reason);// creating new bolo
            Bolos.Add(bolo); // adding teh bolos
            return new RequestData(null, new EventArgument[] { GetPlayerId(p), bolo.ToArray() });
        }
        #endregion
    }
}
