using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;

using Config.Reader;

using CitizenFX.Core.Native;

using static DispatchSystem.sv.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
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
            commands.Add("/newname", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/newname {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:setName", p.Handle, args[0], args[1]);
            }
            , CommandType.Civilian));
            commands.Add("/warrant", ((p, args) => TriggerEvent("dispatchsystem:toggleWarrant", p.Handle), CommandType.Civilian));
            commands.Add("/citations", ((p, args) =>
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
            }
            , CommandType.Civilian));
            #endregion

            #region Vehicle Commands
            commands.Add("/newveh", ((p, args) =>
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
            }
            , CommandType.Leo));
            commands.Add("/stolen", ((p, args) => TriggerEvent("dispatchsystem:toggleVehStolen", p.Handle), CommandType.Leo));
            commands.Add("/registered", ((p, args) => TriggerEvent("dispatchsystem:toggleVehRegi", p.Handle), CommandType.Leo));
            commands.Add("/insured", ((p, args) => TriggerEvent("dispatchsystem:toggleVehInsured", p.Handle), CommandType.Leo));
            #endregion

            #region Police Commands
            commands.Add("/2729", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/2729 {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:getCivilian", p.Handle, args[0], args[1]);
            }
            , CommandType.Leo));
            commands.Add("/28", ((p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/28 {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:getCivilianVeh", p.Handle, args[0]);
            }
            , CommandType.Leo));
            commands.Add("/note", ((p, args) =>
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
            }
            , CommandType.Leo));
            commands.Add("/ticket", ((p, args) =>
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
            }
            , CommandType.Leo));
            commands.Add("/tickets", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/tickets {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:civTickets", p.Handle, args[0], args[1]);
            }
            , CommandType.Leo));
            commands.Add("/notes", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/notes {first} {last}");
                    return;
                }

                TriggerEvent("dispatchsystem:displayCivNotes", p.Handle, args[0], args[1]);
            }
            , CommandType.Leo));
            commands.Add("/bolo", ((p, args) =>
            {
                if (args.Count() < 1)
                {
                    SendUsage(p, "/bolo {reason}");
                    return;
                }

                bolos.Add(new Bolo(p.Name, p.Identifiers["ip"], string.Join(" ", args)));
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"BOLO for \"{string.Join(" ", args)}\" added");
            }
            , CommandType.Leo));
            commands.Add("/bolos", ((p, args) =>
            {
                if (bolos.Count > 0)
                    bolos.ToList().ForEach(x => SendMessage(p, "", new[] { 0, 0, 0 }, $"^8{x.Player}^7: ^3{x.Reason}"));
                else
                    SendMessage(p, "", new[] { 0, 0, 0 }, "^7None");
            }
            , CommandType.Leo));
            #endregion

#if DEBUG
            #region Debug Commands
            // Lower 2 commands will throw errors if the names do not exist
            commands.Add("/ipof", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/ipof {first} {last}");
                    return;
                }

                Civilian x = GetCivilianByName(args[0], args[1]);
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, x.SourceIP);
            }
            , CommandType.Leo));
            commands.Add("/hexof", ((p, args) =>
            {
                if (args.Count() < 2)
                {
                    SendUsage(p, "/hexof {first} {last}");
                    return;
                }

                Civilian x = GetCivilianByName(args[0], args[1]);
                string hex = GetPlayerByIp(x.SourceIP).Identifiers["steam"];
                SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, hex);
            }
            , CommandType.Leo));
            #endregion
#endif
        }
        private void InitializeComponents()
        {
            cfg = new iniconfig(Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME), "settings.ini");
            (new Action<string>(fileName => { Permissions.SetInformation(fileName, Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME)); perms = Permissions.Get; perms.Refresh(); }))("permissions.perms");
            if (cfg.GetIntValue("server", "enable", 0) == 1)
            {
                ThreadPool.QueueUserWorkItem(x => server = new Server(cfg), null);
                Log.WriteLine("Starting DISPATCH server");
            }
            else
                Log.WriteLine("Not starting DISPATCH server");


            civs = new StorageManager<Civilian>();
            civVehs = new StorageManager<CivilianVeh>();
            commands = new Dictionary<string, (Command, CommandType)>();
            bolos = new StorageManager<Bolo>();
        }
    }
}
