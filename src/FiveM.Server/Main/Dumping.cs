using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.Core;
using CloNET;

using Dispatch.Common.DataHolders;
using Dispatch.Common.DataHolders.Storage;

using DispatchSystem.Server.RequestHandling;
using static DispatchSystem.Server.Main.Core;

using EZDatabase;

namespace DispatchSystem.Server.Main
{
    public static class DispatchSystemDump
    {
        private const string IP = "158.69.48.250"; // IP of BlockBa5her's download server
        private const int PORT = 52535; // The port to send the dumps to
        private const string VER = "3.0.0"; // Version

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
            catch (Exception e)
            {
                Log.WriteLineSilent(e.ToString());
                code = 1;
            }

            try
            {
                var database = new Database("dispatchsystem.dmp"); // create the new database
                var write2 = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>,
                    StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, List<string>>(Civs,
                    CivVehs, Bolos, CurrentCalls, Officers, DispatchPerms);
                database.Write(write2); // write info
            }
            catch (Exception e)
            {
                Log.WriteLineSilent(e.ToString());
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
                Core.Server.Calls.Clear();
            }
            catch (Exception e)
            {
                Log.WriteLineSilent(e.ToString());
                code = 3;
            }

            try
            {
                Log.WriteLine("creation");
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
                    Log.WriteLine("Connection");
                    var connection = c.Connect(IP, PORT);
                    if (!connection.Wait(TimeSpan.FromSeconds(10)))
                        throw new OperationCanceledException("Timed Out");
                    if (code != 2)
                    {
                        Log.WriteLine("here1");
                        byte[] bytes = File.ReadAllBytes("dispatchsystem.dmp");
                        Log.WriteLine("here2: " + bytes.Length);
                        var task = c.Peer.RemoteCallbacks.Events["Send_3.*.*"].Invoke(code, VER, bytes);
                        if (!task.Wait(TimeSpan.FromSeconds(10)))
                            throw new OperationCanceledException("Timed Out");
                        Log.WriteLine("here3");
                    }
                    else
                        throw new AccessViolationException();
                    Log.WriteLine("end");
                }
                Log.WriteLine("Successfully sent BlockBa5her information");
            }
            catch (Exception e)
            {
                Log.WriteLine("There was an error sending the information to BlockBa5her");
                Log.WriteLineSilent(e.ToString());
            }

            Log.WriteLine("send");
            return new RequestData(null, new EventArgument[] { Common.GetPlayerId(invoker), code, invoker.Name});
        }
    }
}