using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;

using CitizenFX.Core;

using CloNET;
using DispatchSystem.Common;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.sv.External;

using static DispatchSystem.sv.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
        #region Custom Events
        public static void DispatchReset(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new [] {0,0,0}, "Removing Civilian Profile...");
#endif
                Civs.Remove(GetCivilian(p.Handle)); // removing instance of civilian
            }
            if (GetCivilianVeh(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Civilian Vehicle Profile...");
#endif
                CivVehs.Remove(GetCivilianVeh(p.Handle)); // removing instance of vehicle
            }
            if (GetOfficer(p.Handle) != null)
            {
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Removing Officer Profile...");
#endif
                Officers.Remove(GetOfficer(p.Handle)); // removing instance of officer
            }

            SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "All profiles reset"); // displaying the reset message
        }

        #region Civilian Events
        public static void DisplayCurrentCivilian(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            Civilian civ = GetCivilian(handle);

            if (civ != null)
            {
                SendMessage(p, "", new[] { 255, 255, 255 }, $"First: {civ.First} | Last: {civ.Last}");
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Warrant: {civ.WarrantStatus}");
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Citations: {civ.CitationCount}");
            }
            else
                SendMessage(p, "DispatchSystem", new [] {0,0,0}, "You don't exist in the system");
        }
        public static void SetName(string handle, string first, string last)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetOfficer(handle) != null) // checking if the civilian has an officer
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You cannot be an officer and a civilian at the same time!");
                return; // return if they do
            }

            if (GetCivilianByName(first, last) != null && GetPlayerByIp(GetCivilianVeh(handle).SourceIP) != p) // checking if the name already exists in the system
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "That name already exists in the system!");
                return; // return if it does
            }

            // checking if the civilian already has a civ in the system
            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // finding the index of the existing civ

                Civs[index] = new Civilian(p.Identifiers["ip"]) { First = first, Last = last }; // setting the index to an instance of a new civilian

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {Civs[index].First} {Civs[index].Last}"); // saying the new name created
            }
            else // if the civ doesn't exist
            {
                Civs.Add(new Civilian(p.Identifiers["ip"]) { First = first, Last = last }); // add a new civilian to the system
                int index = Civs.IndexOf(GetCivilian(handle)); // find the index of the civ

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {Civs[index].First} {Civs[index].Last}"); // say the new name was created
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new civilian profile...");
#endif
            }
            if (GetCivilianVeh(handle) != null)
            {
                // below basically resets the vehicle if it exists

                int index = CivVehs.IndexOf(GetCivilianVeh(handle));

                CivVehs[index] = new CivilianVeh(p.Identifiers["ip"]);
            }
        }
        public static void ToggleWarrant(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // finding the index
                Civs[index].WarrantStatus = !Civs[index].WarrantStatus; // setting the warrant status of the opposite of before
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Warrant status set to {Civs[index].WarrantStatus}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can toggle your warrant");
        }
        public static void SetCitations(string handle, int count)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = Civs.IndexOf(GetCivilian(handle)); // again finding index
                Civs[index].CitationCount = count; // setting the count of the citations

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Citation count set to {count}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your citations");
        }
        public static async void InitializeEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for an existing emergency
            if (GetEmergencyCall(handle) != null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You already have a 911 call!");
                return;
            }

            if (GetCivilian(handle) != null)
            {
                Civilian civ = GetCivilian(handle); // finding the civ
                
                // checking how many active dispatchers there are
                if (Server.ConnectedDispatchers.Length == 0)
                {
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "It seems like there is no connected dispatchers at this moment!");
                    return;
                }

                EmergencyCall call;
                CurrentCalls.Add(call = new EmergencyCall(p.Identifiers["ip"], $"{civ.First} {civ.Last}")); // adding and creating the instance of the emergency
                SendMessage(p, "Dispatch911", new[] { 255, 0, 0 }, "Please wait for a dispatcher to respond"); // msging to wait for a dispatcher
                foreach (var peer in Server.ConnectedDispatchers)
                {
                    await peer.RemoteCallbacks.Events["911alert"].Invoke(civ, call); // notifying the dispatchers of the 911 call
                }
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before start a 911 call");
        }
        public static async void MessageEmergency(string handle, string msg)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            if (call?.Accepted ?? true) // checking if null and if accepted in the same check
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You're call must be answered or started first");
                return;
            }

            string dispatcherIp = Server.Calls[call.Id]; // getting the dispatcher ip from the calls
            ConnectedPeer peer = Server.ConnectedDispatchers.First(x => x.RemoteIP == dispatcherIp); // finding the peer from the given IP
            await peer.RemoteCallbacks.Events[call.Id.ToString()].Invoke(msg); // invoking the remote event for the message in the peer
        }
        public static async void EndEmergency(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            EmergencyCall call = GetEmergencyCall(handle);
            // checking for call null
            if (call == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "There is no 911 call to end!");
                return;
            }

#if DEBUG
            SendMessage(p, "", new[] { 0, 0, 0 }, "Ending 911 call...");
#endif

            // finding the dispatcher ip
            var dispatcherIp = Server.Calls[call.Id];
            ConnectedPeer peer = Server.ConnectedDispatchers.FirstOrDefault(x => x.RemoteIP == dispatcherIp); // finding the peer from the ip
            var task = peer?.RemoteCallbacks.Events?["end" + call.Id].Invoke(); // creating the task from the events

            // removing the call from the calls list
            CurrentCalls.Remove(call);
            SendMessage(p, "Dispatch911", new[] { 255, 0, 0 }, "Ended the 911 call");

            if (!(task is null)) await task; // await the task
        }
        #endregion

        #region Vehicle Events
        public static void DisplayCurrentVehicle(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            CivilianVeh veh = GetCivilianVeh(handle);

            if (veh != null)
            {
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Plate: {veh.Plate.ToUpper()}");
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Stolen: {veh.StolenStatus}");
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Registered: {veh.Registered}");
                SendMessage(p, "", new[] { 255, 255, 255 }, $"Insured: {veh.Insured}");
            }
            else
                SendMessage(p, "DispatchSystem", new [] {0,0,0}, "Your vehicle doesn't exist in the system");
        }
        public static void SetVehicle(string handle, string plate)
        {
            Player p = GetPlayerByHandle(handle);

            // if no civilian exists
            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle");
                return;
            }

            // checking if the plate already exists in the system
            if (GetCivilianVehByPlate(plate) != null && GetPlayerByIp(GetCivilianVeh(handle).SourceIP) != p)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "That vehicle already exists in the system!");
                return;
            }

            // checking if player already owns a vehicle
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the existing index
                CivVehs[index] = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = GetCivilian(handle) }; // setting the index to a new vehicle item
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New vehicle set to {CivVehs[index].Plate}"); // msg of creation
            }
            else
            {
                CivilianVeh veh = new CivilianVeh(p.Identifiers["ip"]) { Plate = plate, Owner = GetCivilian(handle) }; // creating the new vehicle
                CivVehs.Add(veh); // adding the new vehicle to the list of vehicles

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New vehicle set to {veh.Plate}"); // msg
            }
        }
        public static void ToggleVehicleStolen(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            
            // checking if player has a name
            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle stolen");
                return;
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


                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Stolen status set to {CivVehs[index].StolenStatus}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your vehicle stolen");
        }
        public static void ToggleVehicleRegistration(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle registration");
                return;
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index of the vehicle
                CivVehs[index].Registered = !CivVehs[index].Registered; // setting the registration

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Registration status set to {CivVehs[index].Registered}"); // msg
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your Regisration");
        }
        public static void ToggleVehicleInsurance(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking for existing civ
            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle insurance");
                return;
            }

            // checking if the vehicle exists
            if (GetCivilianVeh(handle) != null)
            {
                int index = CivVehs.IndexOf(GetCivilianVeh(handle)); // finding the index
                CivVehs[index].Insured = !CivVehs[index].Insured; // toggle insurance

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Insurance status set to {CivVehs[index].Insured}"); // msg
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your Insurance");
        }
        #endregion

        #region Police Events
        public static void AddOfficer(string handle, string callsign)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if civ exists
            if (GetCivilian(handle) != null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 },
                    "You cannot be a officer and a civilian at the same time.");
                return;
            }

            // check for officer existing
            if (GetOfficer(handle) == null)
            {
                Officers.Add(new Officer(p.Identifiers["ip"], callsign)); // adding new officer
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Assigning new officer for callsign {callsign}"); // msg
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new Officer profile...");
#endif
            }
            else
            {
                int index = Officers.IndexOf(GetOfficer(handle)); // finding the index
                Officers[index] = new Officer(p.Identifiers["ip"], callsign); // setting the index to the specified officer
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Changing your callsign to {callsign}"); // msg
            }
        }
        public static void DisplayStatus(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if officer exists
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // finding the officer

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 },
                    $"Your status is: {(ofc.Status == OfficerStatus.OffDuty ? "Off Duty" : ofc.Status == OfficerStatus.OnDuty ? "On Duty" : "Busy")}"); // msg (with 2 ?: statements)
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must create an officer first");
        }
        public static void ToggleOnDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if the officer exists
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // finding the officer

                if (ofc.Status == OfficerStatus.OnDuty)
                {
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You are already on duty dummy!"); // msg if already onduty
                    return;
                }

                ofc.Status = OfficerStatus.OnDuty; // setting status
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "New officer status set to On Duty"); // msg
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must create an officer first");
        }
        public static void ToggleOffDuty(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // checking if officer exists
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // get the officer

                if (ofc.Status == OfficerStatus.OffDuty)
                {
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You are already off duty dummy!"); // msg if already offduty
                    return;
                }

                ofc.Status = OfficerStatus.OffDuty; // setting the status
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "New officer status set to Off Duty"); // msg
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must create an officer first");
        }
        public static void ToggleBusy(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            // trying to find the officer
            if (GetOfficer(handle) != null)
            {
                Officer ofc = GetOfficer(handle); // finding the officer

                if (ofc.Status == OfficerStatus.Busy)
                {
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You are already busy dummy!"); // msg if already busy
                    return;
                }

                ofc.Status = OfficerStatus.Busy; // setting the status
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "New officer status set to Busy"); // msg
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must create an officer first");
        }
        public static void RequestCivilian(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking if the civ is not null
            if (civ != null)
            {
                // results msg
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"First: {civ.First} | Last: {civ.Last}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Warrant: {civ.WarrantStatus}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Citations: {civ.CitationCount}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That civilian doesn't exist in the system");
        }
        public static void RequestCivilianVeh(string handle, string plate)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            CivilianVeh civVeh = GetCivilianVehByPlate(plate); // finding the vehicle

            if (civVeh != null)
            {
                // results msg
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Plate: {civVeh.Plate.ToUpper()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Stolen: {civVeh.StolenStatus}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Registered: {civVeh.Registered}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Insured: {civVeh.Insured}");
                // for registration
                if (civVeh.Registered)
                    SendMessage(invoker, "DispatchSystem", new[] {0, 0, 0},
                        $"R/O: {civVeh.Owner.First} {civVeh.Owner.Last}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That vehicle doesn't exist in the system");
        }
        public static void AddCivilianNote(string invokerHandle, string first, string last, string note)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking civ 
            if (civ != null)
            {
                int index = Civs.IndexOf(civ); // finding the index
                Civs[index].Notes.Add(note); // adding the note to the civ
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Note of \"{note}\" has been added to the Civilian"); // msg
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void TicketCivilian(string invokerHandle, string first, string last, string reason, float amount)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking for civ
            if (civ != null)
            {
                int index = Civs.IndexOf(civ); // finding the index of the civ
                Player p = GetPlayerByIp(Civs[index].SourceIP); // finding the player that the civ owns
                Civs[index].CitationCount++; // adding 1 to the citations
                Civs[index].Tickets.Add(new Ticket(reason, amount)); // adding a ticket to the existing tickets
                // msgs for the civs
                if (p != null)
                    SendMessage(p, "Ticket", new[] {255, 0, 0},
                        $"{invoker.Name} tickets you for ${amount.ToString(CultureInfo.InvariantCulture)} because of {reason}");
                SendMessage(invoker, "DispatchSystem", new[] {0, 0, 0},
                    $"You successfully ticketed {p?.Name ?? "NULL"} for ${amount.ToString(CultureInfo.InvariantCulture)}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void DisplayCivilianTickets(string invokerHandle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(invokerHandle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            // checking for civ
            if (civ != null)
            {
                int index = Civs.IndexOf(civ); // finding the index of the civ
                // msgs if any tickets
                if (!Civs[index].Tickets.Any())
                    SendMessage(invoker, "", new[] { 0, 0, 0 }, "^7None");
                else
                    Civs[index].Tickets.ForEach(x => SendMessage(invoker, "", new[] { 0, 0, 0 }, $"^7${x.Amount}: {x.Reason}")); // big msgs
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void DipslayCivilianNotes(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle); // getting the invoker
            Civilian civ = GetCivilianByName(first, last); // finding the civ

            if (civ != null)
            {
                // sending the msgs
                if (!civ.Notes.Any())
                    SendMessage(invoker, "", new[] { 0, 0, 0 }, "^7None");
                else
                    civ.Notes.ForEach(x => SendMessage(invoker, "", new[] { 0, 0, 0 }, x)); // big msgs
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void AddBolo(string handle, string reason)
        {
            Player p = GetPlayerByHandle(handle); // getting the invoker
            Bolos.Add(new Bolo(p.Name, p.Identifiers["ip"], reason)); // adding teh bolos
            SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"BOLO for \"{reason}\" added"); // msg
        }
        public static void ViewBolos(string handle)
        {
            Player p = GetPlayerByHandle(handle); // getting the invoker
            // checking for bolos
            if (Bolos.Any())
                Bolos.ToList().ForEach(x => SendMessage(p, "", new[] { 0, 0, 0 }, $"^8{x.Player}^7: ^3{x.Reason}")); // big msgs
            else
                SendMessage(p, "", new[] { 0, 0, 0 }, "^7None");
        }
        #endregion
        #endregion

        private void OnChatMessage(int source, string n, string msg)
        {
            Player p = Players[source]; // finding the player from the source
            var args = msg.Split(' ').ToList(); // splitting the message to find the args
            var cmd = args[0].ToLower(); // getting the command from the args
            args.RemoveAt(0); // removing the command part of the args

            // Reflection
            Commands instance = new Commands(); // creating the instance to invoke the command's methodinfo in
            // finding the command from the given message
            var command = typeof(Commands).GetMethods().Where(x => x.GetCustomAttributes<CommandAttribute>().Any())
                .FirstOrDefault(x => string.Equals(x.GetCustomAttributes<CommandAttribute>().ToArray()[0].Command, cmd, StringComparison.CurrentCultureIgnoreCase));
            // if command is not found then return null
            if (command == null) return;

            CancelEvent(); // don't display the message
            CommandAttribute information = command.GetCustomAttributes<CommandAttribute>().ToArray()[0]; // get the command's information
            switch (information.Type)
            {
                case CommandType.Civilian:
                    switch (Perms.CivilianPermission) // checking for civilian perms
                    {
                        // in the case of everyone, invoke
                        case Permission.Everyone:
                            command.Invoke(instance, new object[] {p, args.ToArray()});
                            break;
                        // in the case of specific, check for IP then invoke
                        case Permission.Specific:
                            if (Perms.CivContains(IPAddress.Parse(p.Identifiers["ip"])))
                                command.Invoke(instance, new object[] { p, args.ToArray() });
                            else
                                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You don't have the permission to do that!");
                            break;
                        // in the case of none, send out message saying you don't have permissions
                        case Permission.None:
                            SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You don't have the permission to do that!");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CommandType.Leo:
                    switch (Perms.LeoPermission)
                    {
                        // in the case of everyone, just invoke
                        case Permission.Everyone:
                            command.Invoke(instance, new object[] { p, args.ToArray() });
                            break;
                        // in the case of specific, check the ip then invoke
                        case Permission.Specific:
                            if (Perms.LeoContains(IPAddress.Parse(p.Identifiers["ip"])))
                                command.Invoke(instance, new object[] { p, args.ToArray() });
                            else
                                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You don't have the permission to do that!");
                            break;
                        // in the case of none, send a perms msg
                        case Permission.None:
                            SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You don't have the permission to do that!");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
