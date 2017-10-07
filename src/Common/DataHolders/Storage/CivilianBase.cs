using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public abstract class CivilianBase : IDataHolder
    {
        public virtual string SourceIP { get; protected set; }
        public abstract DateTime Creation { get; }

        public CivilianBase(string ip)
        {
            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }

        public abstract override string ToString();
        public virtual byte[] ToBytes() => Encoding.UTF8.GetBytes(ToString());

        public abstract object[] ToObjectArray();

        public override int GetHashCode() => SourceIP.GetHashCode();
    }
}
