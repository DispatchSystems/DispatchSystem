using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;

using Config.Reader;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
        protected static iniconfig cfg;
        protected static Permissions perms;
        protected static DispatchServer DispatchServer;
        protected static Database data;

        internal static StorageManager<Bolo> bolos;
        internal static StorageManager<Civilian> civs;
        internal static StorageManager<CivilianVeh> civVehs;
        internal static StorageManager<Officer> officers;
        internal static List<Assignment> assignments;
        internal static Dictionary<Officer, Assignment> ofcAssignments;
        public static ReadOnlyCollection<Civilian> Civilians => new ReadOnlyCollection<Civilian>(civs);
        public static ReadOnlyCollection<CivilianVeh> CivilianVehicles => new ReadOnlyCollection<CivilianVeh>(civVehs);
        public static StorageManager<Bolo> ActiveBolos => bolos;

        private Dictionary<string, CommandAttribute> commands;
    }
}
