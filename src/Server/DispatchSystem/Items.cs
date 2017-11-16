using System.Collections.Generic;
using System.Collections.ObjectModel;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;

using Config.Reader;
using EZDatabase;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem
    {
        protected static ServerConfig Cfg; // config
        protected static Permissions Perms; // permissions
        protected static DispatchServer Server; // server for client+server transactions
        protected static Database Data; // database for saving

        internal static StorageManager<Bolo> Bolos; // active bolos
        internal static StorageManager<Civilian> Civs; // civilians
        internal static StorageManager<CivilianVeh> CivVehs; // civilian vehicles
        internal static StorageManager<Officer> Officers; // current officers
        internal static List<Assignment> Assignments; // active assignments
        internal static Dictionary<Officer, Assignment> OfcAssignments; // assignments attached to officers
        internal static StorageManager<EmergencyCall> CurrentCalls; // 911 calls
        public static ReadOnlyCollection<Civilian> Civilians => new ReadOnlyCollection<Civilian>(Civs); // public version of civilians
        public static ReadOnlyCollection<CivilianVeh> CivilianVehicles => new ReadOnlyCollection<CivilianVeh>(CivVehs); // public version of civilian vehicles
        public static StorageManager<Bolo> ActiveBolos => Bolos; // public version of bolos
    }
}
