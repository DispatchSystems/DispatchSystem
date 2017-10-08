using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DispatchSystem.Common.NetCode
{
    public class NetEvent
    {
        private readonly List<Func<NetRequestHandler, object[], Task>> Callback;

        public NetEvent() =>
            Callback = new List<Func<NetRequestHandler, object[], Task>>();

        public NetEvent(params Func<NetRequestHandler, object[], Task>[] funcs) =>
            Callback = new List<Func<NetRequestHandler, object[], Task>>(funcs);

        public static NetEvent operator +(NetEvent netEvent, Func<NetRequestHandler, object[], Task> func)
        {
            netEvent.Callback.Add(func);
            return netEvent;
        }

        public static NetEvent operator -(NetEvent netEvent, Func<NetRequestHandler, object[], Task> func)
        {
            netEvent.Callback.Remove(func);
            return netEvent;
        }

        public async Task Invoke(NetRequestHandler handler, object[] args)
        {
            IEnumerable<object> callbackObjs = Callback.Select(x => x.Invoke(handler, args));

            foreach (Task callbackObj in callbackObjs)
                await callbackObj;
        }
    }
}
