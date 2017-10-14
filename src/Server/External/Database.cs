using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DispatchSystem.Common.DataHolders.Storage;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.sv.External
{
    public sealed class Database
    {
        const int READ_LENGTH = 15000;
        const string CIV_FILE = "dsciv.db";
        const string VEH_FILE = "dsveh.db";

        internal Database()
        {
            if (!File.Exists(CIV_FILE))
                File.Create(CIV_FILE);
            if (!File.Exists(VEH_FILE))
                File.Create(VEH_FILE);
        }

        public StorageManager<T> Read<T>(string filename) where T : IOwnable
        {
            if (filename != CIV_FILE && filename != VEH_FILE)
                throw new InvalidOperationException("filename cannot be different from set DB files");

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                byte[] buffer = new byte[READ_LENGTH];
                int length = stream.Read(buffer, 0, READ_LENGTH);
                buffer = buffer.Take(length).ToArray();

                if (buffer.Length == 0)
                    return new StorageManager<T>();
                else
                    return new StorableValue<StorageManager<T>>(buffer).Value;
            }
        }
        public void Write<T>(StorageManager<T> data, string filename) where T : IOwnable
        {
            if (filename != CIV_FILE && filename != VEH_FILE)
                throw new InvalidOperationException("filename cannot be different from set DB files");

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                if (!stream.CanWrite || !stream.CanRead)
                    throw new IOException("Invalid permissions to read or write");

                byte[] serializedData = new StorableValue<StorageManager<T>>(data).Bytes;
                stream.Write(serializedData, 0, serializedData.Length);
            }
        }
    }
}
