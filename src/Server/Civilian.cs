using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.sv
{
    public class Civilian
    {
        public Player Source { get; }
        public String First { get; set; }
        public String Last { get; set; }
        public Boolean WarrantStatus { get; set; }
        public Int32 CitationCount { get; set; }
        public List<string> Notes { get; }
        public List<(string, float)> Tickets { get; }

        public Civilian(Player source)
        {
            Source = source;
            Notes = new List<string>();
            Tickets = new List<(string, float)>();
            WarrantStatus = false;
            CitationCount = 0;
        }

        public override string ToString()
        {


            string[] strOut = new string[5];
            strOut[0] = string.IsNullOrWhiteSpace(First) || string.IsNullOrWhiteSpace(Last) ? "?,?" : $"{First},{Last}";
            strOut[1] = WarrantStatus.ToString();
            strOut[2] = CitationCount.ToString();
            strOut[3] = Notes.Count == 0 ? "?" : string.Join("\\", Notes.ToArray());
            strOut[4] = Tickets.Count == 0 ? "?" : string.Empty;
            if (strOut[4] == string.Empty)
            {
                List<string> x = new List<string>();

                foreach (var item in Tickets)
                    x.Add(string.Join("!", new string[] { item.Item2.ToString(), item.Item1 }));

                strOut[4] = string.Join("\\", x.ToArray());
            }
            strOut[4] += "^";

            return string.Join("|", strOut);
        }
        public byte[] ToBytes() =>
            Encoding.UTF8.GetBytes(ToString());
    }
}
