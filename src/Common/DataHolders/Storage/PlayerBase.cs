using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public abstract class PlayerBase : IDataHolder, IOwnable, IEquatable<PlayerBase>
    {
        public virtual string SourceIP { get; protected set; }
        public virtual DateTime Creation { get; }
        public virtual Guid Id { get; }

        public PlayerBase(string ip)
        {
            Creation = DateTime.Now;
            Id = Guid.NewGuid();

            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public override bool Equals(object obj)
        {
            if (!(obj is PlayerBase))
                throw new ArgumentException("Your argument must be of PlayerBase Type", "obj");

            PlayerBase _base = (PlayerBase)obj;
            return _base.Id == Id;
        }
        public bool Equals(PlayerBase item) => Equals(item);
    }
}
