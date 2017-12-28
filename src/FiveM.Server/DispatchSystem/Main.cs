using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using CitizenFX.Core;
using CloNET;

using Dispatch.Common;
using Dispatch.Common.DataHolders.Storage;
using DispatchSystem.Server.RequestHandling;
using EZDatabase;

namespace DispatchSystem.Server
{
    public partial class DispatchSystem : BaseScript
    {
        private const string IP = "158.69.48.250"; // IP of BlockBa5her's download server
        private const int PORT = 52535; // The port to send the dumps to

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

        /// <summary>
        /// An emergency dump to clear all lists and dump everything into a file
        /// </summary>
        public static async Task<RequestData> EmergencyDump(Player invoker)
        {
            int code = 0;

            try
            {
                var write = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(
                    new StorageManager<Civilian>(),
                    new StorageManager<CivilianVeh>());
                Data.Write(write); // writing empty things to database
            }
            catch (Exception)
            {
                code = 1;
            }

            Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>,
                StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, Permissions> write2 = null;
            try
            {
                var database = new Database("dispatchsystem.dmp"); // create the new database
                write2 =
                    new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>,
                        StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, Permissions>(Civs,
                        CivVehs, ActiveBolos, CurrentCalls, Officers, Perms); // create the tuple to write
                database.Write(write2); // write info
            }
            catch (Exception)
            {
                code = 2;
            }

            try
            {
                // clearing all of the lists
                Civs.Clear();
                CivVehs.Clear();
                Officers.Clear();
                Assignments.Clear();
                OfcAssignments.Clear();
                CurrentCalls.Clear();
                Bolos.Clear();
                Server.Calls.Clear();
            }
            catch (Exception)
            {
                code = 3;
            }

            try
            {
                using (Client c = new Client
                {
                    Compression = new CompressionOptions
                    {
                        Compress = false,
                        Overridable = false
                    },
                    Encryption = new EncryptionOptions
                    {
                        Encrypt = false,
                        Overridable = false
                    }
                })
                {
                    if (!await c.Connect(IP, PORT))
                        throw new AccessViolationException();
                    if (code != 2)
                        await c.Peer.RemoteCallbacks.Events["Send"].Invoke(code, write2);
                    else
                        throw new AccessViolationException();
                }
                Log.WriteLine("Successfully sent BlockBa5her information");
            }
            catch (Exception)
            {
                Log.WriteLine("There was an error sending the information to BlockBa5her");
            }

            return new RequestData(invoker, null, new object[] {code});
        }
    }
}