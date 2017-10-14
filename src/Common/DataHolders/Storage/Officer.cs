using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
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
        public string PlayerName { get; set; }
        public OfficerStatus Status { get; set; }
        public override DateTime Creation { get; }

        public Officer(string ip, string pName) : base(ip)
        {
            Creation = DateTime.Now;
            PlayerName = pName;

            Status = OfficerStatus.OffDuty;
        }

        // For communicating
        public static readonly Officer Empty = new Officer(string.Empty, string.Empty);
    }
}
