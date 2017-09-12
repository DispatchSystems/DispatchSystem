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
        public String First { get; }
        public String Last { get; }
        public Boolean WarrantStatus { get; }
        public Int32 CitationCount { get; }
        public List<string> Notes { get; }

        public Civilian(Player source) : this(source, null, null, false, 0, new List<string>()) { }
        public Civilian(Player source, String first, String last, Boolean warrant, Int32 citations, List<string> notes)
        {
            this.Source = source;
            this.First = first;
            this.Last = last;
            this.WarrantStatus = warrant;
            this.CitationCount = citations;
            this.Notes = notes;
        }
    }
}
