using CitizenFX.Core;
using static CitizenFX.Core.BaseScript;

namespace DispatchSystem.sv
{
    public class Commands
    {
        [Command(CommandType.Civilian | CommandType.Leo, "/dsreset")]
        public void DispatchSystemReset(Player p, string[] args)
        {
            TriggerEvent("dispatchsystem:dsreset", p.Handle);
        }

        [Command(CommandType.Civilian, "/civ")]
        public void CivilianNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleCivNUI");
        }

        [Command(CommandType.Leo, "/leo")]
        public void LeoNuiInit(Player p, string[] args)
        {
            TriggerClientEvent(p, "dispatchsystem:toggleLeoNUI");
        }
    }
}