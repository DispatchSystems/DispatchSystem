﻿using System;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;

using Dispatch.Common.DataHolders.Storage;
using DispatchSystem.Server.Main;

namespace DispatchSystem.Server
{
    public static class Common
    {
        public static Civilian GetCivilian(string pHandle)
        {
            return Core.Civilians.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle); // Finding the first civ that has that handle
        }
        public static Civilian GetCivilianByName(string first, string last)
        {
            return Core.Civilians.FirstOrDefault(item =>
                string.Equals(item.First, first, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(item.Last, last,
                    StringComparison.CurrentCultureIgnoreCase)); // Finding the first civ that has that name
        }
        public static CivilianVeh GetCivilianVeh(string pHandle)
        {
            return Core.CivilianVehs.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle); // Finding the first Civilian Vehicle that has that handle
        }
        public static CivilianVeh GetCivilianVehByPlate(string plate)
        {
            return Core.CivilianVehs.FirstOrDefault(item => string.Equals(item.Plate, plate, StringComparison.CurrentCultureIgnoreCase)); // Finding the first civilian vehicle that has that plate
        }
        public static Officer GetOfficer(string pHandle)
        {
            return Core.Officers.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle); // Finding the first officer with that handle
        }
        public static EmergencyCall GetEmergencyCall(string pHandle)
        {
            return Core.CurrentCalls.FirstOrDefault(item => GetPlayerByIp(item.SourceIP)?.Handle == pHandle); // Finding the first handle with the emergency call
        }

        public static Assignment GetOfficerAssignment(Officer ofc)
        {
            return Core.OfficerAssignments.ContainsKey(ofc) ? Core.OfficerAssignments[ofc] : null; // Returning the officer's assignment
        }
        internal static void RemoveAllInstancesOfAssignment(Assignment assignment)
        {
            for (int i = 0; i < Core.OfficerAssignments.Count; i++)
                if (Core.OfficerAssignments.ToList()[i].Value.Id == assignment.Id)
                {
                    var item = Core.OfficerAssignments.ToList()[i];
                    item.Key.Status = OfficerStatus.OnDuty; // setting the status to onduty
                    Core.OfficerAssignments.Remove(item.Key); // Removing the assignment that has the right id
                    break;
                }

            Core.Assignments.Remove(assignment); // Removing the assignment from the assignments
        }

        internal static Player GetPlayerByHandle(string handle)
        {
            return new PlayerList().FirstOrDefault(plr => plr.Handle == handle); // Finding the first player with the handle
        }

        internal static Player GetPlayerByIp(string ip)
        {
            return new PlayerList().FirstOrDefault(plr => plr.Identifiers["ip"] == ip); // Finding the first player with the right IP
        }

        public static int GetPlayerId(Player p) => int.Parse(p?.Handle ?? "-1");

        #region Chat Commands
        /// <summary>
        /// EZ Thing for sending a message
        /// </summary>
        /// <param name="p"></param>
        /// <param name="title"></param>
        /// <param name="rgb"></param>
        /// <param name="msg"></param>
        internal static void SendMessage(Player p, string title, int[] rgb, string msg) =>
            BaseScript.TriggerClientEvent(p, "chatMessage", title, rgb, msg);
        /// <summary>
        /// EZ Thing for sending a message to all players in the lobby
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rgb"></param>
        /// <param name="msg"></param>
        internal static void SendAllMessage(string title, int[] rgb, string msg) =>
            BaseScript.TriggerClientEvent("chatMessage", title, rgb, msg);

#endregion

        /// <summary>
        /// Canceled the current event
        /// </summary>
        internal static void CancelEvent() => Function.Call(Hash.CANCEL_EVENT);
    }
}
