using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;

using Config.Reader;
using EZDatabase;

using CitizenFX.Core.Native;
using DispatchSystem.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
        private void RegisterEvents()
        {
            // adding the main events
            EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);
            EventHandlers["dispatchsystem:dsreset"] += new Action<string>(DispatchReset);

            // civilian events
            #region Civilian Commands
            EventHandlers["dispatchsystem:displayCiv"] += new Action<string>(DisplayCurrentCivilian);
            EventHandlers["dispatchsystem:setName"] += new Action<string, string, string>(SetName);
            EventHandlers["dispatchsystem:toggleWarrant"] += new Action<string>(ToggleWarrant);
            EventHandlers["dispatchsystem:setCitations"] += new Action<string, int>(SetCitations);
            EventHandlers["dispatchsystem:911init"] += new Action<string>(InitializeEmergency);
            EventHandlers["dispatchsystem:911msg"] += new Action<string, string>(MessageEmergency);
            EventHandlers["dispatchsystem:911end"] += new Action<string>(EndEmergency);
            EventHandlers["dispatchsystem:requestClientInfo"] += new Action<string>(PushbackClientInfo);
            #endregion

            // events for vehicles
            #region Vehicle Commands
            EventHandlers["dispatchsystem:displayVeh"] += new Action<string>(DisplayCurrentVehicle);
            EventHandlers["dispatchsystem:setVehicle"] += new Action<string, string>(SetVehicle);
            EventHandlers["dispatchsystem:toggleVehStolen"] += new Action<string>(ToggleVehicleStolen);
            EventHandlers["dispatchsystem:toggleVehRegi"] += new Action<string>(ToggleVehicleRegistration);
            EventHandlers["dispatchsystem:toggleVehInsured"] += new Action<string>(ToggleVehicleInsurance);
            #endregion

            // officer specific events
            #region Police Commands
            EventHandlers["dispatchsystem:initOfficer"] += new Action<string, string>(AddOfficer);
            EventHandlers["dispatchsystem:onDuty"] += new Action<string>(ToggleOnDuty);
            EventHandlers["dispatchsystem:offDuty"] += new Action<string>(ToggleOffDuty);
            EventHandlers["dispatchsystem:busy"] += new Action<string>(ToggleBusy);
            EventHandlers["dispatchsystem:displayStatus"] += new Action<string>(DisplayStatus);
            EventHandlers["dispatchsystem:getCivilian"] += new Action<string, string, string>(RequestCivilian);
            EventHandlers["dispatchsystem:addCivNote"] += new Action<string, string, string, string>(AddCivilianNote);
            EventHandlers["dispatchsystem:ticketCiv"] += new Action<string, string, string, string, float>(TicketCivilian);
            EventHandlers["dispatchsystem:civTickets"] += new Action<string, string, string>(DisplayCivilianTickets);
            EventHandlers["dispatchsystem:displayCivNotes"] += new Action<string, string, string>(DipslayCivilianNotes);
            EventHandlers["dispatchsystem:getCivilianVeh"] += new Action<string, string>(RequestCivilianVeh);
            EventHandlers["dispatchsystem:addBolo"] += new Action<string, string>(AddBolo);
            EventHandlers["dispatchsystem:viewBolos"] += new Action<string>(ViewBolos);
            #endregion
        }
        private static void InitializeComponents()
        {
            // creating new instances of objects
            callbacks = new ConcurrentQueue<Action>();
            Officers = new StorageManager<Officer>();
            Assignments = new List<Assignment>();
            OfcAssignments = new Dictionary<Officer, Assignment>();
            Bolos = new StorageManager<Bolo>();
            Civs = new StorageManager<Civilian>();
            CivVehs = new StorageManager<CivilianVeh>();
            CurrentCalls = new StorageManager<EmergencyCall>();
            Cfg = new ServerConfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");

            // creating the permissions singleton
            Log.WriteLine("Setting permission information");
            Permissions.SetInformation(Function.Call<string>(Hash.LOAD_RESOURCE_FILE, Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "permissions.perms"));
            Log.WriteLine("Parsing permission information");
            Perms = Permissions.Get;
            Log.WriteLine("Permissions set!");

            // reading config, then starting the server is config true
            if (Cfg.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => Server = new DispatchServer(Cfg), null);
                Log.WriteLine("Starting DISPATCH server");
            }
            else
                Log.WriteLine("Not starting DISPATCH server");

            // reading config, then starting database if config true
            if (Cfg.GetIntValue("database", "enable", 0) == 1)
            {
                // starting the read/write thread for database
                async void RunDatabase()
                {
                    Log.WriteLine("Reading database...");
                    Data = new Database("dispatchsystem.data"); // creating the database instance
                    Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>> read = Data.Read(); // reading the serialized tuple from the database
                    Civs = read?.Item1 ?? new StorageManager<Civilian>();
                    CivVehs = read?.Item2 ?? new StorageManager<CivilianVeh>();
                    Log.WriteLine("Read and set database"); // logging done

                    // starting while loop for writing the database
                    while (true)
                    {
#if DEBUG
                        Log.WriteLine("Writing current information to database");
#else
                        Log.WriteLineSilent("Writing current information to database");
#endif
                        // creating the tuple to write
                        var write = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(Civs, CivVehs);
                        // writing the information
                        Data.Write(write);
                        // waiting 3 minutes before doing it again
                        await Delay(180 * 1000);
                    }
                }

                new Thread(RunDatabase) { Name = "Database Thread"}.Start(); // starting database thread
            }
            else
            {
                Log.WriteLine("Not starting the database");
            }
        }
    }
}
