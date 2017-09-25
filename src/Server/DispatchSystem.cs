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
#define DEBUG // Leave undefined unless you want unwanted messages in chat
#undef VDB // Add future version of DB?

#if !VDB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;

using CitizenFX.Core;
using CitizenFX.Core.Native;
using Config.Reader;

using DispatchSystem.sv.External;
using DispatchSystem.sv.Storage;

namespace DispatchSystem.sv
{
    internal delegate void Command(Player player, string[] args);

    public class DispatchSystem : BaseScript
    {
        protected static iniconfig cfg;
        private static Server server;

        internal static List<(string, string)> bolos;
        internal static StorageManager<Civilian> civs;
        internal static StorageManager<CivilianVeh> civVehs;
        public static ReadOnlyCollection<Civilian> Civilians => new ReadOnlyCollection<Civilian>(civs);
        public static ReadOnlyCollection<CivilianVeh> CivilianVehicles => new ReadOnlyCollection<CivilianVeh>(civVehs);
        public static List<(string, string)> ActiveBolos => bolos;

        private Dictionary<string, Command> commands;

        public DispatchSystem()
        {
            Server.Log.Create("dispatchsystem.log");

            InitializeComponents();
            RegisterEvents();
            RegisterCommands();

            Server.Log.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            SendAllMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
        }

        private void RegisterEvents()
        {
            EventHandlers["chatMessage"] += new Action<int, string, string>(OnChatMessage);

            #region Civilian Commands
            EventHandlers["dispatchsystem:setName"] += new Action<string, string, string>(SetName);
            EventHandlers["dispatchsystem:toggleWarrant"] += new Action<string>(ToggleWarrant);
            EventHandlers["dispatchsystem:setCitations"] += new Action<string, int>(SetCitations);
            #endregion

            #region Vehicle Commands
            EventHandlers["dispatchsystem:setVehicle"] += new Action<string, string>(SetVehicle);
            EventHandlers["dispatchsystem:toggleVehStolen"] += new Action<string>(ToggleVehicleStolen);
            EventHandlers["dispatchsystem:toggleVehRegi"] += new Action<string>(ToggleVehicleRegistration);
            EventHandlers["dispatchsystem:toggleVehInsured"] += new Action<string>(ToggleVehicleInsurance);
            #endregion

            #region Police Commands
            EventHandlers["dispatchsystem:getCivilian"] += new Action<string, string, string>(RequestCivilian);
            EventHandlers["dispatchsystem:addCivNote"] += new Action<string, string, string, string>(AddCivilianNote);
            EventHandlers["dispatchsystem:ticketCiv"] += new Action<string, string, string, string, float>(TicketCivilian);
            EventHandlers["dispatchsystem:civTickets"] += new Action<string, string, string>(DisplayCivilianTickets);
            EventHandlers["dispatchsystem:displayCivNotes"] += new Action<string, string, string>(DipslayCivilianNotes);
            EventHandlers["dispatchsystem:getCivilianVeh"] += new Action<string, string>(RequestCivilianVeh);
            #endregion
        }
        private void RegisterCommands()
        {
            #region Player Commands
            commands.Add("/newname", (p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/newname {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:setName", p.Handle, args[0], args[1]);
            });
            commands.Add("/warrant", (p, args) => TriggerEvent("dispatchsystem:toggleWarrant", p.Handle));
            commands.Add("/citations", (p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/citations {amount}");
                    return;
                }

                if (int.TryParse(args[0], out int parse))
                {
                    TriggerEvent("dispatchsystem:setCitations", p.Handle, parse);
                }
                else
                    SendUsage(p, "The amount is not a valid number!");
            });
            #endregion
            #region Vehicle Commands
            commands.Add("/newveh", (p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/newveh {plate}");
                    return;
                }

                string plate = string.Join(" ", args);

                if (plate.Count() > 8)
                {
                    SendUsage(p, "Your plate cannot be over 8 characters long");
                    return;
                }

                TriggerEvent("dispatchsystem:setVehicle", p.Handle, plate);
            });
            commands.Add("/stolen", (p, args) => TriggerEvent("dispatchsystem:toggleVehStolen", p.Handle));
            commands.Add("/registered", (p, args) => TriggerEvent("dispatchsystem:toggleVehRegi", p.Handle));
            commands.Add("/insured", (p, args) => TriggerEvent("dispatchsystem:toggleVehInsured", p.Handle));
            #endregion
            #region Police Commands
            commands.Add("/2729", (p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/2729 {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:getCivilian", p.Handle, args[0], args[1]);
            });
            commands.Add("/28", (p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/28 {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:getCivilianVeh", p.Handle, args[0]);
            });
            commands.Add("/note", (p, args) =>
            {
                if (args.Count() < 3)
                {
                    SendUsage(p, "/note {first} {last} {text}");
                    return;
                }

                string note = string.Empty;

                for (int i = 0; i < args.Count(); i++)
                {
                    if (i == 0 || i == 1)
                        continue;

                    note += args[i];
                    note += ' ';
                }

                TriggerEvent("dispatchsystem:addCivNote", p.Handle, args[0], args[1], note);
            });
            commands.Add("/ticket", (p, args) =>
            {
                if (args.Count() < 4)
                {
                    SendUsage(p, "/ticket {first} {last} {amount} {reason}");
                    return;
                }

                string reason = string.Empty;

                for (int i = 0; i < args.Count(); i++)
                {
                    if (i == 0 || i == 1 || i == 2)
                        continue;

                    reason += args[i];
                    reason += ' ';
                }

                if (float.TryParse(args[2], out float amount))
                {
                    TriggerEvent("dispatchsystem:ticketCiv", p.Handle, args[0], args[1], reason, amount);
                }
                else
                    SendUsage(p, "The amount is not a valid number!");
            });
            commands.Add("/tickets", (p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/tickets {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:civTickets", p.Handle, args[0], args[1]);
            });
            commands.Add("/notes", (p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/notes {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:displayCivNotes", p.Handle, args[0], args[1]);
            });
            commands.Add("/bolo", (p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/bolo {reason}");
                    return;
                }

                bolos.Add((p.Name, string.Join(" ", args)));
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"BOLO for \"{string.Join(" ", args)}\" added");
            });
            commands.Add("/bolos", (p, args) =>
            {
                if (bolos.Count > 0)
                    bolos.ForEach(x => SendMessage(p, "", new[] { 0, 0, 0 }, $"^8{x.Item1}^7: ^3{x.Item2}"));
                else
                    SendMessage(p, "", new[] { 0, 0, 0 }, "^7None");
            });
            #endregion
        }
        private void InitializeComponents()
        {
            cfg = new iniconfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");
            if (cfg.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => server = new Server((iniconfig)x), cfg);
                Server.Log.WriteLine("Starting DISPATCH server");
            }
            else
                Server.Log.WriteLine("Not starting DISPATCH server");


            civs = new StorageManager<Civilian>();
            civVehs = new StorageManager<CivilianVeh>();
            commands = new Dictionary<string, Command>();
            bolos = new List<(string, string)>();
        }

        #region Event Methods
        public static void SetName(string handle, string first, string last)
        {
            Player p = GetPlayerByHandle(handle);
            if (p == null) return;

            if (GetCivilian(handle) != null)
            {
                int index = civs.IndexOf(GetCivilian(handle));

                civs[index] = new Civilian(p) { First = first, Last = last };

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {civs[index].First} {civs[index].Last}");
            }
            else
            {
                civs.Add(new Civilian(p) { First = first, Last = last });
                int index = civs.IndexOf(GetCivilian(handle));

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New name set to: {civs[index].First} {civs[index].Last}");
#if DEBUG
                SendMessage(p, "", new[] { 0, 0, 0 }, "Creating new civilian profile...");
#endif
            }
            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));

                civVehs[index] = new CivilianVeh(p);
            }
        }
        public static void ToggleWarrant(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) != null)
            {
                int index = civs.IndexOf(GetCivilian(handle));
                civs[index].WarrantStatus = !civs[index].WarrantStatus;
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
                civs[index].CitationCount = count;

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Citation count set to {count.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your citations");
        }
        public static void SetVehicle(string handle, string plate)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle");
                return;
            }

            if (GetCivilianVeh(handle) != null)
            {
                Int32 index = civVehs.IndexOf(GetCivilianVeh(handle));

                civVehs[index] = new CivilianVeh(p) { Plate = plate.ToUpper(), Owner = GetCivilian(handle) };

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New vehicle set to {plate.ToUpper()}");
            }
            else
            {
                civVehs.Add(new CivilianVeh(p) { Plate = plate.ToLower(), Owner = GetCivilian(handle) });

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"New vehicle set to {plate.ToUpper()}");
            }
        }
        public static void ToggleVehicleStolen(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle stolen");
                return;
            }

            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));
                civVehs[index].StolenStatus = !civVehs[index].StolenStatus;

                if (civVehs[index].StolenStatus)
                {
                    Civilian civ = Civilian.CreateRandomCivilian();
                    civVehs[index].Owner = civ;
                    civs.Add(civ);
                }
                else
                {
                    Civilian civ = civVehs[index].Owner;
                    civs.Remove(civ);
                    civVehs[index].Owner = GetCivilian(handle);
                }


                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Stolen status set to {civVehs[index].StolenStatus.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your vehicle stolen");
        }
        public static void ToggleVehicleRegistration(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle registration");
                return;
            }

            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));
                civVehs[index].Registered = !civVehs[index].Registered;

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Registration status set to {civVehs[index].Registered.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your Regisration");
        }
        public static void ToggleVehicleInsurance(string handle)
        {
            Player p = GetPlayerByHandle(handle);

            if (GetCivilian(handle) == null)
            {
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your name before you can set your vehicle insurance");
                return;
            }

            if (GetCivilianVeh(handle) != null)
            {
                int index = civVehs.IndexOf(GetCivilianVeh(handle));
                CivilianVeh last = civVehs[index];

                civVehs[index].Insured = !civVehs[index].Insured;

                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"Insurance status set to {civVehs[index].Insured.ToString()}");
            }
            else
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "You must set your vehicle before you can set your Insurance");
        }
        public static void RequestCivilian(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"First: {civ.First} | Last: {civ.Last}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Warrant: {civ.WarrantStatus.ToString()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Citations: {civ.CitationCount.ToString()}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That civilian doesn't exist in the system");
        }
        public static void RequestCivilianVeh(string handle, string plate)
        {
            Player invoker = GetPlayerByHandle(handle);
            CivilianVeh civVeh = GetCivilianVehByPlate(plate);

            if (civVeh != null)
            {
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "Results: ");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Plate: {civVeh.Plate.ToUpper()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Stolen: {civVeh.StolenStatus.ToString()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Registered: {civVeh.Registered.ToString()}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Insured: {civVeh.Insured.ToString()}");
                if (civVeh.Registered) SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"R/O: {civVeh.Owner.First} {civVeh.Owner.Last}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That vehicle doesn't exist in the system");
        }
        public static void AddCivilianNote(string invokerHandle, string first, string last, string note)
        {
            Player invoker = GetPlayerByHandle(invokerHandle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                int index = civs.IndexOf(civ);
                civs[index].Notes.Add(note);
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"Note of \"{note}\" has been added to the Civilian");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void TicketCivilian(string invokerHandle, string first, string last, string reason, float amount)
        {
            Player invoker = GetPlayerByHandle(invokerHandle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                int index = civs.IndexOf(civ);
                Player p = civs[index].Player();
                civs[index].CitationCount++;
                civs[index].Tickets.Add((reason, amount));
                if (p != null)
                    SendMessage(p, "Ticket", new[] { 255, 0, 0 }, $"{invoker.Name} tickets you for ${amount.ToString()} because of {reason}");
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, $"You successfully ticketed {p.Name} for ${amount.ToString()}");
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void DisplayCivilianTickets(string invokerHandle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(invokerHandle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                int index = civs.IndexOf(civ);
                if (civs[index].Tickets.Count() == 0)
                    SendMessage(invoker, "", new[] { 0, 0, 0 }, "^7None");
                else
                    civs[index].Tickets.ForEach(x => SendMessage(invoker, "", new[] { 0, 0, 0 }, $"^7${x.Item2}: {x.Item1}"));
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        public static void DipslayCivilianNotes(string handle, string first, string last)
        {
            Player invoker = GetPlayerByHandle(handle);
            Civilian civ = GetCivilianByName(first, last);

            if (civ != null)
            {
                if (civ.Notes.Count == 0)
                    SendMessage(invoker, "", new[] { 0, 0, 0 }, "^9None");
                else
                    civ.Notes.ForEach(x => SendMessage(invoker, "", new[] { 0, 0, 0 }, x));
            }
            else
                SendMessage(invoker, "DispatchSystem", new[] { 0, 0, 0 }, "That name doesn't exist in the system");
        }
        #endregion

        private void OnChatMessage(int source, string n, string msg)
        {
            Player p = this.Players[source];
            var args = msg.Split(' ').ToList();
            var cmd = args[0].ToLower();
            args.RemoveAt(0);

            if (commands.ContainsKey(cmd))
            {
                CancelEvent();
                commands[cmd].Invoke(p, args.ToArray());
            }
        }

        #region Common
        public static Civilian GetCivilian(string pHandle)
        {
            foreach (var item in civs)
            {
                if (item.Player() != null)
                    if (item.Player().Handle == pHandle)
                        return item;
            }

            return null;
        }
        public static Civilian GetCivilianByName(string first, string last)
        {
            foreach (var item in civs)
            {
                if (item.First.ToLower() == first.ToLower() && item.Last.ToLower() == last.ToLower())
                    return item;
            }

            return null;
        }
        public static CivilianVeh GetCivilianVeh(string pHandle)
        {
            foreach (var item in civVehs)
            {
                if (item.Player() != null)
                    if (item.Player().Handle == pHandle)
                        return item;
            }

            return null;
        }
        public static CivilianVeh GetCivilianVehByPlate(string plate)
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

        private static Player GetPlayerByHandle(string handle)
        {
            foreach (var plr in new PlayerList())
                if (plr.Handle == handle) return plr;

            return null;
        }


        #region Chat Commands
        private static void WriteChatLine(Player p) => TriggerClientEvent(p, "chatMessage", "", new[] { 0, 0, 0 }, "\n");
        private static void WriteChatLine() => TriggerClientEvent("chatMessage", "", new[] { 0, 0, 0 }, "\n");
        private static void SendMessage(Player p, string title, int[] rgb, string msg) => TriggerClientEvent(p, "chatMessage", title, rgb, msg);
        private static void SendAllMessage(string title, int[] rgb, string msg) => TriggerClientEvent("chatMessage", title, rgb, msg);
        private static void SendUsage(Player p, string usage) => TriggerClientEvent(p, "chatMessage", "Usage", new[] { 255, 255, 255 }, usage);
        #endregion

        private static void CancelEvent() => Function.Call(Hash.CANCEL_EVENT);
        #endregion
    }
}
#endif
