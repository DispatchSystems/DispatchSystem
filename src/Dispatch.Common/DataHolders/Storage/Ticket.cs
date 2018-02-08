using System;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class Ticket : IDataHolder, IEventInfo
    {
        public string Reason { get; }
        public float Amount { get; }

        public DateTime Creation { get; }
        public BareGuid Id { get; }

        public Ticket(string reason, float amount)
        {
            Reason = reason;
            Amount = amount;

            Creation = DateTime.Now;
            Id = BareGuid.NewBareGuid();
        }

        public EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                Reason,
                Amount,
                new EventArgument[] {Id.ToString(), Creation.Ticks}
            };
        }
    }
}
