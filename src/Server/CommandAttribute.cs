using System;

namespace DispatchSystem.sv
{
    [Flags]
    public enum CommandType
    {
        Leo,
        Civilian
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandType Type { get; }

        public string Command { get; }

        public CommandAttribute(CommandType type, string command)
        {
            Type = type;
            Command = command;
        }
    }
}
