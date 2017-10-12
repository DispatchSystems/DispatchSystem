using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class CivilianVeh : CivilianBase, IDataHolder, IOwnable
    {
        public Civilian Owner { get; set; }
        protected string _plate;
        public string Plate { get => string.IsNullOrEmpty(_plate) ? string.Empty : _plate.ToUpper(); set => _plate = value; }
        public bool StolenStatus { get; set; }
        public bool Registered { get; set; }
        public bool Insured { get; set; }
        public override DateTime Creation { get; }

        public CivilianVeh(string ip) : base(ip)
        {
            StolenStatus = false;
            Registered = true;
            Insured = true;
            Creation = DateTime.Now;
        }

        public override string ToString()
        {
            string[] strOut = new string[6];
            strOut[0] = string.IsNullOrWhiteSpace(Plate) ? "?" : Plate;
            strOut[1] = string.IsNullOrWhiteSpace(Owner.First) || string.IsNullOrWhiteSpace(Owner.Last) ? "?,?" : $"{Owner.First},{Owner.Last}";
            strOut[2] = StolenStatus.ToString();
            strOut[3] = Registered.ToString();
            strOut[4] = Insured.ToString();
            strOut[5] = SourceIP;
            strOut[5] += "^";

            return string.Join("|", strOut);
        }

        public override object[] ToObjectArray()
        {
            return new[] { (object)Owner, (object)Plate, (object)StolenStatus, (object)Registered, (object)Insured };
        }

        // Below is for communcation reasons between server and client
        [NonSerialized]
        public static readonly CivilianVeh Empty = new CivilianVeh(null);
        [NonSerialized]
        public static readonly CivilianVeh Null = null;
    }
}
