using System;
using System.Collective.Generic;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace DispatchSystems.Server
{
  public class DispatchSystems : BaseScript
  {
    List<Civilian> civs;
    List<CivilianVeh> civVehs;
    
    String lastPlate = null;
  
    public DispatchSystems()
    {
		// Constructing in the constructor
		civs = new List<Civilian>();
		civVehs = new List<CivilianVeh>();
		
      // Setting event handlers here:
      this.EventHandlers["chatMessage"] += new Action<Int32, String, String>(this.OnChatMessage);
    
      // Adding event handlers here:
        // Player Events
      this.EventHandlers["dispatchsystem:transferLP"] += new Action<String>(this.TransferLicense);
      this.EventHandlers["dispatchsystem:setName"] += new Action<Player, String, String>(this.SetName);
      this.EventHandlers["dispatchsystem:toggleWarrant"] += new Action<Player>(this.ToggleWarrant);
      this.EventHandlers["dispatchsystem:setCitations"] += new Action<Player, Int32>(this.SetCitations);
        // Vehicle Events
      this.EventHandlers["dispatchsystem:setVehicle"] += new Action<Player>(this.SetVehicle);
      this.EventHandlers["dispatchsystem:toggleVehStolen"] += new Action<Player>(this.ToggleVehicleStolen);
    }
	
	private void OnChatMessage(Int32 source, String n, String msg)
	{
		Player p = this.Players[source];
		string[] args = msg.Split(' ');
	}
  }
  
  public String RequestPlate(Player player)
  {
    
  }
}

