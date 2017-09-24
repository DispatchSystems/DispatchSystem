using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;

namespace DispatchSystem.sv.Storage
{
    public class Civilian : CivilianBase
    {
        public String First { get; set; }
        public String Last { get; set; }
        public Boolean WarrantStatus { get; set; }
        public Int32 CitationCount { get; set; }
        public List<string> Notes { get; set; }
        public List<(string, float)> Tickets { get; set; }

        public Civilian(Player source) : base(source)
        {
            Notes = new List<string>();
            Tickets = new List<(string, float)>();
            WarrantStatus = false;
            CitationCount = 0;
        }
        public Civilian() : base()
        {
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

        public static Civilian CreateRandomCivilian()
        {
            #region NamesDic
            List<string> rndNames = new List<string>
            {
                "Mason Bishan",
                "Zaahir Romolo",
                "Sang-Hun Adam",
                "Morgan Narayan",
                "Arsen Wendel",
                "Lazzaro Kylian",
                "Raleigh Jacob",
                "Paolino Marko",
                "Hafiz Shahnaz",
                "Saddam Feidhlimidh",
                "Eugène Doriano",
                "Gorden Roger",
                "Luke Patrizio",
                "Hisham Bertram",
                "Cornelius Kuldeep",
                "Vasu Wolter",
                "Rhett Uaithne",
                "Gallo Énna",
                "Jaffar Niklaus",
                "Silver Hendrik",
                "Norman Frazier",
                "Jerrold Hall",
                "Manus Cormac",
                "Arsenio Ikaia",
                "Yasser Morten",
                "Eugène Carmine",
                "Ciar Claude",
                "Michelangelo Olivier",
                "Fiachna Vasileios",
                "Wulf Myles",
                "Pyry Hyun-Woo",
                "Salman Gallo",
                "Anish Gabriel",
                "Karl Andreas"
            };
            #endregion

            Random rnd = new Random();

            string[] name = rndNames[rnd.Next(rndNames.Count)].Split(' ');

            return new Civilian
            {
                First = name[0],
                Last = name[1],
                CitationCount = rnd.Next(0, 11)
            };
        }
    }
}
