using System;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public enum OfficerStatus
    {
        OnDuty,
        OffDuty,
        Busy
    }
    [Serializable]
    public class Officer : PlayerBase
    {
        public string Callsign { get; set; }
        public OfficerStatus Status { get; set; }

        public Officer(string ip, string callsign) : base(ip)
        {
            Callsign = callsign;
            Status = OfficerStatus.OffDuty;
        }

        public override EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                Callsign,
                Status,
                new EventArgument[] {Id.ToString(), SourceIP, Creation.Ticks}
            };
        }
    }
}
