using System;

namespace DispatchSystem.Server
{
    /// <summary>
    /// The type of command 
    /// <para>Used for command permissions</para>
    /// </summary>
    [Flags] // Flags used for multiple permissions given
    public enum CommandType
    {
        /// <summary>
        /// Permission for LEO use
        /// </summary>
        Leo,
        /// <summary>
        /// Permission for Civilian use
        /// </summary>
        Civilian
    }
    /// <inheritdoc />
    /// <summary>
    /// Command attribute for setting commands in dispatchsystem
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)] // Only supposed to be used on methods
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command permissions
        /// </summary>
        public CommandType Type { get; }

        /// <summary>
        /// The string for the command given
        /// </summary>
        public string Command { get; }
        
        public CommandAttribute(CommandType type, string command)
        {
            Type = type;
            Command = command;
        }
    }
}
