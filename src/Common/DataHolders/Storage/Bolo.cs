using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class Bolo : IDataHolder, IOwnable
    {
        string _player;
        string _reason;
        DateTime _creation;

        public string SourceIP { get; }
        public string Player => _player;
        public string Reason => _reason;
        public DateTime Creation => _creation;

        public Bolo(string playerName, string createrIp, string reason)
        {
            _player = playerName;
            SourceIP = string.IsNullOrWhiteSpace(createrIp) ? string.Empty : createrIp;
            _reason = reason;
            _creation = DateTime.Now;
        }

        public object[] ToObjectArray()
        {
            return new[] { (object)_player, (object)_reason };
        }
    }
}
