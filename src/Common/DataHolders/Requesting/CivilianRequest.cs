using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Requesting
{
    [Serializable]
    public class CivilianRequest : IDataHolder
    {
        public readonly string First;
        public readonly string Last;
        public DateTime Creation { get; }

        public CivilianRequest(string first, string last)
        {
            First = first;
            Last = last;
            Creation = DateTime.Now;
        }

        public object[] ToObjectArray()
        {
            return new[] { (object)First, (object)Last };
        }
    }
}
