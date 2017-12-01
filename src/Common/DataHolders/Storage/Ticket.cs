using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class Ticket : IDataHolder
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
    }
}
