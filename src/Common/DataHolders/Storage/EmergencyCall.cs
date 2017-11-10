using System;
using System.Collections.Generic;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class EmergencyCall : IOwnable, IDataHolder
    {
        public EmergencyCall(string ip, string playerName)
        {
            Id = Guid.NewGuid();
            SourceIP = ip;
            Creation = DateTime.Now;
            PlayerName = playerName;
        }

        public bool Accepted { get; set; }
        public string PlayerName { get; }

        public string SourceIP { get; }
        public DateTime Creation { get; }
        public Guid Id { get; }
    }
}