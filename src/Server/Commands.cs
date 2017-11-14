using CitizenFX.Core;
using static CitizenFX.Core.BaseScript;

namespace DispatchSystem.sv
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
        [Command(CommandType.Civilian, "/civ")]
        public void CivilianNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleCivNUI");
        }

        /// <summary>
        /// Command for opening the LEO NUI
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        [Command(CommandType.Leo, "/leo")]
        public void LeoNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleLeoNUI");
        }
    }
}