using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.DataHolders;

using Config.Reader;

using CitizenFX.Core;
using CitizenFX.Core.Native;

using static DispatchSystem.sv.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
        private void RegisterEvents()
        {
            EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);
            EventHandlers["dispatchsystem:dsreset"] += new Action<string>(DispatchReset);

            #region Civilian Commands
            EventHandlers["dispatchsystem:setName"] += new Action<string, string, string>(SetName);
            EventHandlers["dispatchsystem:toggleWarrant"] += new Action<string>(ToggleWarrant);
            EventHandlers["dispatchsystem:setCitations"] += new Action<string, int>(SetCitations);
            EventHandlers["dispatchsystem:911init"] += new Action<string>(InitializeEmergency);
            EventHandlers["dispatchsystem:911msg"] += new Action<string, string>(MessageEmergency);
            EventHandlers["dispatchsystem:911end"] += new Action<string>(EndEmergency);
            #endregion

            #region Vehicle Commands
            EventHandlers["dispatchsystem:setVehicle"] += new Action<string, string>(SetVehicle);
            EventHandlers["dispatchsystem:toggleVehStolen"] += new Action<string>(ToggleVehicleStolen);
            EventHandlers["dispatchsystem:toggleVehRegi"] += new Action<string>(ToggleVehicleRegistration);
            EventHandlers["dispatchsystem:toggleVehInsured"] += new Action<string>(ToggleVehicleInsurance);
            #endregion

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
        private void InitializeComponents()
        {
            callbacks = new ConcurrentQueue<Action>();
            officers = new StorageManager<Officer>();
            assignments = new List<Assignment>();
            ofcAssignments = new Dictionary<Officer, Assignment>();
            commands = new Dictionary<string, CommandAttribute>();
            bolos = new StorageManager<Bolo>();
            civs = new StorageManager<Civilian>();
            civVehs = new StorageManager<CivilianVeh>();
            currentCalls = new List<EmergencyCall>();

            cfg = new iniconfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");

            Permissions.SetInformation("permissions.perms", Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME));
            perms = Permissions.Get;
            perms.Refresh();

            if (cfg.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => server = new DispatchServer(cfg), null);
                Log.WriteLine("Starting DISPATCH server");
            }
            else
                Log.WriteLine("Not starting DISPATCH server");
            if (cfg.GetIntValue("database", "enable", 0) == 1)
            {
                new Thread(async () =>
                {
                    Log.WriteLine("Reading database...");
                    data = new Database("dispatchsystem.dontdelete");
                    Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>> read =
                        data.Read() ??
                        new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(new StorageManager<Civilian>(),
                            new StorageManager<CivilianVeh>());
                    civs = read.Item1;
                    civVehs = read.Item2;
                    Log.WriteLine("Read and set database");

                    while (true)
                    {
#if DEBUG
                        Log.WriteLine("Writing current information to database");
#else
                        Log.WriteLineSilent("Writing current information to database");
#endif
                        Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>> write = new Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>>(civs, civVehs);
                        data.Write(write);
                        await Delay(180 * 1000);
                    }
                }) { Name = "Database Thread"}.Start();
            }
            else
            {
                Log.WriteLine("Not start database");
            }
        }
    }
}
