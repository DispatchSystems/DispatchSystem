using System;
using CitizenFX.Core;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Server.External;
using static CitizenFX.Core.BaseScript;

using EZDatabase;

namespace DispatchSystem.Server
{
    public class Commands
    {
        /// <summary>
        /// Command for reseting the current profiles
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        [Command(CommandType.Civilian | CommandType.Leo, "/dsreset")]
        public void DispatchSystemReset(Player p, string[] args)
        {
            TriggerEvent("dispatchsystem:dsreset", p.Handle);
        }

        /// <summary>
        /// Command for opening the Civilian NUI
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        [Command(CommandType.Civilian, "/dsciv")]
        public void CivilianNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleCivNUI");
        }

        /// <summary>
        /// Command for opening the LEO NUI
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        [Command(CommandType.Leo, "/dsleo")]
        public void LeoNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleLeoNUI");
        }

        /// <summary>
        /// Dumps everything into a file, and clears all databases
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        [Command(CommandType.Leo | CommandType.Civilian, "/dsdmp")]
        public void DispatchSystemDump(Player p, string[] args)
        {
            DispatchSystem.EmergencyDump(p);
        }
    }
}