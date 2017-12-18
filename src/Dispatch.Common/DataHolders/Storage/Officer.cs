using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class Officer : PlayerBase, IDataHolder, IOwnable
    {
        public string Callsign { get; set; }
        public OfficerStatus Status { get; set; }

        public Officer(string ip, string callsign) : base(ip)
        {
            Callsign = callsign;

            Status = OfficerStatus.OffDuty;
        }

        // For communicating
        public static readonly Officer Empty = new Officer(string.Empty, string.Empty);
    }
}
