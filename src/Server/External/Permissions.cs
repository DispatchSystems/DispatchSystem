using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using System.IO;
using System.Net;

namespace DispatchSystem.sv.External
{
    public enum Permission
    {
        Everyone,
        Specific,
        None
    }
    public sealed class Permissions
    {
        #region Singleton
        static object _lock = new object();
        static Permissions obj = null;
        static string _fileName;
        static string _resourceName;
        public static void SetInformation(string fileName, string resourceName) { _fileName = fileName; _resourceName = resourceName; }

        public static Permissions Get
        {
            get
            {
                if (obj == null)
                    lock (_lock)
                        if (obj == null)
                            obj = new Permissions(_fileName, _resourceName);
                return obj;
            }
        }
        #endregion

        public const string CIV_KEY = "civilian:";
        public const string COP_KEY = "leo:";
        public const string DISPATCH_KEY = "dispatcher:";

        string fileName;
        string resourceName;

        List<(string, string)> items = new List<(string, string)>();

        public Permission CivilianPermission { get; private set; } = Permission.Specific;
        public Permission LeoPermission { get; private set; } = Permission.Specific;
        public Permission DispatchPermission { get; private set; } = Permission.Specific;

        public IEnumerable<IPAddress> CivilianData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == CIV_KEY)
                        if (IPAddress.TryParse(item.Item2, out IPAddress address))
                            yield return address;
            }
        }
        public IEnumerable<IPAddress> LeoData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == COP_KEY)
                        if (IPAddress.TryParse(item.Item2, out IPAddress address))
                            yield return address;
            }
        }
        public IEnumerable<IPAddress> DispatchData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == DISPATCH_KEY)
                        if (IPAddress.TryParse(item.Item2, out IPAddress address))
                            yield return address;
            }
        }

        #region constructor
        private Permissions(string fileName, string resourceName)
        {
            this.fileName = fileName;
            this.resourceName = resourceName;
        }
        public void Refresh()
        {
            Log.WriteLine("Setting the permissions");
            string data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, resourceName, fileName);
            string current = string.Empty;
            string[] lines = data.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !x.StartsWith("//")).Select(x => x.Trim().ToLower()).ToArray();

            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];

                if (line == CIV_KEY || line == COP_KEY || line == DISPATCH_KEY) { current = line; continue; }
                if (current == string.Empty) continue;

                if (line == "everyone")
                {
                    if (current == CIV_KEY)
                        CivilianPermission = Permission.Everyone;
                    if (current == COP_KEY)
                        LeoPermission = Permission.Everyone;
                    if (current == DISPATCH_KEY)
                        DispatchPermission = Permission.Everyone;
                }
                else if (line == "none")
                {
                    if (current == CIV_KEY)
                        CivilianPermission = Permission.None;
                    if (current == COP_KEY)
                        LeoPermission = Permission.None;
                    if (current == DISPATCH_KEY)
                        DispatchPermission = Permission.None;
                }
                else
                    items.Add((current, line));
            }

            Log.WriteLine("Permissions set!");
        }
        #endregion

        public bool CivContains(IPAddress address) => CivilianData.Contains(address);
        public bool LeoContains(IPAddress address) => LeoData.Contains(address);
        public bool DispatchContains(IPAddress address) => DispatchData.Contains(address);
    }
}
