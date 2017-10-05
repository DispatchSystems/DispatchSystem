using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders
{
    [Serializable]
    public class Bolo
    {
        string _player;
        string _reason;

        public string Player => _player;
        public string Reason => _reason;

        public Bolo(string player, string reason)
        {
            _player = player;
            _reason = reason;
        }
    }
}
