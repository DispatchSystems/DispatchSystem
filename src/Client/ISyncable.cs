using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispatchSystem.Client
{
    public interface ISyncable
    {
        bool IsCurrentlySyncing { get; }
        DateTime LastSyncTime { get; }
        Task Resync(bool skipTime);
        void UpdateCurrentInformation();
    }
}
