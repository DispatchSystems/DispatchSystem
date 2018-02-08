using System;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class CivilianVeh : PlayerBase
    {
        public Civilian Owner { get; set; }
        private string plate;
        public string Plate { get => string.IsNullOrEmpty(plate) ? string.Empty : plate.ToUpper(); set => plate = value; }
        public bool StolenStatus { get; set; }
        public bool Registered { get; set; }
        public bool Insured { get; set; }

        public CivilianVeh(string ip) : base(ip)
        {
            StolenStatus = false;
            Registered = true;
            Insured = true;
        }

        public override EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                Owner?.ToArray(),
                Plate,
                StolenStatus,
                Registered,
                Insured,
                new EventArgument[] {SourceIP, Id.ToString(), Creation.Ticks}
            };
        }
    }
}
