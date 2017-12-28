using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using CitizenFX.Core.Native;
using CitizenFX.Core;

using Config.Reader;

using EZDatabase;

using DispatchSystem.Server.External;
using DispatchSystem.Server.RequestHandling;

using Dispatch.Common;
using Dispatch.Common.DataHolders.Storage;

namespace DispatchSystem.Server
{
    public partial class DispatchSystem
    {
        private void RegisterEvents()
        {
            // source, type, error, args, calArgs
            EventHandlers["dispatchsystem:event"] +=
                new Action<int, string, string, List<object>, List<object>>((source, type, error, args, calArgs) => { });
            // type, args, calArgs
            EventHandlers["dispatchsystem:post"] +=
                new Action<string, List<object>, List<object>>((str, args, calArgs) =>
                    ReqHandler.Handle(str, args.ToArray(), calArgs?.ToArray()));

            #region Request Types
            Log.WriteLine("Adding request types to request handler");
            ReqHandler.Types = new List<Request>
            {
                // General events
                new Request("gen_reset", args => DispatchReset((string)args[0])),
                new Request("gen_dump", args => EmergencyDump(Common.GetPlayerByHandle((string)args[0])).Result),
                new Request("gen_info", args => PushClientInfo((string)args[0])),

                // Civilian events
                new Request("civ_display", args => DisplayCurrentCivilian((string)args[0])),
                new Request("civ_create", args => SetName((string)args[0], (string)args[1], (string)args[2])),
                new Request("civ_toggle_warrant", args => ToggleWarrant((string)args[0])),
                new Request("civ_set_citations", args => SetCitations((string)args[0], (int)args[1])),
                new Request("civ_911_init", args => InitializeEmergency((string)args[0]).Result),
                new Request("civ_911_msg", args => MessageEmergency((string)args[0], (string)args[1]).Result),
                new Request("civ_911_end", args => EndEmergency((string)args[0]).Result),

                // Vehicle events
                new Request("veh_display", args => DisplayCurrentVehicle((string)args[0])),
                new Request("veh_create", args => SetVehicle((string)args[0], (string)args[1])),
                new Request("veh_toggle_stolen", args => ToggleVehicleStolen((string)args[0])),
                new Request("veh_toggle_regi", args => ToggleVehicleRegistration((string)args[0])),
                new Request("veh_toggle_insurance", args => ToggleVehicleRegistration((string)args[0])),

                // Officer events
                new Request("leo_create", args => AddOfficer((string)args[0], (string)args[1])),
                new Request("leo_on_duty", args => ToggleOnDuty((string)args[0])),
                new Request("leo_off_duty", args => ToggleOffDuty((string)args[0])),
                new Request("leo_busy", args => ToggleBusy((string)args[0])),
                new Request("leo_display_status", args => DisplayStatus((string)args[0])),
                new Request("leo_get_civ", args => RequestCivilian(
                    (string)args[0], (string)args[1], (string)args[2])),
                new Request("leo_add_civ_note", args => AddCivilianNote(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3])),
                new Request("leo_add_civ_ticket", args => TicketCivilian(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3], (float)args[4])),
                new Request("leo_display_civ_tickets", args => DisplayCivilianTickets(
                    (string)args[0], (string)args[1], (string)args[2])),
                new Request("leo_display_civ_notes", args => DisplayCivilianNotes(
                    (string)args[0], (string)args[1], (string)args[2])),
                new Request("leo_get_civ_veh", args => RequestCivilianVeh((string)args[0], (string)args[1])),
                new Request("leo_bolo_add", args => AddBolo((string)args[0], (string)args[1])),
                new Request("leo_bolo_view", args => ViewBolos((string)args[0]))
            };
            #endregion
        }
        private static void InitializeComponents()
        {
            // creating new instances of object
            Log.WriteLine("Creating needed objects");
            callbacks = new ConcurrentQueue<Action>();
            Officers = new StorageManager<Officer>();
            Assignments = new StorageManager<Assignment>();
            OfcAssignments = new Dictionary<Officer, Assignment>();
            Bolos = new StorageManager<Bolo>();
            Civs = new StorageManager<Civilian>();
            CivVehs = new StorageManager<CivilianVeh>();
            CurrentCalls = new StorageManager<EmergencyCall>();
            Cfg = new ServerConfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");

            Log.WriteLine("Starting Request Handler");
            ReqHandler = new RequestHandler();

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
                    Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>> read;
                    try
                    {
                        read = Data.Read(); // reading the serialized tuple from the database
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("----------------------------------------\n" +
                                        "     Error reading the data file\n" +
                                        "   More information in the log file\n" +
                                        "----------------------------------------");
                        Log.WriteLineSilent(e.ToString());
                        try
                        {
                            Data.Write(null);
                        }
                        catch (Exception e2)
                        {
                            Debug.WriteLine("-------------------------------------------\n" +
                                            "     Error writing the data file\n" +
                                            "    Is this a read/write problem?\n" +
                                            "   Stopping database functionality\n" +
                                            "   More information in the log file\n" +
                                            "-------------------------------------------");
                            Log.WriteLineSilent(e2.ToString());
                            return;
                        }
                        read = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(null, null);
                    }
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
