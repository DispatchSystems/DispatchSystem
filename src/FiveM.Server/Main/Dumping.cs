using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

using Dispatch.Common.DataHolders;
using Dispatch.Common.DataHolders.Storage;
using DispatchSystem.Server.RequestHandling;
using static DispatchSystem.Server.Main.Core;

using EZDatabase;
using Json;

namespace DispatchSystem.Server.Main
{
    public static class DispatchSystemDump
    {
        private const string URL = "https://prealityv.com/ds/dump.php";
        private const string METHOD = "POST";

        /// <summary>
        /// An emergency dump to clear all lists and dump everything into a file
        /// </summary>
        public static RequestData EmergencyDump(Player invoker)
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

            string json = string.Empty;
            try
            {
                var db = new Database("dispatchsystem.dmp"); // create the new database
                var write2 = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>,
                    StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, List<string>>(Civilians,
                    CivilianVehs, Bolos, CurrentCalls, Officers, DispatchPerms);
                db.Write(write2); // write info

                json = JsonParser.Serialize(write2);
            }
            catch (Exception e)
            {
                Log.WriteLineSilent(e.ToString());
                code = 2;
            }

            try
            {
                // clearing all of the lists
                Civilians.Clear();
                CivilianVehs.Clear();
                Officers.Clear();
                Assignments.Clear();
                OfficerAssignments.Clear();
                CurrentCalls.Clear();
                Bolos.Clear();
                Core.Server.Calls.Clear();
            }
            catch (Exception e)
            {
                Log.WriteLineSilent(e.ToString());
                code = 3;
            }

            void SendError(Exception e)
            {
                Log.WriteLine("There was an error sending the information to BlockBa5her");
#if DEBUG
                Log.WriteLine(e.ToString());
#else
                Log.WriteLineSilent(e.ToString());
#endif
            }
            try
            {
                if (json == string.Empty)
                    throw new NullReferenceException();
                var form = $"dump_json={json}&code={code}";

                var headers = new Dictionary<string, object>
                {
                    {"Content-Type", "application/x-www-form-urlencoded"}
                };

                void Callback(List<object> x)
                {
                    try
                    {
#if DEBUG
                        Log.WriteLine("Web Callback: \"{0}\"", x[1]);
#else
                        Log.WriteLineSilent("Web Callback: \"{0}\"", x[1]);
#endif
                        
                        var obj = JsonParser.FromJson((string)x[1]);
                        if ((string)obj["message"] != "success")
                            throw new InvalidOperationException("Return code was not \"success\"");
                        
                        Log.WriteLine("Successfully sent BlockBa5her information");
                    }
                    catch (Exception e)
                    {
                        SendError(e);
                    }
                }

                DispatchSystem.InternalExports[API.GetCurrentResourceName()].httpRequest(new object[] 
                {
                    URL,
                    METHOD,
                    form, 
                    headers, 
                    new Action<List<object>>(Callback)
                });
            }
            catch (Exception e)
            {
                SendError(e);
            }

            return new RequestData(null, new EventArgument[] {Common.GetPlayerId(invoker), code, invoker.Name});
        }
    }
}
