using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;

using CloNET;
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

            return new RequestData(p, null, new object[] {deletedProfiles});
        }
        public static RequestData PushClientInfo(string handle)
        {
            Player p = GetPlayerByHandle(handle); // find player from handle
            var civ = GetCivilian(handle); // find civ from handle
            var civVeh = GetCivilianVeh(handle); // find veh from handle
            var ofc = GetOfficer(handle); // find ofc from handle

            // set all of the civ arr for event
            string[] civArr =
            {
                civ?.First,
                civ?.Last,
                civ?.CitationCount.ToString(),
                civ?.WarrantStatus.ToString(),
            };
            string[] vehArr =
            {
                civVeh?.Plate,
                civVeh?.StolenStatus.ToString(),
                civVeh?.Registered.ToString(),
                civVeh?.Insured.ToString()
            };
            // set all of the ofc arr for event
            string[] ofcArr =
            {
                ofc?.Callsign,
                ofc?.Status.GetHashCode().ToString(),
                ofc == null ? null : OfcAssignments.ContainsKey(ofc) ? OfcAssignments[ofc].Summary : null
            };

            return new RequestData(p, null, new object[] {civArr, vehArr, ofcArr});
        }

        #region Civilian Events
        public static RequestData DisplayCurrentCivilian(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            Civilian civ = GetCivilian(handle);

            if (civ != null)
            {
                return new RequestData(p, null, new object[]
                {
                    civ.First, civ.Last, civ.WarrantStatus, civ.CitationCount, civ.Notes.ToArray(),
                    civ.Tickets.ToArray()
                });
            }
            return new RequestData(p, "civ_not_exist");
        }
        public static RequestData SetName(string handle, string first, string last)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilianByName(first, last) != null && GetPlayerByIp(GetCivilian(handle).SourceIP) != p) // checking if the name already exists in the system
            {
                return new RequestData(p, "civ_name_exist");
            }
            if (GetCivilianVeh(handle) != null)
            {
                // below basically resets the vehicle if it exists
                int index = CivVehs.IndexOf(GetCivilianVeh(handle));
                CivVehs[index] = new CivilianVeh(p.Identifiers["ip"]);
            }

            Civilian civ;
            // checking if the civilian already has a civ in the system
            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // finding the index of the existing civ

                civ = new Civilian(p.Identifiers["ip"]) { First = first, Last = last }; // setting the index to an instance of a new civilian
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

            return new RequestData(p, null, new object[] {civ.First, civ.Last});
        }
        public static RequestData ToggleWarrant(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // finding the index
                Civs[index].WarrantStatus =
                    !Civs[index].WarrantStatus; // setting the warrant status of the opposite of before
                return new RequestData(p, null, new object[] {Civs[index].WarrantStatus});
            }
            return new RequestData(p, "civ_not_exist");
        }
        public static RequestData SetCitations(string handle, int count)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // again finding index
                Civs[index].CitationCount = count; // setting the count of the citations

                return new RequestData(p, null, new object[] {Civs[index].CitationCount});
            }
            return new RequestData(p, "civ_not_exist");
        }
        public static async Task<RequestData> InitializeEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for an existing emergency
            if (GetEmergencyCall(handle) != null)
            {
                return new RequestData(p, "civ_911_exist");
            }

            if (GetCivilian(handle) != null)
            {
                Civilian civ = GetCivilian(handle); // finding the civ

                // checking how many active dispatchers there are
                if (Server.ConnectedDispatchers.Length == 0)
                {
                    return new RequestData(p, "civ_911_no_dispatchers");
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
                return new RequestData(p, null);
            }
            return new RequestData(p, "civ_not_exist");
        }
        public static async Task<RequestData> MessageEmergency(string handle, string msg)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            if (call?.Accepted ?? true) // checking if null and if accepted in the same check
            {
                return new RequestData(p, "civ_911_not_exist");
            }

            string dispatcherIp = Server.Calls[call.Id]; // getting the dispatcher ip from the calls
            ConnectedPeer peer = Server.ConnectedDispatchers.First(x => x.RemoteIP == dispatcherIp); // finding the peer from the given IP
            await peer.RemoteCallbacks.Events[call.Id.ToString()].Invoke(msg); // invoking the remote event for the message in the peer

            return new RequestData(p, null, new object[] {msg});
        }
        public static async Task<RequestData> EndEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            // checking for call null
            if (call == null)
            {
                return new RequestData(p, "civ_911_not_exist");
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

            return new RequestData(p, null);
        }
        #endregion

        #region Vehicle Events
        public static RequestData DisplayCurrentVehicle(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            CivilianVeh veh = GetCivilianVeh(handle);

            if (veh != null)
            {
                return new RequestData(p, null,
                    new object[] {veh.Plate, veh.Owner.First, veh.Owner.Last, veh.Registered, veh.Insured});
            }

            return new RequestData(p, "veh_not_exist");
        }
        public static RequestData SetVehicle(string handle, string plate)
        {
            Player p = GetPlayerByHandle(handle);

            // if no civilian exists
            if (GetCivilian(handle) == null)
            {
                return new RequestData(p, "civ_not_exist");
            }
            // checking if the plate already exists in the system
            if (GetCivilianVehByPlate(plate) != null && GetPlayerByIp(GetCivilianVeh(handle).SourceIP) != p)
            {
                return new RequestData(p, "veh_plate_exist", new object[] {plate});
            }

            // checking if player already owns a vehicle
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the existing index
                CivVehs[index] = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = GetCivilian(handle) }; // setting the index to a new vehicle item
                return new RequestData(p, null, new object[] {CivVehs[index].Plate});
            }

            CivilianVeh veh = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = GetCivilian(handle) }; // creating the new vehicle
            CivVehs.Add(veh); // adding the new vehicle to the list of vehicles
            return new RequestData(p, null, new object[] {veh.Plate});
        }
        public static RequestData ToggleVehicleStolen(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            
            // checking if player has a name
            if (GetCivilian(handle) == null)
            {
                return new RequestData(p, "civ_not_exist");
            }

            // checking if vehicle exists
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding index of vehicle
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

                return new RequestData(p, null, new object[] {CivVehs[index].StolenStatus});
            }

            return new RequestData(p, "veh_not_exist");
        }
        public static RequestData ToggleVehicleRegistration(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                return new RequestData(p, "civ_not_exist");
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index of the vehicle
                CivVehs[index].Registered = !CivVehs[index].Registered; // setting the registration
                return new RequestData(p, null, new object[] {CivVehs[index].Registered});
            }

            return new RequestData(p, "veh_not_exist");
        }
        public static RequestData ToggleVehicleInsurance(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                return new RequestData(p, "civ_not_exist");
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index
                CivVehs[index].Insured = !CivVehs[index].Insured; // toggle insurance
                return new RequestData(p, null, new object[] {CivVehs[index].Insured});
            }

            return new RequestData(p, "veh_not_exist");
        }
        #endregion

        #region Police Events
        public static RequestData AddOfficer(string handle, string callsign)
        {
            Player p = GetPlayerByHandle(handle);

            // check for officer existing
            if (GetOfficer(handle) == null)
            {
                Officer ofc = new Officer(p.Identifiers["ip"], callsign);
                Officers.Add(ofc); // adding new officer
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new Officer profile...");
#endif
                return new RequestData(p, null, new object[] {ofc.Callsign, ofc.Status.GetHashCode()});
            }

            int index = Officers.IndexOf(GetOfficer(handle)); // finding the index
            Officers[index] = new Officer(p.Identifiers["ip"], callsign); // setting the index to the specified officer
            return new RequestData(p, null, new object[] {Officers[index].Callsign, Officers[index].Status.GetHashCode()});
        }
        public static RequestData DisplayStatus(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if officer exists
            if (GetOfficer(handle) == null)
                return new RequestData(p, "leo_not_exist");

            Officer ofc = GetOfficer(handle); // finding the officer
            return new RequestData(p, null, new object[] { ofc.Status.GetHashCode() });
        }
        public static RequestData ToggleOnDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if the officer exists
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // finding the officer

                if (ofc.Status == OfficerStatus.OnDuty)
                {
                    return new RequestData(p, "leo_status_prev", new object[] { ofc.Status.GetHashCode() });
                }

                ofc.Status = OfficerStatus.OnDuty; // setting status
                return new RequestData(p, null, new object[] { ofc.Status.GetHashCode() });
            }

            return new RequestData(p, "leo_not_exist");
        }
        public static RequestData ToggleOffDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if officer exists
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // get the officer

                if (ofc.Status == OfficerStatus.OffDuty)
                {
                    return new RequestData(p, "leo_status_prev", new object[] { ofc.Status.GetHashCode() });
                }

                ofc.Status = OfficerStatus.OffDuty; // setting the status
                return new RequestData(p, null, new object[] { ofc.Status.GetHashCode() });
            }

            return new RequestData(p, "leo_not_exist");
        }
        public static RequestData ToggleBusy(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // send back if the officer doesn't exist
            if (GetOfficer(handle) == null) return new RequestData(p, "leo_not_exist");

            Officer ofc = GetOfficer(handle); // finding the officer

            if (ofc.Status == OfficerStatus.Busy)
            {
                return new RequestData(p, "leo_status_prev", new object[] { ofc.Status.GetHashCode() });
            }

            ofc.Status = OfficerStatus.Busy; // setting the status
            return new RequestData(p, null, new object[] { ofc.Status.GetHashCode() });
        }
        public static RequestData RequestCivilian(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking if the civ is not null
            if (civ != null)
            {
                // results msg
                return new RequestData(invoker, null, new object[]
                {
                    civ.First,
                    civ.Last,
                    civ.WarrantStatus,
                    civ.CitationCount,
                    civ.Notes.ToArray(),
                    civ.Tickets.Select(x => new object[] {x.Amount, x.Reason}).ToArray()
                });
            }

            return new RequestData(invoker, "civ_not_exist");
        }
        public static RequestData RequestCivilianVeh(string handle, string plate)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            CivilianVeh civVeh = GetCivilianVehByPlate(plate); // finding the vehicle

            if (civVeh != null)
            {
                // results msg
                return new RequestData(invoker, null, new object[]
                {
                    civVeh.Plate,
                    civVeh.Owner.First,
                    civVeh.Owner.Last,
                    civVeh.StolenStatus,
                    civVeh.Registered,
                    civVeh.Insured
                });
            }

            return new RequestData(invoker, "veh_not_exist");
        }
        public static RequestData AddCivilianNote(string invokerHandle, string first, string last, string note)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking civ 
            if (civ == null) return new RequestData(invoker, "civ_not_exist");

            int index = Civs.IndexOf(civ); // finding the index
            Civs[index].Notes.Add(note); // adding the note to the civ
            return new RequestData(invoker, null, new object[]
            {
                civ.First,
                civ.Last,
                note,
                civ.Notes.ToArray()
            });
        }
        public static RequestData TicketCivilian(string invokerHandle, string first, string last, string reason, float amount)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking for civ
            if (civ != null)
            {
                int index = Civs.IndexOf(civ); // finding the index of the civ
                Player p = GetPlayerByIp(Civs[index].SourceIP); // finding the player that the civ owns
                Civs[index].CitationCount++; // adding 1 to the citations
                Ticket ticket = new Ticket(reason, amount);
                Civs[index].Tickets.Add(ticket); // adding a ticket to the existing tickets
                // msgs for the civs
                return new RequestData(p, null, new object[]
                {
                    civ.First,
                    civ.Last,
                    new object[]
                    {
                        ticket.Amount,
                        ticket.Reason
                    },
                    civ.Tickets.Select(x => new object[] {x.Amount, x.Reason}).ToArray()
                });
            }

            return new RequestData(invoker, "civ_not_exist");
        }
        public static RequestData DisplayCivilianTickets(string invokerHandle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking for civ
            if (civ != null)
            {
                return new RequestData(invoker, null, new object[]
                {
                    civ.First,
                    civ.Last,
                    civ.Tickets.Select(x => new object[] {x.Amount, x.Reason}).ToArray()
                });
            }
            return new RequestData(invoker, "civ_not_exist");
        }
        public static RequestData DisplayCivilianNotes(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            if (civ != null)
            {
                // sending the msgs
                return new RequestData(invoker, null, new object[]
                {
                    civ.First,
                    civ.Last,
                    civ.Notes.ToArray()
                });
            }

            return new RequestData(invoker, "civ_not_exist");
        }
        public static RequestData AddBolo(string handle, string reason)
        {
            Player p = GetPlayerByHandle(handle); // getting the invoker
            Bolos.Add(new Bolo(p.Name, p.Identifiers["ip"], reason)); // adding teh bolos
            return new RequestData(p, null, new object[] {reason});
        }
        public static RequestData ViewBolos(string handle)
        {
            Player p = GetPlayerByHandle(handle); // getting the invoker
            return new RequestData(p, null, new object[]
            {
                Bolos.Select(x => new object[] {x.Player, x.Reason})
            });
        }
        #endregion
    }
}
