using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchSystem.Common.DataHolders.Storage
{
    [Serializable]
    public class Civilian : CivilianBase, IDataHolder
    {
        protected string _first;
        public string First
        {
            get
            {
                char[] str = _first.ToCharArray();
                for (int i = 0; i < str.Length; i++)
                {
                    if (i == 0)
                        str[i] = char.ToUpper(str[i]);
                    else
                        str[i] = char.ToLower(str[i]);
                }
                return new string(str);
            }
            set => _first = value;
        }
        protected string _last;
        public string Last
        {
            get
            {
                char[] str = _last.ToCharArray();
                for (int i = 0; i < str.Length; i++)
                {
                    if (i == 0)
                        str[i] = char.ToUpper(str[i]);
                    else
                        str[i] = char.ToLower(str[i]);
                }
                return new string(str);
            }
            set => _last = value;
        }
        public bool WarrantStatus { get; set; }
        public int CitationCount { get; set; }
        public List<string> Notes { get; set; }
        public List<Tuple<string, float>> Tickets { get; set; }
        public override DateTime Creation { get; }

        public Civilian(string ip) : base(ip)
        {
            Notes = new List<string>();
            Tickets = new List<Tuple<string, float>>();
            WarrantStatus = false;
            CitationCount = 0;
            Creation = DateTime.Now;
        }

        public override string ToString()
        {
            string[] strOut = new string[6];
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
            strOut[5] = SourceIP;
            strOut[5] += "^";

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

            return new Civilian(string.Empty)
            {
                First = name[0],
                Last = name[1],
                CitationCount = rnd.Next(0, 11)
            };
        }

        public override object[] ToObjectArray()
        {
            return new[] { (object)First, (object)Last, (object)WarrantStatus, (object)CitationCount, (object)Notes, (object)Tickets };
        }
    }
}
