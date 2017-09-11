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
        public String Plate { get; }
        public Boolean StolenStatus { get; }

        public CivilianVeh(Player source) : this(source, null, false) { }
        public CivilianVeh(Player source, String plate, Boolean stolen)
        {
            this.Source = source;
            this.Plate = plate;
            this.StolenStatus = stolen;
        }
    }
}
