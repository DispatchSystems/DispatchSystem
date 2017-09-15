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
        }

        private void ParseCivilian(string data)
        {
            string[] main = data.Split('|');
            string[] name = main[0].Split(',');
            bool wanted = bool.Parse(main[1]);
            int citations = int.Parse(main[2]);
            string[] notes = main[3].Split('\\');

            this.firstName = name[0];
            this.lastName = name[1];
            this.wanted = wanted;
            this.citations = citations;
            this.notes = notes;
        }
    }
}
