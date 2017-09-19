using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public interface ISyncable
    {
        bool IsCurrentlySyncing { get; }
        Timer Timer { get; }
        Task Resync();
    }
}
