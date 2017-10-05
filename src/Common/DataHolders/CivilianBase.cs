using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders
{
    [Serializable]
    public abstract class CivilianBase
    {
        public virtual string SourceIP { get; protected set; }

        public CivilianBase(string ip)
        {
            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }

        public abstract override string ToString();
        public virtual byte[] ToBytes() => Encoding.UTF8.GetBytes(ToString());

        public override int GetHashCode() => SourceIP.GetHashCode();
    }
}
