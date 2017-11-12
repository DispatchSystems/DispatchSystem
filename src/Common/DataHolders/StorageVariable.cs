/*

░▄███▄░████▄░████░████▄░░██░████░▄███▄░░░
██▀░▀▀░██░██░██▄░░██░▀██░██░░██░░▀█▄▀▀░██
██▄░▄▄░████▀░██▀░░██░▄██░██░░██░░▄▄▀█▄░░░
░▀███▀░██░██░████░████▀░░██░░██░░▀███▀░██
This entire was made by CloneCommando
ps thanks clone my boi
https://www.github.com/CloneCommando
Found in his CloNET repository
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace DispatchSystem.Common.DataHolders
{
    public struct StorableValue<T>
    {
        private T value;
        public T Value
        {
            get => value;
            set
            {
                valChanged = true;
                this.value = value;
            }
        }

        public string FilePath { get; set; }

        private byte[] bytes;
        private bool valChanged;
        public byte[] Bytes
        {
            set
            {
                using (var compressedStream = new MemoryStream())
                {
                    compressedStream.Write(value, 0, value.Length);
                    compressedStream.Seek(0, SeekOrigin.Begin);

                    using (var uncompressedStream = new MemoryStream())
                    {
                        uncompressedStream.Position = 0;
                        using (var decompress = new GZipStream(compressedStream, CompressionMode.Decompress, true))
                        {
                            decompress.CopyTo(uncompressedStream);
                        }
                        uncompressedStream.Position = 0;
                        Value = (T)new BinaryFormatter().Deserialize(uncompressedStream);
                    }
                }

                valChanged = true;
            }
            get
            {
                if (!valChanged) return bytes;
                if (Value == null) return null;
                if (!Value.GetType().IsSerializable) return null;
                using (var ms = new MemoryStream())
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    new BinaryFormatter().Serialize(gzip, Value);

                    gzip.Close();
                    ms.Close();

                    bytes = ms.ToArray();
                }
                valChanged = false;
                return bytes;
            }
        }

        public StorableValue(T value) : this()
        {
            Value = value;
            valChanged = true;
        }

        public StorableValue(string filePath) : this()
        {
            if (!File.Exists(filePath))
                throw new InvalidOperationException("The file does not exist!");

            FilePath = filePath;

            Bytes = File.ReadAllBytes(filePath);

            valChanged = true;
        }

        public StorableValue(byte[] valueBytes) : this()
        {
            Bytes = valueBytes;

            valChanged = true;
        }

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(StorableValue<T> obj1, T obj2)
        {
            return obj1.Value.Equals(obj2);
        }

        public static bool operator !=(StorableValue<T> obj1, T obj2)
        {
            return !obj1.Value.Equals(obj2);
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new InvalidOperationException("Cannot save to file if " + nameof(FilePath) + " is null or whitespace!");

            File.WriteAllBytes(FilePath, Bytes);
        }

        public static StorableValue<T> operator +(StorableValue<T> storableValue, T value)
        {
            storableValue.Value = value;
            return storableValue;
        }
    }
}
