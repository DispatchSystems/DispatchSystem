using System;
using System.Collections.Generic;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class EmergencyCall : IOwnable, IDataHolder
    {
        public EmergencyCall(string ip, string playerName)
        {
            Id = BareGuid.NewBareGuid();
            SourceIP = ip;
            Creation = DateTime.Now;
            PlayerName = playerName;
        }

        public bool Accepted { get; set; }
        public string PlayerName { get; }

        public string SourceIP { get; }
        public DateTime Creation { get; }
        public BareGuid Id { get; }
    }
}