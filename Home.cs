using System;
using System.Windows.Forms;

namespace Vela31_Ineo
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Model_search_MouseClick(object sender, MouseEventArgs e)
        {
            this.text_model_search.SelectAll();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.text_ip_address.SelectAll();
        }

        private void radio_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton)sender;
            if (selection.Checked)
            {
                if (selection == radio_color_yes)
                {
                    // Afficher seulement les copieurs couleurs dans la listview
                    MessageBox.Show("Copieur couleur");
                }
                else if (selection == radio_color_no)
                {
                    // Afficher seulement les copieurs noir et blanc dans la listview
                    MessageBox.Show("Copieur N/B");
                }
            }
        }

        private void radio_format_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton)sender;
            if (selection.Checked)
            {
                if (selection == radio_a3)
                {
                    MessageBox.Show("Copieur A3");
                }
                else if (selection == radio_a4)
                {
                    MessageBox.Show("Copieur A4");
                }
            }
        }
    }
}
