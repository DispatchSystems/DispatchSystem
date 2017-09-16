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
    public partial class BoloView : MaterialForm
    {
        Dictionary<int, (string, string)> bolos = new Dictionary<int, (string, string)>();

        public BoloView(string data)
        {
            InitializeComponent();

            ParseInformation(data);
            UpdateInformation();
        }

        void UpdateInformation()
        {
            List<ListViewItem> lvis = new List<ListViewItem>();

            foreach (var item in bolos)
            {
                ListViewItem lvi = new ListViewItem(item.Key.ToString());
                lvi.SubItems.Add(item.Value.Item1.ToString());
                lvi.SubItems.Add(item.Value.Item2.ToString());
                lvis.Add(lvi);
            }

            bolosView.Items.AddRange(lvis.ToArray());
        }

        void ParseInformation(string data)
        {
            string[] main = data.Split('|');

            if (main[0] == "?")
                return;

            foreach (var item in main)
            {
                string[] other = item.Split('\\');

                int index = int.Parse(other[0]) + 1;
                string[] other2 = other[1].Split(':');
                string name = other2[0];
                string desc = other2[1];

                bolos.Add(index, (name, desc));
            }
        }
    }
}
