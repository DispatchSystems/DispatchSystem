using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class CivilianVeh : PlayerBase, IDataHolder, IOwnable
    {
        public Civilian Owner { get; set; }
        protected string _plate;
        public string Plate { get => string.IsNullOrEmpty(_plate) ? string.Empty : _plate.ToUpper(); set => _plate = value; }
        public bool StolenStatus { get; set; }
        public bool Registered { get; set; }
        public bool Insured { get; set; }

        public CivilianVeh(string ip) : base(ip)
        {
            StolenStatus = false;
            Registered = true;
            Insured = true;
        }

        // Below is for communcation reasons between server and client
        [NonSerialized]
        public static readonly CivilianVeh Empty = new CivilianVeh(null);
        [NonSerialized]
        public static readonly CivilianVeh Null = null;
    }
}
