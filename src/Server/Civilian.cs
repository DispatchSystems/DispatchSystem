using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.Server
{
    public class Civilian
    {
        public Player Source { get; }
        public String First { get; set; }
        public String Last { get; set; }
        public Boolean WarrantStatus { get; set; }
        public Int32 CitationCount { get; set; }
        public List<string> Notes { get; }

        public Civilian(Player source)
        {
            Source = source;
            Notes = new List<string>();
            WarrantStatus = false;
            CitationCount = 0;
        }

        public override string ToString()
        {
            string[] strOut = new string[4];
            strOut[0] = string.IsNullOrWhiteSpace(First) || string.IsNullOrWhiteSpace(Last) ? "?,?" : $"{First},{Last}";
            strOut[1] = WarrantStatus.ToString();
            strOut[2] = CitationCount.ToString();
            strOut[3] = Notes.Count == 0 ? "?" : string.Join("\\", Notes.ToArray());
            strOut[3] += "^";

            return string.Join("|", strOut);
        }
        public byte[] ToBytes() =>
            Encoding.UTF8.GetBytes(ToString());
    }
}
