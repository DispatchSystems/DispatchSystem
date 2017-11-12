using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders
{
    public interface IDataHolder
    {
        DateTime Creation { get; }
        Guid Id { get; }
    }
}
