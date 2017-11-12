using System;
using System.IO;
using System.Runtime.Serialization;

namespace DispatchSystem.Common.DataHolders
{
    /// <summary>
    /// Thread safe read and write to a file of serializable objects
    /// </summary>
    public sealed class Database
    {
        // The file to save the database stuff on
        private readonly string file;

        /// <summary>
        /// Initializes the class of <see cref="Database"/>
        /// </summary>
        /// <param name="file"></param>
        public Database(string file)
        {
            this.file = file;
            lock (this) // Locking so thread read/write
                new FileStream(file, FileMode.OpenOrCreate).Dispose(); // Creating the database file
        }

        /// <summary>
        /// Reads the information of the Datafile file, then returns the dynamic value
        /// </summary>
        /// <returns>Value of the database file</returns>
        public dynamic Read()
        {
            lock (this) // Locking so thread read/write
            {
                using (var stream = new FileStream(file, FileMode.Open))
                {
                    if (!stream.CanWrite || !stream.CanRead)
                        throw new IOException("Invalid permissions to read or write"); // Throw when perms bad

                    byte[] buffer = new byte[stream.Length]; // creates the buffer of the stream
                    stream.Read(buffer, 0, (int)stream.Length); // reading the stream length to the buffer

                    return buffer.Length == 0 ? default(dynamic) : new StorableValue<dynamic>(buffer).Value; // returning the value from the bytes given
                }
            }
        }
        /// <summary>
        /// Writes the data to the database file, from the starting point
        /// </summary>
        /// <param name="data">The data to write to the database</param>
        public void Write(dynamic data)
        {
            lock (this) // Locking so thread read/write
            {
                if (!((object)data).GetType().IsSerializable)
                    throw new SerializationException("The object trying to be serialized is not marked serializable",
                        new NullReferenceException());

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    if (!stream.CanWrite || !stream.CanRead)
                        throw new IOException("Invalid permissions to read or write"); // Throw when perms bad

                    byte[] serializedData = new StorableValue<dynamic>(data).Bytes; // Getting the data from the object given
                    stream.Write(serializedData, 0, serializedData.Length); // Writing the bytes to the stream, starting at 0
                }
            }
        }
    }
}
