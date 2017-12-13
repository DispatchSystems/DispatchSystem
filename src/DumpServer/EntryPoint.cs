using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CloNET;

namespace Dump_Server
{
    internal static class EntryPoint
    {
        private static void Main(string[] args)
        {
            // creation of the server
            new DownServer();

            // literally nothing else. it just downloads and stores
        }
    }
}
