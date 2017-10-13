using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public abstract class PlayerBase : IDataHolder, IOwnable
    {
        public virtual string SourceIP { get; protected set; }
        public abstract DateTime Creation { get; }

        public PlayerBase(string ip)
        {
            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }

        public override int GetHashCode() => SourceIP.GetHashCode();
    }
}
