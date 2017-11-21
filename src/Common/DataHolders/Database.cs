/*
 * Original file made by http://www.github.com/blockba5her
 * File OPEN DOMAIN (No license)
 * Free for public/private use in applications
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EZDatabase
{
    /// <summary>
    /// Thread safe read and write to a file of serializable objects
    /// </summary>
    public sealed class Database
    {
        // The file to save the database stuff on
        private readonly string file;

        private const int MAX_FAIL = 10;

        /// <summary>
        /// Initializes the class of <see cref="Database"/>
        /// </summary>
        /// <param name="file">The name of the file to save the data in</param>
        /// <param name="createFile">Should the file be created if it doesn't already exist</param>
        public Database(string file, bool createFile = true)
        {
            this.file = file;
            if (!createFile) return;
            lock (this) // Locking so thread read/write
                File.Open(file, (FileMode)4).Dispose(); // Creating the database file (if not already created)
        }

        /// <summary>
        /// Reads the information of the Datafile file, then returns the dynamic value
        /// </summary>
        /// <returns>Value of the database file</returns>
        public dynamic Read()
        {
            lock (this) // Locking so thread read/write
            {
                using (var compressed = new FileStream(file, FileMode.Open))
                {
                    if (!compressed.CanWrite || !compressed.CanRead)
                        throw new IOException("Invalid permissions to read or write"); // Throw when perms bad

                    using (var uncompressed = new MemoryStream())
                    {
                        // while loop to suppress compression errors
                        int failed = 0;
                        while (true)
                        {
                            try // trying deserialization
                            {
                                using (var gzip = new GZipStream(compressed, CompressionMode.Decompress, true)
                                ) // creating decompresser stream
                                    gzip.CopyTo(
                                        uncompressed); // copy stream to the uncompressed stream for manipulation
                                uncompressed.Seek(0, SeekOrigin.Begin); // Setting the position to 0 for deserialization
                                return uncompressed.Length != 0
                                    ? new BinaryFormatter().Deserialize(uncompressed)
                                    : null; // Deserialization of the bytes given
                            }
                            catch (SerializationException) // Catching anything that has to due with serialization
                            {
                                failed++;
                            }
                            catch (IOException) // Catching anything that has to due with IO streams corrupting
                            {
                                failed++;
                            }
                            finally // throwing exception if failed too many times
                            {
                                if (failed > MAX_FAIL)
                                    throw new SerializationException("Database failed to serialize items after " + MAX_FAIL + " attempts");
                            }
                        }
                    }
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

                using (var fileStream = new FileStream(file, FileMode.Open))
                {
                    if (!fileStream.CanWrite || !fileStream.CanRead)
                        throw new IOException("Invalid permissions to read or write"); // Throw when perms bad

                    byte[] bytes;
                    // while loop to supress compression errors
                    int failed = 0;
                    while (true)
                    {
                        try // trying serialization
                        {
                            using (var ms = new MemoryStream()) // Creating a memory stream for the bytes
                            using (var gzip = new GZipStream(ms, CompressionMode.Compress)
                            ) // Creating a GZip stream for compression
                            {
                                new BinaryFormatter().Serialize(gzip, data); // Serializing the bytes to the gzip stream

                                // Closing the streams for read
                                gzip.Close();
                                ms.Close();

                                // Setting the bytes in the class to the memorystream
                                bytes = ms.ToArray();
                                break;
                            }
                        }
                        catch (SerializationException) // Catching anything that has to due with serialization
                        {
                            failed++;
                        }
                        catch (IOException) // Catching anything that has to due with IO streams corrupting
                        {
                            failed++;
                        }
                        finally // throwing exception if failed too many times
                        {
                            if (failed > MAX_FAIL)
                                throw new SerializationException("Database failed to serialize items after " + MAX_FAIL + " attempts");
                        }
                    }

                    fileStream.Write(bytes, 0, bytes.Length); // Writing the bytes to the stream, starting at 0
                }
            }
        }
    }
}