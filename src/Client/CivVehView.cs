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
    public partial class CivVehView : MaterialForm
    {
        string plate;
        string firstName;
        string lastName;
        bool stolen;
        bool registered;
        bool insured;

        public CivVehView(string civVehData)
        {
            InitializeComponent();

            ParseCivilian(civVehData);
            UpdateCurrentInfromation();
        }

        public void UpdateCurrentInfromation()
        {
            this.plateView.Text = plate;
            this.firstNameView.Text = firstName;
            this.lastNameView.Text = lastName;
            this.stolenView.Checked = stolen;
            this.registrationView.Checked = registered;
            this.insuranceView.Checked = insured;
        }

        private void ParseCivilian(string data)
        {
            string[] main = data.Split('|');
            string plate = main[0];
            string[] name = main[1].Split(',');
            bool stolen = bool.Parse(main[2]);
            bool registered = bool.Parse(main[3]);
            bool insured = bool.Parse(main[4]);

            this.plate = plate;
            this.firstName = name[0];
            this.lastName = name[1];
            this.stolen = stolen;
            this.registered = registered;
            this.insured = insured;

            if (!this.registered)
            {
                firstName = "None";
                lastName = "None";
            }
        }
    }
}
