using System;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public abstract class PlayerBase : IDataHolder, IOwnable, IEquatable<PlayerBase>
    {
        public virtual string SourceIP { get; protected set; }
        public virtual DateTime Creation { get; }
        public virtual BareGuid Id { get; }

        public PlayerBase(string ip)
        {
            Creation = DateTime.Now;
            Id = BareGuid.NewBareGuid();

            SourceIP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public override bool Equals(object obj)
        {
            if (!(obj is PlayerBase))
                throw new ArgumentException("Your argument must be of PlayerBase Type", nameof(obj));

            PlayerBase _base = (PlayerBase)obj;
            return _base.Id == Id;
        }
        public bool Equals(PlayerBase item) => Equals(item);
    }
}
