using System.Collections.Generic;
using Dispatch.Common;
using Dispatch.Common.DataHolders.Storage;

namespace DispatchSystem.Dump.Client
{
    public class Dump
    {
        public StorageManager<Civilian> Civilians { get; set; }
        public StorageManager<CivilianVeh> Vehicles { get; set; }
        public StorageManager<Bolo> Bolos { get; set; }
        public StorageManager<EmergencyCall> EmergencyCalls { get; set; }
        public StorageManager<Officer> Officers { get; set; }
        public List<string> Permissions { get; set; }
    }
}