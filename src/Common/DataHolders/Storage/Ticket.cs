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
        string _reason;
        float _amount;
        public string Reason => _reason;
        public float Amount => _amount;

        public DateTime Creation { get; }

        public Ticket(string reason, float amount)
        {
            _reason = reason;
            _amount = amount;

            Creation = DateTime.Now;
        }
    }
}
