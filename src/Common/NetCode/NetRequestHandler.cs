using System.Collections.Generic;

namespace DispatchSystem.Common.NetCode
{
    public class NetRequestHandler
    {
        public Dictionary<string, NetEvent> NetEvents { get; }

        public NetRequestHandler()
        {
            NetEvents = new Dictionary<string, NetEvent>();
        }

        public async void Handle(NetRequest Request)
        {
            switch (Request.Metadata)
            {
                case NetRequestMetadata.Invocation:
                    await NetEvents[(string)Request.Data.Value[0]].Invoke(Request.Data.Value);
                    break;
            }
        }
    }
}