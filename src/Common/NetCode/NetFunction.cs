using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.NetCode
{
    public class NetFunction<T>
    {
        private Func<object[], Task<T>> callback;

        public NetFunction(Func<object[], Task<T>> FunctionCallback) =>
            callback = FunctionCallback;

        public static NetFunction<T> operator +(NetFunction<T> NetProperty, Func<object[], Task<T>> FunctionCallback)
        {
            NetProperty.callback = (FunctionCallback);
            return NetProperty;
        }

        public async Task<T> Invoke(object[] args) =>
            await callback.Invoke(args);
    }
}