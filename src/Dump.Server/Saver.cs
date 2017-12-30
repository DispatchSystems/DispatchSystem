using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DispatchSystem.Dump.Server
{
    public class Saver
    {
        private readonly string binaryPath;
        private readonly byte[] data;

        public Saver(string filename, byte[] data)
        {
            binaryPath = $"dumps/{filename}.dmp";
            this.data = data;
        }
        public void Save()
        {
            Console.WriteLine($"Writing information to dumps/\"{binaryPath}\"");
            File.WriteAllBytes(binaryPath, data);
        }
    }
}
