using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using CitizenFX.Core;

using Config.Reader;

using EZDatabase;

using DispatchSystem.Server.External;
using DispatchSystem.Server.RequestHandling;
using static DispatchSystem.Server.Main.Core;

using Dispatch.Common.DataHolders.Storage;


namespace DispatchSystem.Server.Main
{
    public class DispatchSystem : BaseScript
    {
        #region Main
        public DispatchSystem()
        {
            Log.Create("dispatchsystem.log");

            // starting the dispatchsystem
            InitializeComponents();
            RegisterEvents();

            // setting ontick for invocation
            Tick += OnTick;

            // logging and saying that dispatchsystem has been started
            Log.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            ReqHandler.TriggerEvent("load");
        }

        /*
         * Below is just for executing something on the main thread
        */

        /// <summary>
        /// Queue for action to execute on the main thread
        /// </summary>
        private static volatile ConcurrentQueue<Action> callbacks;
        private static async Task OnTick()
        {
            // Executing all of the callback methods available
            while (callbacks.TryDequeue(out Action queue))
                queue(); // executing the queue

            await Delay(0);
        }
        internal static void Invoke(Action method) => callbacks.Enqueue(method); // adding method for execution in main thread
        #endregion

        #region Initialization
        private void RegisterEvents()
        {
            // source, type, error, args, calArgs
            EventHandlers["dispatchsystem:event"] +=
                new Action<string, string, List<object>, List<object>>((type, error, args, calArgs) => { });
            // type, args, calArgs
            EventHandlers["dispatchsystem:post"] +=
                new Action<string, List<object>, List<object>>((str, args, calArgs) =>
                    ReqHandler.Handle(str, args?.ToArray(), calArgs?.ToArray()));

            #region Request Types
            Log.WriteLine("Adding request types to request handler");
            ReqHandler.Types = new List<Request>
            {
                // Set requests
                new Request("set_dispatch_perms", SetDispatchPerms),

                // Backbone requests
                new Request("req_civ", args => RequestCivilian((string)args[0])),
                new Request("req_veh", args => RequestCivilianVeh((string)args[0])),
                new Request("req_civ_by_name", args => RequestCivilianByName((string)args[0], (string)args[1])),
                new Request("req_veh_by_plate", args => RequestCivilianVehByPlate((string)args[0])),
                new Request("req_leo", args => RequestOfficer((string)args[0])),
                new Request("req_leo_by_callsign", args => RequestOfficerByCallsign((string)args[0])),
                new Request("req_leo_assignment", args => RequestOfficerAssignment((string)args[0])),
                new Request("req_bolos", args => RequestBolos()),

                // General events
                new Request("gen_reset", args => DispatchReset((string)args[0])),
                new Request("gen_dump", args => DispatchSystemDump.EmergencyDump(Common.GetPlayerByHandle((string)args[0])).Result),

                // Civilian events
                new Request("civ_create", args => SetName((string)args[0], (string)args[1], (string)args[2])),
                new Request("civ_toggle_warrant", args => ToggleWarrant((string)args[0])),
                new Request("civ_set_citations", args => SetCitations((string)args[0], (int)args[1])),
                new Request("civ_911_init", args => InitializeEmergency((string)args[0]).Result),
                new Request("civ_911_msg", args => MessageEmergency((string)args[0], (string)args[1]).Result),
                new Request("civ_911_end", args => EndEmergency((string)args[0]).Result),

                // Vehicle events
                new Request("veh_create", args => SetVehicle((string)args[0], (string)args[1])),
                new Request("veh_toggle_stolen", args => ToggleVehicleStolen((string)args[0])),
                new Request("veh_toggle_regi", args => ToggleVehicleRegistration((string)args[0])),
                new Request("veh_toggle_insurance", args => ToggleVehicleInsurance((string)args[0])),

                // Officer events
                new Request("leo_create", args => AddOfficer((string)args[0], (string)args[1])),
                new Request("leo_on_duty", args => ToggleOnDuty((string)args[0])),
                new Request("leo_off_duty", args => ToggleOffDuty((string)args[0])),
                new Request("leo_busy", args => ToggleBusy((string)args[0])),
                new Request("leo_add_civ_note", args => AddCivilianNote(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3])),
                new Request("leo_add_civ_ticket", args => TicketCivilian(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3], (float)args[4])),
                new Request("leo_bolo_add", args => AddBolo((string)args[0], (string)args[1]))
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
            DispatchPerms = new List<string>();

            Log.WriteLine("Starting Request Handler");
            ReqHandler = new RequestHandler();

            // reading config, then starting the server is config true
            if (Cfg.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => Core.Server = new DispatchServer(Cfg, DispatchPerms), null);
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
        #endregion
    }
}
