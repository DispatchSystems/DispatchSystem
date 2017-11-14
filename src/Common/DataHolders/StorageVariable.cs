/*
   _____                     _   _   _             
  / ____|                   | | (_) | |          _ 
 | |       _ __    ___    __| |  _  | |_   ___  (_)
 | |      | '__|  / _ \  / _` | | | | __| / __|    
 | |____  | |    |  __/ | (_| | | | | |_  \__ \  _ 
  \_____| |_|     \___|  \__,_| |_|  \__| |___/ (_)
                                                   
                                                   
This entire was made by CloneCommando
ps thanks clone my boi
https://www.github.com/CloneCommando
Found in his CloNET repository
*/

using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace DispatchSystem.Common.DataHolders
{
    /// <summary>
    /// Value that can be translated from bytes to object, and vise versa
    /// </summary>
    /// <typeparam name="T">The type of value to translate</typeparam>
    public struct StorableValue<T>
    {
        private T value;
        /// <summary>
        /// The value that has been translated
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                valChanged = true;
                this.value = value;
            }
        }

        private byte[] bytes; // private bytes version
        private bool valChanged;
        /// <summary>
        /// Serialization and deserialization of the given bytes
        /// </summary>
        public byte[] Bytes
        {
            set
            {
                using (var compressedStream = new MemoryStream())
                {
                    compressedStream.Write(value, 0, value.Length); // Writing the value to the compressed stream
                    compressedStream.Seek(0, SeekOrigin.Begin); // Going to the beginning of the stream

                    using (var uncompressedStream = new MemoryStream())
                    {
                        using (var decompress = new GZipStream(compressedStream, CompressionMode.Decompress, true)) // Creating instance of GZip stream, to uncompress stream
                        {
                            decompress.CopyTo(uncompressedStream); // Copying gzip stream to uncompressed stream
                        }
                        uncompressedStream.Position = 0; // Setting the position to 0 for deserialization
                        Value = (T)new BinaryFormatter().Deserialize(uncompressedStream); // Deserialization of the bytes given
                    }
                }

                valChanged = true;
            }
            get
            {
                if (!valChanged) return bytes;
                if (Value == null) return null;
                if (!Value.GetType().IsSerializable) return null; // Returning null if object unserializable
                using (var ms = new MemoryStream()) // Creating a memory stream for the bytes
                using (var gzip = new GZipStream(ms, CompressionMode.Compress)) // Creating a GZip stream for compression
                {
                    new BinaryFormatter().Serialize(gzip, Value); // Serializing the bytes to the gzip stream

                    // Closing the streams for read
                    gzip.Close();
                    ms.Close();

                    // Setting the bytes in the class to the memorystream
                    bytes = ms.ToArray();
                }
                valChanged = false;
                return bytes;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="StorableValue{T}"/> from <see cref="T"/>
        /// </summary>
        /// <param name="value"></param>
        public StorableValue(T value) : this()
        {
            Value = value;
            valChanged = true;
        }

        /// <summary>
        /// Creates an instance of <see cref="StorableValue{T}"/> from an array of <see cref="byte"/>
        /// </summary>
        /// <param name="valueBytes"></param>
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

        public static StorableValue<T> operator +(StorableValue<T> storableValue, T value)
        {
            storableValue.Value = value;
            return storableValue;
        }
    }
}
