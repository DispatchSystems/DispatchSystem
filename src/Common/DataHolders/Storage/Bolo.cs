using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class Bolo : IDataHolder
    {
        string _player;
        string _reason;
        DateTime _creation;

        public string Player => _player;
        public string Reason => _reason;
        public DateTime Creation => _creation;

        public Bolo(string player, string reason)
        {
            _player = player;
            _reason = reason;
            _creation = DateTime.Now;
        }

        public object[] ToObjectArray()
        {
            return new[] { (object)_player, (object)_reason };
        }
    }
}
