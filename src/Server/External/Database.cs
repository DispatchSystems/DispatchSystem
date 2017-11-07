using System;
using System.Linq;
using System.IO;
using System.Threading;
using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.sv.External
{
    public sealed class Database
    {
        private const int READ_LENGTH = 15000;
        private const string CIV_FILE = "dsciv.db";
        private const string VEH_FILE = "dsveh.db";

        private bool reading;

        internal Database()
        {
            reading = true;
            new FileStream(CIV_FILE, FileMode.OpenOrCreate).Dispose(); // Creating the CIV file
            new FileStream(VEH_FILE, FileMode.OpenOrCreate).Dispose(); // Creating the VEH file
            reading = false;
        }

        public StorageManager<T> Read<T>(string filename) where T : IOwnable
        {
            while (reading)
                Thread.Sleep(50);
            reading = true;
            if (filename != CIV_FILE && filename != VEH_FILE)
                throw new InvalidOperationException("filename cannot be different from set DB files");

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                if (!stream.CanWrite || !stream.CanRead)
                    throw new IOException("Invalid permissions to read or write");

                byte[] buffer = new byte[READ_LENGTH];
                int length = stream.Read(buffer, 0, READ_LENGTH);
                buffer = buffer.Take(length).ToArray();

                reading = false;
                return buffer.Length == 0 ? null : new StorableValue<StorageManager<T>>(buffer).Value;
            }
        }
        public void Write<T>(StorageManager<T> data, string filename) where T : IOwnable
        {
            while (reading)
                Thread.Sleep(50);
            reading = true;
            if (filename != CIV_FILE && filename != VEH_FILE)
                throw new InvalidOperationException("filename cannot be different from set DB files");

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                if (!stream.CanWrite || !stream.CanRead)
                    throw new IOException("Invalid permissions to read or write");

                byte[] serializedData = new StorableValue<StorageManager<T>>(data).Bytes;
                stream.Write(serializedData, 0, serializedData.Length);
                reading = false;
            }
        }
    }
}
