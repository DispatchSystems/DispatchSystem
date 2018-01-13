using System;
using Dispatch.Common.DataHolders;

namespace DispatchSystem.Server.RequestHandling
{
    public delegate RequestData RequestCallback(object[] args);

    public class Request
    {
        public string Name { get; }
        public RequestCallback Callback { get; }

        public Request(string name, RequestCallback callback)
        {
            Name = name?.ToLower() ?? throw new ArgumentNullException(nameof(name));
            Callback = callback;
        }
    }
}
