using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.sv
{
    public delegate Task<bool> CommandCallback(Player invoker, string[] args);
    public enum CommandType
    {
        Leo,
        Civilian
    }
    public class Command
    {
        private CommandCallback _callback = null;
        public CommandCallback Callback { get => _callback; set => _callback = value; }

        private CommandType _type;
        public CommandType Type { get => _type; set => _type = value; }

        public Command(CommandType type) => _type = type;
    }
}
