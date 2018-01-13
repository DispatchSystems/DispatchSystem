using System;
using System.Collections.Generic;
using System.Linq;

namespace Dispatch.Common.DataHolders
{
    public class EventArgument
    {
        public object Data { get; }

        private EventArgument(object item)
        {
            Data = item;
        }

        public static implicit operator EventArgument(int argument) => new EventArgument(argument);
        public static implicit operator EventArgument(uint argument) => new EventArgument(argument);
        public static implicit operator EventArgument(float argument) => new EventArgument(argument);
        public static implicit operator EventArgument(decimal argument) => new EventArgument(argument);
        public static implicit operator EventArgument(double argument) => new EventArgument(argument);
        public static implicit operator EventArgument(long argument) => new EventArgument(argument);
        public static implicit operator EventArgument(ulong argument) => new EventArgument(argument);
        public static implicit operator EventArgument(short argument) => new EventArgument(argument);
        public static implicit operator EventArgument(ushort argument) => new EventArgument(argument);
        public static implicit operator EventArgument(byte argument) => new EventArgument(argument);
        public static implicit operator EventArgument(sbyte argument) => new EventArgument(argument);
        public static implicit operator EventArgument(char argument) => new EventArgument(argument);
        public static implicit operator EventArgument(bool argument) => new EventArgument(argument);
        public static implicit operator EventArgument(string argument) => new EventArgument(argument);
        public static implicit operator EventArgument(Enum argument) => new EventArgument(argument.GetHashCode());
        public static implicit operator EventArgument(EventArgument[] argument) =>
            new EventArgument(ToArray(argument));

        public static object[] ToArray(IEnumerable<EventArgument> args) =>
            args?.Select(x => x?.Data).ToArray();
    }
}
