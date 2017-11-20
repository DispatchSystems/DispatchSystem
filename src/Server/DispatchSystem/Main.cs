using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using CitizenFX.Core;

using DispatchSystem.Common;
using DispatchSystem.Common.DataHolders.Storage;

using EZDatabase;
using static DispatchSystem.sv.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem : BaseScript
    {
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
            SendAllMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
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

        /// <summary>
        /// An emergency dump to clear all lists and dump everything into a file
        /// </summary>
        public static void EmergencyDump(Player invoker)
        {
            var write = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(new StorageManager<Civilian>(),
                new StorageManager<CivilianVeh>());
            Data.Write(write); // writing empty things to database

            var database = new Database("dispatchsystem.dmp"); // create the new database
            var write2 =
                new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>,
                    StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, Permissions>(Civs,
                    CivVehs, ActiveBolos, CurrentCalls, Officers, Perms); // create the tuple to write
            database.Write(write2); // write info

            // clearing all of the lists
            Civs.Clear();
            CivVehs.Clear();
            Officers.Clear();
            Assignments.Clear();
            OfcAssignments.Clear();
            CurrentCalls.Clear();
            Bolos.Clear();
            Server.Calls.Clear();

            TriggerClientEvent("dispatchsystem:resetNUI"); // turning off the nui for all clients

            // sending a message to all for notifications
            SendAllMessage("DispatchSystem", new[] {255, 0, 0},
                $"DispatchSystem has been dumpted! Everything has been deleted and scratched by {invoker.Name} [{invoker.Handle}]. " +
                "All previous items have been placed in a file labeled \"dispatchsystem.dmp\"");
        }
    }
}