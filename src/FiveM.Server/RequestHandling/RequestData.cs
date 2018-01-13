using Dispatch.Common.DataHolders;

namespace DispatchSystem.Server.RequestHandling
{
    public class RequestData
    {
        public readonly string Error;
        public readonly EventArgument[] Arguments;

        public RequestData(string err, EventArgument[] args = null)
        {
            Error = err;
            Arguments = args ?? new EventArgument[] {};
        }
    }
}
