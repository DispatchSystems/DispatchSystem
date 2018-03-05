using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using CitizenFX.Core;

using Config.Reader;

using EZDatabase;

using DispatchSystem.Server.External;
using DispatchSystem.Server.RequestHandling;

using Dispatch.Common.DataHolders.Storage;


namespace DispatchSystem.Server.Main
{
    public class DispatchSystem : BaseScript
    {
        internal static ExportDictionary InternalExports { get; private set; }
        
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
            Core.RequestHandler.TriggerEvent("load");
        }

        /*
         * Below is just for executing something on the main thread
        */

        /// <summary>
        /// Queue for action to execute on the main thread
        /// </summary>
        private static volatile ConcurrentQueue<Action> callbacks;
        private async Task OnTick()
        {
            // setting the "InternalExports" property constantly
            InternalExports = Exports;
            
            // Executing all of the callback methods available
            while (callbacks.TryDequeue(out Action queue))
                queue(); // executing the queue

            await Delay(100);
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
                    Core.RequestHandler.Handle(str, args?.ToArray(), calArgs?.ToArray()));
            // array of array of type, args, calArgs
            // request used to request multiple requests
            EventHandlers["dispatchsystem:post-multi"] +=
                new Action<List<object>>(objects => Core.RequestHandler.HandleMultiple(objects));

            #region Request Types
            Log.WriteLine("Adding request types to request handler");
            Core.RequestHandler.Types = new List<Request>
            {
                // Set requests
                new Request("set_dispatch_perms", Core.SetDispatchPerms),

                // Backbone requests
                new Request("req_civ", args => Core.RequestCivilian((string)args[0])),
                new Request("req_veh", args => Core.RequestCivilianVeh((string)args[0])),
                new Request("req_civ_by_name", args => Core.RequestCivilianByName((string)args[0], (string)args[1])),
                new Request("req_veh_by_plate", args => Core.RequestCivilianVehByPlate((string)args[0])),
                new Request("req_leo", args => Core.RequestOfficer((string)args[0])),
                new Request("req_leo_by_callsign", args => Core.RequestOfficerByCallsign((string)args[0])),
                new Request("req_leo_assignment", args => Core.RequestOfficerAssignment((string)args[0])),
                new Request("req_bolos", args => Core.RequestBolos()),

                // General events
                new Request("gen_reset", args => Core.DispatchReset((string)args[0])),
                new Request("gen_dump", args => DispatchSystemDump.EmergencyDump(Common.GetPlayerByHandle((string)args[0]))),

                // Civilian events
                new Request("civ_create", args => Core.SetName((string)args[0], (string)args[1], (string)args[2])),
                new Request("civ_toggle_warrant", args => Core.ToggleWarrant((string)args[0])),
                new Request("civ_set_citations", args => Core.SetCitations((string)args[0], (int)args[1])),
                new Request("civ_911_init", args => Core.InitializeEmergency((string)args[0]).Result),
                new Request("civ_911_msg", args => Core.MessageEmergency((string)args[0], (string)args[1]).Result),
                new Request("civ_911_end", args => Core.EndEmergency((string)args[0]).Result),

                // Vehicle events
                new Request("veh_create", args => Core.SetVehicle((string)args[0], (string)args[1])),
                new Request("veh_toggle_stolen", args => Core.ToggleVehicleStolen((string)args[0])),
                new Request("veh_toggle_regi", args => Core.ToggleVehicleRegistration((string)args[0])),
                new Request("veh_toggle_insurance", args => Core.ToggleVehicleInsurance((string)args[0])),

                // Officer events
                new Request("leo_create", args => Core.AddOfficer((string)args[0], (string)args[1])),
                new Request("leo_on_duty", args => Core.ToggleOnDuty((string)args[0])),
                new Request("leo_off_duty", args => Core.ToggleOffDuty((string)args[0])),
                new Request("leo_busy", args => Core.ToggleBusy((string)args[0])),
                new Request("leo_add_civ_note", args => Core.AddCivilianNote(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3])),
                new Request("leo_add_civ_ticket", args => Core.TicketCivilian(
                    (string)args[0], (string)args[1], (string)args[2], (string)args[3], (float)args[4])),
                new Request("leo_bolo_add", args => Core.AddBolo((string)args[0], (string)args[1]))
            };
            #endregion
        }
        private static void InitializeComponents()
        {
            // creating new instances of object
            Log.WriteLine("Creating needed objects");
            callbacks = new ConcurrentQueue<Action>();
            Core.Officers = new StorageManager<Officer>();
            Core.Assignments = new StorageManager<Assignment>();
            Core.OfficerAssignments = new Dictionary<Officer, Assignment>();
            Core.Bolos = new StorageManager<Bolo>();
            Core.Civilians = new StorageManager<Civilian>();
            Core.CivilianVehs = new StorageManager<CivilianVeh>();
            Core.CurrentCalls = new StorageManager<EmergencyCall>();
            Core.Config = new ServerConfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");
            Core.DispatchPerms = new List<string>();

            Log.WriteLine("Starting Request Handler");
            Core.RequestHandler = new RequestHandler();

            // reading config, then starting the server is config true
            if (Core.Config.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => Core.Server = new DispatchServer(Core.Config, Core.DispatchPerms), null);
                Log.WriteLine("Starting DISPATCH server");
            }
            else
                Log.WriteLine("Not starting DISPATCH server");

            // reading config, then starting database if config true
            if (Core.Config.GetIntValue("database", "enable", 0) == 1)
            {
                // starting the read/write thread for database
                async void RunDatabase()
                {
                    Log.WriteLine("Reading database...");
                    Core.Data = new Database("dispatchsystem.data"); // creating the database instance
                    Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>> read;
                    try
                    {
                        read = Core.Data.Read(); // reading the serialized tuple from the database
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
                            Core.Data.Write(null);
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
                    Core.Civilians = read?.Item1 ?? new StorageManager<Civilian>();
                    Core.CivilianVehs = read?.Item2 ?? new StorageManager<CivilianVeh>();
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
                        var write = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(Core.Civilians, Core.CivilianVehs);
                        // writing the information
                        Core.Data.Write(write);
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
