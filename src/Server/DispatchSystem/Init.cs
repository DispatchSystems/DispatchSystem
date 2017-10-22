using System;
using System.Collections.Concurrent;
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
            EventHandlers["dispatchsystem:initOfficer"] += new Action<string, string>(AddOfficer);
            EventHandlers["dispatchsystem:onDuty"] += new Action<string>(ToggleOnDuty);
            EventHandlers["dispatchsystem:offDuty"] += new Action<string>(ToggleOffDuty);
            EventHandlers["dispatchsystem:busy"] += new Action<string>(ToggleBusy);
            EventHandlers["dispatchsystem:displayStatus"] += new Action<string>(DisplayStatus);
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
            // General Purpose
            commands.Add("/dsreset", new Command(CommandType.Civilian | CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (GetCivilian(p.Handle) != null)
                    {
                        civs.Remove(GetCivilian(p.Handle));
                    }
                    if (GetCivilianVeh(p.Handle) != null)
                    {
                        civVehs.Remove(GetCivilianVeh(p.Handle));
                    }
                    if (GetOfficer(p.Handle) != null)
                    {
                        officers.Remove(GetOfficer(p.Handle));
                    }

                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, "All profiles reset");
                    return true;
                }
            });

            #region Player Commands
            commands.Add("/newname", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/newname {first} {last}");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:setName", p.Handle, args[0], args[1]);
                    return true;
                }
            });
            commands.Add("/warrant", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    TriggerEvent("dispatchsystem:toggleWarrant", p.Handle);
                    return true;
                }
            });
            commands.Add("/citations", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 1)
                    {
                        SendUsage(p, "/citations {amount}");
                        return false;
                    }

                    if (int.TryParse(args[0], out int parse))
                    {
                        TriggerEvent("dispatchsystem:setCitations", p.Handle, parse);
                        return false;
                    }
                    else
                    {
                        SendUsage(p, "The amount is not a valid number!");
                        return false;
                    }

                }
            });
            #endregion

            #region Vehicle Commands
            commands.Add("/newveh", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 1)
                    {
                        SendUsage(p, "/newveh {plate}");
                        return false;
                    }

                    string plate = string.Join(" ", args);

                    if (plate.Count() > 8)
                    {
                        SendUsage(p, "Your plate cannot be over 8 characters long");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:setVehicle", p.Handle, plate);
                    return true;
                }
            });
            commands.Add("/stolen", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    TriggerEvent("dispatchsystem:toggleVehStolen", p.Handle);
                    return true;
                }
            });
            commands.Add("/registered", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    TriggerEvent("dispatchsystem:toggleVehRegi", p.Handle);
                    return true;
                }
            });
            commands.Add("/insured", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    TriggerEvent("dispatchsystem:toggleVehInsured", p.Handle);
                    return true;
                }
            });
            #endregion

            #region Police Commands
            commands.Add("/newofficer", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Length < 1)
                    {
                        SendUsage(p, "/newofficer {callsign}");
                        return false;
                    }
                    TriggerEvent("dispatchsystem:initOfficer", p.Handle, args[0]);
                    return true;
                }
            });
            commands.Add("/status", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Length < 1)
                    {
                        TriggerEvent("dispatchsystem:displayStatus", p.Handle);
                        return false;
                    }
                    if (args[0].ToLower() == "on")
                    {
                        TriggerEvent("dispatchsystem:onDuty", p.Handle);
                        return true;
                    }
                    else if (args[0].ToLower() == "off")
                    {
                        TriggerEvent("dispatchsystem:offDuty", p.Handle);
                        return true;
                    }
                    else if (args[0].ToLower() == "busy")
                    {
                        TriggerEvent("dispatchsystem:busy", p.Handle);
                        return true;
                    }
                    else
                    {
                        SendUsage(p, "/status {on|off|busy}");
                        return false;
                    }
                }
            });
            commands.Add("/2729", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/2729 {first} {last}");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:getCivilian", p.Handle, args[0], args[1]);
                    return true;
                }
            });
            commands.Add("/28", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 1)
                    {
                        SendUsage(p, "/28 {first} {last}");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:getCivilianVeh", p.Handle, args[0]);
                    return true;
                }
            });
            commands.Add("/note", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 3)
                    {
                        SendUsage(p, "/note {first} {last} {text}");
                        return false;
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
                    return true;
                }
            });
            commands.Add("/ticket", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 4)
                    {
                        SendUsage(p, "/ticket {first} {last} {amount} {reason}");
                        return false;
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
                        return true;
                    }
                    else
                    {
                        SendUsage(p, "The amount is not a valid number!");
                        return false;
                    }
                }
            });
            commands.Add("/tickets", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/tickets {first} {last}");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:civTickets", p.Handle, args[0], args[1]);
                    return true;
                }
            });
            commands.Add("/notes", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/notes {first} {last}");
                        return false;
                    }

                    TriggerEvent("dispatchsystem:displayCivNotes", p.Handle, args[0], args[1]);
                    return true;
                }
            });
            commands.Add("/bolo", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 1)
                    {
                        SendUsage(p, "/bolo {reason}");
                        return false;
                    }

                    bolos.Add(new Bolo(p.Name, p.Identifiers["ip"], string.Join(" ", args)));
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, $"BOLO for \"{string.Join(" ", args)}\" added");
                    return true;
                }
            });
            commands.Add("/bolos", new Command(CommandType.Leo)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (bolos.Count > 0)
                        bolos.ToList().ForEach(x => SendMessage(p, "", new[] { 0, 0, 0 }, $"^8{x.Player}^7: ^3{x.Reason}"));
                    else
                        SendMessage(p, "", new[] { 0, 0, 0 }, "^7None");

                    return true;
                }
            });
            #endregion

#if DEBUG
            #region Debug Commands
            // Lower 2 commands will throw errors if the names do not exist
            commands.Add("/ipof", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/ipof {first} {last}");
                        return false;
                    }

                    Civilian x = GetCivilianByName(args[0], args[1]);
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, x.SourceIP);
                    return true;
                }
            });
            commands.Add("/hexof", new Command(CommandType.Civilian)
            {
                Callback = async (p, args) =>
                {
                    await Delay(0);
                    if (args.Count() < 2)
                    {
                        SendUsage(p, "/hexof {first} {last}");
                        return false;
                    }

                    Civilian x = GetCivilianByName(args[0], args[1]);
                    string hex = GetPlayerByIp(x.SourceIP).Identifiers["steam"];
                    SendMessage(p, "DispatchSystem", new[] { 0, 0, 0 }, hex);
                    return true;
                }
            });
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
            if (cfg.GetIntValue("database", "enable", 0) == 1)
            {
                Log.WriteLine("Reading database...");
                ThreadPool.QueueUserWorkItem(x =>
                {
                    data = new Database();
                    StorageManager<Civilian> civs = data.Read<Civilian>("dsciv.db");
                    StorageManager<CivilianVeh> civVehs = data.Read<CivilianVeh>("dsveh.db");

                    DispatchSystem.civs = civs;
                    DispatchSystem.civVehs = civVehs;
                });
                Log.WriteLine("Read and set database");
                ThreadPool.QueueUserWorkItem(async x =>
                {
                    await Delay(15000);
                    while (true)
                    {
#if DEBUG
                        Log.WriteLine("Writing current information to database");
#else
                        Log.WriteLineSilent("Writing current information to database");
#endif
                        data.Write(civs, "dsciv.db");
                        data.Write(civVehs, "dsveh.db");
                        await Delay(180 * 1000);
                    }
                });
            }
            else
                Log.WriteLine("Not start database");


            callbacks = new ConcurrentQueue<Action>();
            civs = new StorageManager<Civilian>();
            civVehs = new StorageManager<CivilianVeh>();
            officers = new StorageManager<Officer>();
            assignments = new List<Assignment>();
            ofcAssignments = new Dictionary<Officer, Assignment>();
            commands = new Dictionary<string, Command>();
            bolos = new StorageManager<Bolo>();
        }
    }
}
