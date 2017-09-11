/*
 * Information:
 * 
 * 
 * 
 *                __THIS IS A PRE-RELEASE__
 *                -------------------------
 *             There may be some features missing
 *         There may be some bugs in the features here
 * 
 * 
 * 
 * DispatchSystem made by BlockBa5her
 * 
 * Protected under the MIT License
*/

// Definitions
#undef ENABLE_VEH // Undefined because work is needed on topic
#undef DEBUG // Leave undefined unless you want unwanted messages in chat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace DispatchSystem.Server
{
    public class DispatchSystem : BaseScript
    {
        protected static List<Civilian> civs = new List<Civilian>();
        protected static List<CivilianVeh> civVehs = new List<CivilianVeh>();

        public DispatchSystem()
        {
            // Setting event handlers here:
            EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);

            // Adding event handlers here:
              // Player Events
            EventHandlers["dispatchsystem:setName"] += new Action<string, string, string>(SetName);
            EventHandlers["dispatchsystem:toggleWarrant"] += new Action<string>(ToggleWarrant);
            EventHandlers["dispatchsystem:setCitations"] += new Action<string, int>(SetCitations);
              // Vehicle Events
#if ENABLE_VEH
            EventHandlers["dispatchsystem:setVehicle"] += new Action<string>(SetVehicle);
            EventHandlers["dispatchsystem:toggleVehStolen"] += new Action<string>(ToggleVehicleStolen);
#endif
              // Police Events
            EventHandlers["dispatchsystem:getCivilian"] += new Action<string, string, string>(RequestCivilian);
#if ENABLE_VEH
            EventHandlers["dispatchsystem:getCivilianVeh"] += new Action<string, string>(RequestCivilianVeh);
            EventHandlers["dispatchsystem:transferLP"] += new Action<string>(TransferLicense);
#endif

            Debug.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            SendMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
        }

        #region Event Methods
        public static void SetName(string handle, string first, string last)
        {
            Player p = GetPlayerByHandle(handle);
            if (p == null) return;

            if (GetCivilian(handle) != null)
            {
                int index = civs.IndexOf(GetCivilian(handle));

                civs[index] = new Civilian(p, first, last, false, 0);

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {civs[index].First} {civs[index].Last}");
            }
            else
            {
                civs.Add(new Civilian(p, first, last, false, 0));
                int index = civs.IndexOf(GetCivilian(handle));

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {civs[index].First} {civs[index].Last}");
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new civilian profile...");
#endif
            }
#if ENABLE_VEH
            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));

                civVehs[index] = new CivilianVeh(p);
            }
#endif
        }
        public static void ToggleWarrant(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = civs.IndexOf(GetCivilian(handle));
                Civilian last = civs[index];

                civs[index] = new Civilian(p, last.First, last.Last, !last.WarrantStatus, last.CitationCount);

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Warrant status set to {civs[index].WarrantStatus.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can toggle your warrant");
        }
        public static void SetCitations(string handle, int count)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = civs.IndexOf(GetCivilian(handle));
                Civilian last = civs[index];

                civs[index] = new Civilian(p, last.First, last.Last, last.WarrantStatus, count);

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Citation count set to {count.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your citations");
        }
#if ENABLE_VEH
        public static void SetVehicle(string handle)
        {
            Player p = GetPlayerByHandle(handle);
            string lp = GetLicensePlate(p);
            if (lp == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must be in a vehicle to execute that operation!");
            }

            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));

                civVehs[index] = new CivilianVeh(p, lp, false);

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New Vehicle license plate set to {civVehs[index].Plate}");
            }
            else
            {
                civVehs.Add(new CivilianVeh(p, lp, false));
            }
        }
        public static void ToggleVehicleStolen(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));
                CivilianVeh last = civVehs[index];

                civVehs[index] = new CivilianVeh(p, last.Plate, !last.StolenStatus);

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Stolen status set to {civVehs[index].StolenStatus.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must have a vehicle set to execute that operation!");
        }
#endif
        public static void RequestCivilian(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                WriteChatLine(invoker);
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"First: {civ.First} | Last: {civ.Last}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Warrant: {civ.WarrantStatus.ToString()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Citations: {civ.CitationCount.ToString()}");
                WriteChatLine(invoker);
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
#if ENABLE_VEH
        public static void RequestCivilianVeh(string handle, string plate)
        {
            Player invoker = GetPlayerByHandle(handle);
            CivilianVeh civVeh = GetCivilianVehByPlate(plate);

            if (civVeh != null)
            {
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "", new[] { 0, 0, 0 }, $"Plate: {civVeh.Plate}");
                SendMessage(invoker, "", new[] { 0, 0, 0 }, $"Stolen: {civVeh.StolenStatus.ToString()}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That plate doesn't exist in the system");
        }
#endif
#endregion

        private void OnChatMessage(int source, string n, string msg)
        {
            Player p = Players[source];
            List<string> args = msg.Split(' ').ToList();
            string cmd = args[0].ToLower();
            args.RemoveAt(0);

              // Player Commands
            if (cmd == "/newname")
            {
                CancelEvent();

                if (args.Count < 2)
                {
                    SendUsage(p, "You must have atleast 2 arguments");
                    return;
                }

                TriggerEvent("dispatchsystem:setName", p.Handle, args[0], args[1]);
            }
            if (cmd == "/warrant")
            {
                CancelEvent();
                TriggerEvent("dispatchsystem:toggleWarrant", p.Handle);
            }
            if (cmd == "/citations")
            {
                CancelEvent();

                if (args.Count < 1)
                {
                    SendUsage(p, "You must have atleast 1 argument");
                    return;
                }

                if (int.TryParse(args[0], out int parse))
                {
                    TriggerEvent("dispatchsystem:setCitations", p.Handle, parse);
                }
                else
                    SendUsage(p, "The argument specified is not a valid number");
            }

              // Vehicle Commands
#if ENABLE_VEH
            if (cmd == "/newveh")
            {
                CancelEvent();
                TriggerEvent("dispatchsystem:setVehicle", p.Handle);
            }
            if (cmd == "/stolen")
            {
                CancelEvent();
                TriggerEvent("dispatchsystem:toggleVehStolen", p.Handle);
            }
#endif

              // Police Commands
            if (cmd == "/2729")
            {
                CancelEvent();

                if (args.Count < 2)
                {
                    SendUsage(p, "You must have atleast 2 arguments");
                    return;
                }

                TriggerEvent("dispatchsystem:getCivilian", p.Handle, args[0], args[1]);
            }
#if ENABLE_VEH
            if (cmd == "/28")
            {
                CancelEvent();

                if (args.Count < 1)
                {
                    SendUsage(p, "You must have atleast 1 argument");
                }

                TriggerEvent("dispatchsystem:getCivilianVeh");
            }
#endif
        }

#region Common
        private static Civilian GetCivilian(string pHandle)
        {
            foreach (var item in civs)
            {
                if (item.Source.Handle == pHandle)
                    return item;
            }

            return null;
        }
        private static Civilian GetCivilianByName(string first, string last)
        {
            foreach (var item in civs)
            {
                if (item.First.ToLower() == first.ToLower() && item.Last.ToLower() == last.ToLower())
                    return item;
            }

            return null;
        }
#if ENABLE_VEH
        private static CivilianVeh GetCivilianVeh(string pHandle)
        {
            foreach (var item in civVehs)
            {
                if (item.Source.Handle == pHandle)
                    return item;
            }

            return null;
        }
        private static CivilianVeh GetCivilianVehByPlate(string plate)
        {
            foreach (var item in civVehs)
            {
                if (item.Plate.ToLower() == plate.ToLower())
                    return item;
            }

            return null;
        }

        static string lastPlate = null;
        public static void TransferLicense(string license) => lastPlate = license;
        public static string GetLicensePlate(Player p)
        {
            TriggerClientEvent(p, "dispatchsystem:requestLP");

            while (lastPlate == null)
                Delay(10).Wait();

            string @return = null;
            if (lastPlate != "---------")
                @return = lastPlate;

            lastPlate = null;

            return @return;
        }
#endif

        private static Player GetPlayerByHandle(string handle)
        {
            foreach (var plr in new PlayerList())
                if (plr.Handle == handle) return plr;

            return null;
        }

        private static void WriteChatLine(Player p) => TriggerClientEvent(p, "chatMessage", "", new[] { 0, 0, 0 }, "\n");
        private static void WriteChatLine() => TriggerClientEvent("chatMessage", "", new[] { 0, 0, 0 }, "\n");
        private static void SendMessage(Player p, string title, int[] rgb, string msg) => TriggerClientEvent(p, "chatMessage", title, rgb, msg);
        private static void SendMessage(string title, int[] rgb, string msg) => TriggerClientEvent("chatMessage", title, rgb, msg);
        private static void SendUsage(Player p, string usage) => TriggerClientEvent(p, "chatMessage", "Usage", new[] { 255, 255, 255 }, usage);

        private static void CancelEvent() => Function.Call(Hash.CANCEL_EVENT);
#endregion
    }
}

