using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class Config
    {
        public static IPAddress IP { get; private set; }
        public static int Port { get; private set; }

        public static void Create(string FilePath)
        {
            string[] lines = File.ReadAllLines(FilePath);

            foreach (string[] line in lines.Where(x => !x.StartsWith(";")).Select(x => x.Split('=').Select(y => y.Trim()).ToArray()))
                switch (line[0])
                {
                    case "IP":
                        if (line[1] == "changeme")
                        {
                            MessageBox.Show("Looks like you forgot to change the config.\nPlease edit your config and then come back ༼ つ ◕_◕ ༽つ", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }

                        if (!IPAddress.TryParse(line[1], out IPAddress address))
                        {
                            MessageBox.Show("The ip address is invalid.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }

                        IP = address;
                        break;

                    case "Port":
                        if (!int.TryParse(line[1], out int _Port) || _Port < 1024 || _Port > 65536)
                        {
                            MessageBox.Show("The port is invalid.\nMake sure it is a positive integer within 1025-65535.", "DispatchSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }

                        Port = _Port;
                        break;
                }
        }
    }
}
