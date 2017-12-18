using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispatch.Common.DataHolders
{
    public interface IOwnable
    {
        string SourceIP { get; }
    }
}
