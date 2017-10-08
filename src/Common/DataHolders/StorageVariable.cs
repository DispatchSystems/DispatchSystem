using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DispatchSystem.Common.DataHolders
{
    [Serializable]
    public class StorableValue<T>
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
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream memStream = new MemoryStream())
                {
                    memStream.Write(value, 0, value.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    Value = (T)bf.Deserialize(memStream);
                }

                bytes = value;
                valChanged = false;
            }
            get
            {
                if (!valChanged)
                    return bytes;

                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, Value);
                    bytes = ms.ToArray();
                }

                valChanged = true;
                return bytes;
            }
        }

        public StorableValue()
        {
            valChanged = true;
        }

        public StorableValue(T Value) : this()
        {
            this.Value = Value;
        }

        public StorableValue(string FilePath) : this()
        {
            if (!File.Exists(FilePath))
                throw new InvalidOperationException("The file does not exist!");

            this.FilePath = FilePath;

            Bytes = File.ReadAllBytes(FilePath);
        }

        public StorableValue(byte[] ValueBytes) : this()
        {
            Bytes = ValueBytes;
        }

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(StorableValue<T> obj1, T obj2) =>
            obj1.Value.Equals(obj2);

        public static bool operator !=(StorableValue<T> obj1, T obj2) =>
            !obj1.Value.Equals(obj2);

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new InvalidOperationException("Cannot save to file if " + nameof(FilePath) + " is null or whitespace!");

            File.WriteAllBytes(FilePath, Bytes);
        }

        public static StorableValue<T> operator +(StorableValue<T> StorableValue, T Value)
        {
            StorableValue.Value = Value;
            return StorableValue;
        }
    }
}
