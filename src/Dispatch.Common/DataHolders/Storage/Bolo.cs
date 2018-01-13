using System;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class Bolo : IDataHolder, IOwnable, IEventInfo
    {
        public string SourceIP { get; }
        public string Player { get; }
        public string Reason { get; }
        public DateTime Creation { get; }
        public BareGuid Id { get; }

        public Bolo(string playerName, string creatorIp, string reason)
        {
            Player = playerName;
            SourceIP = string.IsNullOrWhiteSpace(creatorIp) ? string.Empty : creatorIp;
            Reason = reason;
            Creation = DateTime.Now;
            Id = BareGuid.NewBareGuid();
        }

        public EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                Player,
                Reason,
                new EventArgument[] {Id.ToString(), SourceIP, Creation.Ticks}
            };
        }
    }
}
