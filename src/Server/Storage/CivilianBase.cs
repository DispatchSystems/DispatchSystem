using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.sv.Storage
{
    public abstract class CivilianBase
    {
        public virtual string SourceIP { get; protected set; }

        public CivilianBase(Player source)
        {
            SourceIP = source == null ? string.Empty : source.Identifiers["ip"];
        }
        public CivilianBase(string ip)
        {
            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }

        public virtual Player Player()
        {
            foreach (var item in new PlayerList())
                if (item.Identifiers["ip"] == SourceIP)
                    return item;

            return null;
        }

        public abstract override string ToString();
        public virtual byte[] ToBytes() => Encoding.UTF8.GetBytes(ToString());

        public override int GetHashCode() => SourceIP.GetHashCode();

        public static implicit operator Player(CivilianBase civBase) => civBase.Player();
    }
}
