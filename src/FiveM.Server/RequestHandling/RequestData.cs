using CitizenFX.Core;

namespace DispatchSystem.Server.RequestHandling
{
    public class RequestData
    {
        public readonly Player Player;
        public readonly string Error;
        public readonly object[] Arguments;

        public RequestData(Player p, string err, object[] args = null)
        {
            Player = p;
            Error = err;
            Arguments = args ?? new object[] {};
        }
    }
}
