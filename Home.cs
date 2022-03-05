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

        private void TextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.text_ip_address.SelectAll();
        }

        private void Radio_color_CheckedChanged(object sender, EventArgs e)
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

        private void Radio_format_CheckedChanged(object sender, EventArgs e)
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

        private void Radio_mode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton) sender;

            if (selection.Checked)
            {
                if (selection == radio_recto_only)
                {
                    MessageBox.Show("Impression recto par défaut");
                } else if (selection == radio_recto_verso)
                {
                    MessageBox.Show("Impression R/V par défaut");
                }
            }
        }

        private void radio_default_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton) sender;

            if (selection.Checked)
            {
                if (selection == radio_default_color)
                {
                    MessageBox.Show("Impression en couleur par défaut");
                } else if (selection == radio_default_black)
                {
                    MessageBox.Show("Impression en N/B par défaut");
                }
            }
        }
    }
}
