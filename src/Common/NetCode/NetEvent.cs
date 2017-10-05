using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DispatchSystem.Common.NetCode
{
    public class NetEvent
    {
        private readonly List<Delegate> callback;

        public NetEvent() =>
            callback = new List<Delegate>();

        public NetEvent(params Action[] Actions) =>
            callback = new List<Delegate>(Actions);

        public static NetEvent operator +(NetEvent NetEvent, Delegate Delegate)
        {
            NetEvent.callback.Add(Delegate);
            return NetEvent;
        }

        public static NetEvent operator -(NetEvent NetEvent, Delegate Delegate)
        {
            NetEvent.callback.Remove(Delegate);
            return NetEvent;
        }

        public async Task Invoke(object[] args)
        {
            IEnumerable<object> callbackObjs = callback.Select(x => x.DynamicInvoke(args));

            foreach (object callbackObj in callbackObjs)
            {
                if (callbackObj == null)
                    continue;

                await (Task)callbackObj;
            }
        }
    }
}