using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;
using CitizenFX.Core.Native;

using DispatchSystem.Common.DataHolders.Storage;

using static DispatchSystem.sv.DispatchSystem;
using static CitizenFX.Core.BaseScript;

namespace DispatchSystem.sv
{
    public static class Common
    {
        public static Civilian GetCivilian(string pHandle)
        {
            return civs.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle);
        }
        public static Civilian GetCivilianByName(string first, string last)
        {
            return civs.FirstOrDefault(item => string.Equals(item.First, first, StringComparison.CurrentCultureIgnoreCase) && string.Equals(item.Last, last, StringComparison.CurrentCultureIgnoreCase));
        }
        public static CivilianVeh GetCivilianVeh(string pHandle)
        {
            return civVehs.Where(item => GetPlayerByIp(item.SourceIP) != null).FirstOrDefault(item => GetPlayerByIp(item.SourceIP).Handle == pHandle);
        }
        public static CivilianVeh GetCivilianVehByPlate(string plate)
        {
            return civVehs.FirstOrDefault(item => string.Equals(item.Plate, plate, StringComparison.CurrentCultureIgnoreCase));
        }
        public static Officer GetOfficer(string pHandle)
        {
            return officers.Where(item => GetPlayerByIp(item.SourceIP) != null).FirstOrDefault(item => GetPlayerByIp(item.SourceIP).Handle == pHandle);
        }
        public static EmergencyCall GetEmergencyCall(string pHandle)
        {
            return currentCalls.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle);
        }

        public static Assignment GetOfficerAssignment(Officer ofc)
        {
            return ofcAssignments.ContainsKey(ofc) ? ofcAssignments[ofc] : null;
        }
        internal static void RemoveAllInstancesOfAssignment(Assignment assignment)
        {
            foreach (var item in ofcAssignments)
                if (item.Value.Id == assignment.Id)
                    ofcAssignments.Remove(item.Key);

            assignments.Remove(assignment);
        }

        internal static Player GetPlayerByHandle(string handle)
        {
            return new PlayerList().FirstOrDefault(plr => plr.Handle == handle);
        }

        internal static Player GetPlayerByIp(string ip)
        {
            return new PlayerList().FirstOrDefault(plr => plr.Identifiers["ip"] == ip);
        }


        #region Chat Commands
        internal static void SendMessage(Player p, string title, int[] rgb, string msg) => TriggerClientEvent(p, "chatMessage", title, rgb, msg);
        internal static void SendAllMessage(string title, int[] rgb, string msg) => TriggerClientEvent("chatMessage", title, rgb, msg);
        internal static void SendUsage(Player p, string usage) => TriggerClientEvent(p, "chatMessage", "Usage", new[] { 255, 255, 255 }, usage);
        #endregion

        internal static void CancelEvent() => Function.Call(Hash.CANCEL_EVENT);
    }
}
