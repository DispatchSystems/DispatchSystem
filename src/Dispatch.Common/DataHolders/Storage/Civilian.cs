using System;
using System.Collections.Generic;
using System.Linq;

namespace Dispatch.Common.DataHolders.Storage
{
    [Serializable]
    public class Civilian : PlayerBase
    {
        private static string NameString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            char[] str = input.ToCharArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0)
                    str[i] = char.ToUpper(str[i]);
                else
                    str[i] = char.ToLower(str[i]);
            }
            return new string(str);
        }

        private string first;
        public string First
        {
            get => NameString(first);
            set => first = value;
        }
        private string last;
        public string Last
        {
            get => NameString(last);
            set => last = value;
        }
        public bool WarrantStatus { get; set; }
        public int CitationCount { get; set; }
        public List<string> Notes { get; set; }
        public List<Ticket> Tickets { get; set; }
        public override DateTime Creation { get; }

        public Civilian(string ip) : base(ip)
        {
            Notes = new List<string>();
            Tickets = new List<Ticket>();
            WarrantStatus = false;
            CitationCount = 0;
            Creation = DateTime.Now;
        }

        public override EventArgument[] ToArray()
        {
            return new EventArgument[]
            {
                First,
                Last,
                WarrantStatus,
                CitationCount,
                Notes.Select(x => (EventArgument)x).ToArray(),
                Tickets.Select(x => (EventArgument)x.ToArray()).ToArray(),
                new EventArgument[] {SourceIP, Id.ToString(), Creation.Ticks}
            };
        }

        public static Civilian CreateRandomCivilian()
        {
            #region NamesDic
            // ReSharper disable StringLiteralTypo
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
            // ReSharper restore StringLiteralTypo
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
    }
}
