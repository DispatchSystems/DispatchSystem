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
    }
}
