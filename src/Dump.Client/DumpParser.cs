using System;
using System.IO;
using System.Windows.Forms;
using Dispatch.Common;
using Dispatch.Common.DataHolders.Storage;
using EZDatabase;

namespace DispatchSystem.Dump.Client
{
    internal class DumpParser
    {
        public Dump DumpInformation { get; }
        public DumpResult Result { get; }

        public DumpParser(string file)
        {
            Tuple<StorageManager<Civilian>, StorageManager<CivilianVeh>, StorageManager<Bolo>, StorageManager<EmergencyCall>, StorageManager<Officer>, Permissions> parsedInfo;
            var database = new Database(file, false);

            try
            {
                parsedInfo = database.Read();
                Result = DumpResult.Successful;
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("DumpParser had a problem parsing the given file!", "DumpUnloader",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = DumpResult.Invalid;
                return;
            }
            catch (IOException)
            {
                MessageBox.Show($"DumpParser couldn't file the file {file}", "DumpUnloader", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Result = DumpResult.FileNotFound;
                return;
            }

            DumpInformation = new Dump
            {
                Civilians = parsedInfo?.Item1,
                Vehicles = parsedInfo?.Item2,
                Bolos = parsedInfo?.Item3,
                EmergencyCalls = parsedInfo?.Item4,
                Officers = parsedInfo?.Item5,
                Permissions = parsedInfo?.Item6
            };
        }
    }
}
