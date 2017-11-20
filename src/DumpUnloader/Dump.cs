using DispatchSystem.Common;
using DispatchSystem.Common.DataHolders.Storage;

namespace DumpUnloader
{
    public class Dump
    {
        public StorageManager<Civilian> Civilians { get; set; }
        public StorageManager<CivilianVeh> Vehicles { get; set; }
        public StorageManager<Bolo> Bolos { get; set; }
        public StorageManager<EmergencyCall> EmergencyCalls { get; set; }
        public StorageManager<Officer> Officers { get; set; }
        public Permissions Permissions { get; set; }
    }
}