using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Requesting
{
    [Serializable]
    public class CivilianVehRequest : IDataHolder
    {
        public readonly string Plate;
        public DateTime Creation { get; }

        public CivilianVehRequest(string plate)
        {
            Plate = plate;
            Creation = DateTime.Now;
        }

        public object[] ToObjectArray()
        {
            return new[] { (object)Plate };
        }
    }
}
