using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.NetCode
{
    public class NetFunction
    {
        private Func<NetRequestHandler, object[], Task<object>> Callback;

        public NetFunction(Func<NetRequestHandler, object[], Task<object>> functionCallback) =>
            Callback = functionCallback;

        public static NetFunction operator +(NetFunction netProperty, Func<NetRequestHandler, object[], Task<object>> functionCallback)
        {
            netProperty.Callback = (functionCallback);
            return netProperty;
        }

        public async Task<object> Invoke(NetRequestHandler handler, object[] args) =>
            await Callback.Invoke(handler, args);
    }
}
