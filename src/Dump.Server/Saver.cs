using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DispatchSystem.Dump.Server
{
    public class Saver
    {
        public Saver(string filename, object data, int code)
        {
            string json = ConvertToJson(new [] {code, data});
            string jsonPath = $"dumps/{filename}.json";
            string binaryPath = $"dumps/{filename}.dmp";
            Console.WriteLine($"Writing information to dumps/\"{filename}\"");
            File.WriteAllLines($"{jsonPath}", new [] {json});
            new EZDatabase.Database(binaryPath).Write(data);
        }

        public static string ConvertToJson(object data) => JsonConvert.SerializeObject(data);
    }
}
