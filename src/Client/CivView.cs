using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

namespace Client
{
    public partial class CivView : MaterialForm
    {
        string firstName = "Test";
        string lastName = "Name";
        bool wanted = true;
        int citations;
        string[] notes;
        (float, string)[] tickets;

        public CivView(string civData)
        {
            InitializeComponent();

            ParseCivilian(civData);
            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
        {
            firstNameView.Text = firstName;
            lastNameView.Text = lastName;
            wantedView.Checked = wanted;
            citationsView.Text = citations.ToString();

            if (notes[0] == "?")
                notes[0] = "None";

            if (notes[0] != null)
            {
                this.notes.ToList().ForEach(x => notesView.Items.Add(x));
            }

            if (tickets.Count() != 0)
            {
                foreach (var item in tickets)
                {
                    ListViewItem li = new ListViewItem($"${item.Item1.ToString()}");
                    li.SubItems.Add(item.Item2);
                    ticketsView.Items.Add(li);
                }
            }
        }

        private void ParseCivilian(string data)
        {
            string[] main = data.Split('|');
            string[] name = main[0].Split(',');
            bool wanted = bool.Parse(main[1]);
            int citations = int.Parse(main[2]);
            string[] notes = main[3].Split('\\');
            string[] ticketsMain = main[4].Split('\\');
            List<(float, string)> tickets = new List<(float, string)>();
            if (ticketsMain[0] != "?")
                foreach (var item in ticketsMain)
                {
                    string[] main2 = item.Split('!');
                    float amount = float.Parse(main2[0]);
                    string reason = main2[1];
                    tickets.Add((amount, reason));
                }
            this.tickets = tickets.ToArray();

            this.firstName = name[0];
            this.lastName = name[1];
            this.wanted = wanted;
            this.citations = citations;
            this.notes = notes;
        }
    }
}
