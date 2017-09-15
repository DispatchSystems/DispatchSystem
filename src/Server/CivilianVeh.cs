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
        public Civilian Owner { get; }
        public String Plate { get; }
        public Boolean StolenStatus { get; }
        public Boolean Registered { get; }
        public Boolean Insured { get; }

        public CivilianVeh(Player source) : this(source, null, null, false, true, true) { }
        public CivilianVeh(Player source, Civilian owner, String plate, Boolean stolen, Boolean regi, Boolean insur)
        {
            this.Source = source;
            this.Plate = plate;
            this.StolenStatus = stolen;
            this.Owner = owner;
            this.Registered = regi;
            this.Insured = insur;
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
