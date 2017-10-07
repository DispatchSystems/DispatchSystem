using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Requesting
{
    /// <summary>
    /// Just a data request over the interwebs using a string
    /// </summary>
    [Serializable]
    public class DataRequest : IDataHolder
    {
        public readonly object[] Data;
        public DateTime Creation { get; }

        public DataRequest(object[] data)
        {
            Data = data;
            Creation = DateTime.Now;
        }

        public object[] ToObjectArray() => Data;
    }
}
