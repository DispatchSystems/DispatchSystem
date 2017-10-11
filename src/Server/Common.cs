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
            foreach (var item in civs)
            {
                if (GetPlayerByIp(item.SourceIP) != null)
                    if (GetPlayerByIp(item.SourceIP).Handle == pHandle)
                        return item;
            }

            return null;
            // return civs.ToList().Find(x => GetPlayerByIp(x.SourceIP) != null && GetPlayerByIp(x.SourceIP).Handle == pHandle);
            // TODO: Implement and fix Linq methods
        }
        public static Civilian GetCivilianByName(string first, string last)
        {
            foreach (var item in civs)
            {
                if (item.First.ToLower() == first.ToLower() && item.Last.ToLower() == last.ToLower())
                    return item;
            }

            return null;
            // return civs.ToList().Find(x => x.First.ToLower() == first.ToLower() && x.Last.ToLower() == last.ToLower());
            // TODO: Implement and fix Linq methods
        }
        public static CivilianVeh GetCivilianVeh(string pHandle)
        {
            foreach (var item in civVehs)
            {
                if (GetPlayerByIp(item.SourceIP) != null)
                    if (GetPlayerByIp(item.SourceIP).Handle == pHandle)
                        return item;
            }

            return null;
            // return civVehs.ToList().Find(x => GetPlayerByIp(x.SourceIP) != null && GetPlayerByIp(x.SourceIP).Handle == pHandle);
            // TODO: Implement and fix Linq methods
        }
        public static CivilianVeh GetCivilianVehByPlate(string plate)
        {
            foreach (var item in civVehs)
            {
                if (item.Plate.ToLower() == plate.ToLower())
                    return item;
            }

            return null;
            // return civVehs.ToList().Find(x => x.Plate.ToLower() == plate.ToLower());
            // TODO: Implement and fix Linq methods
        }

        internal static Player GetPlayerByHandle(string handle)
        {
            foreach (var plr in new PlayerList())
                if (plr.Handle == handle) return plr;

            return null;
            // return new PlayerList().ToList().Find(x => x.Handle == handle);
            // TODO: Implement and fix Linq methods
        }

        internal static Player GetPlayerByIp(string ip)
        {
            foreach (var plr in new PlayerList())
                if (plr.Identifiers["ip"] == ip) return plr;

            return null;
            // return new PlayerList().ToList().Find(x => x.Identifiers["ip"] == ip);
            // TODO: Implement and fix Linq methods
        }


        #region Chat Commands
        internal static void WriteChatLine(Player p) => TriggerClientEvent(p, "chatMessage", "", new[] { 0, 0, 0 }, "\n");
        internal static void WriteChatLine() => TriggerClientEvent("chatMessage", "", new[] { 0, 0, 0 }, "\n");
        internal static void SendMessage(Player p, string title, int[] rgb, string msg) => TriggerClientEvent(p, "chatMessage", title, rgb, msg);
        internal static void SendAllMessage(string title, int[] rgb, string msg) => TriggerClientEvent("chatMessage", title, rgb, msg);
        internal static void SendUsage(Player p, string usage) => TriggerClientEvent(p, "chatMessage", "Usage", new[] { 255, 255, 255 }, usage);
        #endregion

        internal static void CancelEvent() => Function.Call(Hash.CANCEL_EVENT);
    }
}
