using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;
using System.Net;

namespace DispatchSystem.sv.External
{
    /// <summary>
    /// Permission levels
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// Permission available to everyone
        /// </summary>
        Everyone,
        /// <summary>
        /// Permission available for specific people
        /// </summary>
        Specific,
        /// <summary>
        /// Permission available to nobody
        /// </summary>
        None
    }
    public sealed class Permissions
    {
        #region Singleton
        // Threadsafe lock for permission
        private static readonly object _lock = new object();
        // Instance of permissions object
        private static Permissions obj;
        // The filename for the permissions
        static string _fileName;
        // The resource name of the permissions file
        static string _resourceName;
        // Void to set the information listed above
        public static void SetInformation(string fileName, string resourceName) { _fileName = fileName; _resourceName = resourceName; }

        /// <summary>
        /// Property for returning the permissions
        /// </summary>
        public static Permissions Get
        {
            get
            {
                if (obj != null) return obj; // Checking if instance is already initiated
                lock (_lock) // thread locking when instance null
                    if (obj == null) // checking if current thread thinks it's still null
                        obj = new Permissions(_fileName, _resourceName); // creating instance when everything checks out
                return obj; // returns the instance
            }
        }
        #endregion

        /// <summary>
        /// The key to find when searching for Civilian perms
        /// </summary>
        public const string CIV_KEY = "civilian:";
        /// <summary>
        /// The key to find when searching for LEO perms
        /// </summary>
        public const string COP_KEY = "leo:";
        /// <summary>
        /// The key to find when searching for Dispatch perms
        /// </summary>
        public const string DISPATCH_KEY = "dispatcher:";

        // filename of the permission file
        private readonly string fileName;
        // resource that the permission file is under
        private readonly string resourceName;

        // A list of specific permissions
        private readonly List<Tuple<string, string>> items = new List<Tuple<string, string>>();

        /// <summary>
        /// The permission for the Civilian
        /// </summary>
        public Permission CivilianPermission { get; private set; } = Permission.Specific; // default specific
        /// <summary>
        /// The permission for the LEO
        /// </summary>
        public Permission LeoPermission { get; private set; } = Permission.Specific; // default specific
        /// <summary>
        /// The permission for the Dispatcher
        /// </summary>
        public Permission DispatchPermission { get; private set; } = Permission.Specific;

        /// <summary>
        /// IPs for Civilian specific permissions
        /// </summary>
        public IEnumerable<IPAddress> CivilianData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == CIV_KEY) // finding where they key is the civ key
                        if (IPAddress.TryParse(item.Item2, out IPAddress address)) // checking if the ip can be parsable
                            yield return address; // yield returns the ip to a enumerable
            }
        }
        /// <summary>
        /// IPs for the LEO specific permissions
        /// </summary>
        public IEnumerable<IPAddress> LeoData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == COP_KEY) // finding where they key is the leo key
                        if (IPAddress.TryParse(item.Item2, out IPAddress address)) // checking if the ip can be parsable
                            yield return address; // yield returns the ip to a enumerable
            }
        }
        /// <summary>
        /// IPs for the Dispatch specific permissions
        /// </summary>
        public IEnumerable<IPAddress> DispatchData
        {
            get
            {
                foreach (var item in items)
                    if (item.Item1 == DISPATCH_KEY) // finding where they key is the dispatch key
                        if (IPAddress.TryParse(item.Item2, out IPAddress address)) // checking if the ip can be parsable
                            yield return address; // yield returns the ip to a enumerable
            }
        }

        #region constructor
        private Permissions(string fileName, string resourceName)
        {
            // setting file items
            this.fileName = fileName;
            this.resourceName = resourceName;
        }
        public void Refresh()
        {
            Log.WriteLine("Setting the permissions"); // logs xd

            // reading the string data from the file
            string data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, resourceName, fileName);
            string current = string.Empty; // current key that the permissions is on
            // splitting the lines so that it is only important lines
            string[] lines = data.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => !x.StartsWith("//"))
                .Select(x => x.Trim().ToLower()).ToArray();

            // reading each line in all of the important lines
            foreach (string line in lines)
            {
                // checking for key lines, then continuing
                if (line == CIV_KEY || line == COP_KEY || line == DISPATCH_KEY) { current = line; continue; }
                // if there is no key so far, then continue
                if (current == string.Empty) continue;

                // switch for the current line
                switch (line)
                {
                    // checking for permission type of everyone
                    case "everyone":
                        // checking on which key it is on
                        switch (current)
                        {
                            case CIV_KEY:
                                // setting the civilian permission as everyone
                                CivilianPermission = Permission.Everyone;
                                break;
                            case COP_KEY:
                                // setting the leo permission as everyone
                                LeoPermission = Permission.Everyone;
                                break;
                            case DISPATCH_KEY:
                                // setting the dispatch permission as everyone
                                DispatchPermission = Permission.Everyone;
                                break;
                        }
                        break;
                    // checking for permission type of none
                    case "none":
                        switch (current)
                        {
                            case CIV_KEY:
                                // setting the civilian permission as none
                                CivilianPermission = Permission.None;
                                break;
                            case COP_KEY:
                                // setting the leo permission as none
                                LeoPermission = Permission.None;
                                break;
                            case DISPATCH_KEY:
                                // setting the dispatch permission as none
                                DispatchPermission = Permission.None;
                                break;
                        }
                        break;
                     // if no conditions apply, add the line as an IP to items
                    default:
                        items.Add(new Tuple<string, string>(current, line));
                        break;
                }
            }

            Log.WriteLine("Permissions set!"); // more logging
        }
        #endregion

        // ez contains
        public bool CivContains(IPAddress address) => CivilianData.Contains(address);
        public bool LeoContains(IPAddress address) => LeoData.Contains(address);
        public bool DispatchContains(IPAddress address) => DispatchData.Contains(address);
    }
}
