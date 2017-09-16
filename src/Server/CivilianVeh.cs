using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.Server
{
    public class CivilianVeh
    {
        public Player Source { get; }
        public Civilian Owner { get; set; }
        public String Plate { get; set; }
        public Boolean StolenStatus { get; set; }
        public Boolean Registered { get; set; }
        public Boolean Insured { get; set; }

        public CivilianVeh(Player source)
        {
            Source = source;
            StolenStatus = false;
            Registered = true;
            Insured = true;
        }

        public override string ToString()
        {
            string[] strOut = new string[5];
            strOut[0] = string.IsNullOrWhiteSpace(Plate) ? "?" : Plate;
            strOut[1] = string.IsNullOrWhiteSpace(Owner.First) || string.IsNullOrWhiteSpace(Owner.Last) ? "?,?" : $"{Owner.First},{Owner.Last}";
            strOut[2] = StolenStatus.ToString();
            strOut[3] = Registered.ToString();
            strOut[4] = Insured.ToString();
            strOut[4] += "^";

            return string.Join("|", strOut);
        }
        public byte[] ToBytes() =>
            Encoding.UTF8.GetBytes(ToString());
    }
}
