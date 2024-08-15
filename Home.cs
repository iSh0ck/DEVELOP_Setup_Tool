using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vela31_Ineo
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            combo_smb.SelectedIndex = 0;
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
            text_ip_address.SelectAll();
        }

        /* Modification de la liste via des radio buttons
         * 
         * private void Radio_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton)sender;

            if (selection.Checked)
            {
                if (selection == radio_color_yes)
                {
                    // Afficher seulement les copieurs couleurs dans la listview

                    // Regénérer la liste
                    if (radio_a3.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddColorPrinters(model_list);
                        Home.AddA3Printers();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[1].Text == "No" || item.SubItems[2].Text == "A4")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else if (radio_a4.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddColorPrinters(model_list);
                        Home.AddA4Printers();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[1].Text == "No" || item.SubItems[2].Text == "A3")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        model_list.Items.Clear();
                        Home.AddColorPrinters(model_list);
                    }
                }
                else if (selection == radio_color_no)
                {
                    // Afficher seulement les copieurs noir et blanc dans la listview

                    // Regénérer la liste
                    if (radio_a3.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddNonColorPrinters();
                        Home.AddA3Printers();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[1].Text == "Yes" || item.SubItems[2].Text == "A4")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else if (radio_a4.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddNonColorPrinters();
                        Home.AddA4Printers();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[1].Text == "Yes" || item.SubItems[2].Text == "A3")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        model_list.Items.Clear();
                        Home.AddNonColorPrinters();
                    }
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
                    // Afficher seulement les copieurs A3

                    // Regénérer la liste
                    if (radio_color_yes.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddA3Printers();
                        Home.AddColorPrinters(model_list);

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[2].Text == "A4" || item.SubItems[1].Text == "No")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else if (radio_color_no.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddA3Printers();
                        Home.AddNonColorPrinters();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[2].Text == "A4" || item.SubItems[1].Text == "Yes")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        model_list.Items.Clear();
                        Home.AddA3Printers();
                    }
                }
                else if (selection == radio_a4)
                {
                    // Afficher seulement les copieurs A4

                    // Regénérer la liste
                    if (radio_color_yes.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddA4Printers();
                        Home.AddColorPrinters(model_list);

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[2].Text == "A3" || item.SubItems[1].Text == "No")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else if (radio_color_no.Checked)
                    {
                        model_list.Items.Clear();
                        Home.AddA4Printers();
                        Home.AddNonColorPrinters();

                        foreach (ListViewItem item in model_list.Items)
                        {
                            if (item.SubItems[2].Text == "A3" || item.SubItems[1].Text == "Yes")
                            {
                                model_list.Items.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        model_list.Items.Clear();
                        Home.AddA4Printers();
                    }
                }
            }
        }

        private void Radio_mode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton)sender;

            if (selection.Checked)
            {
                if (selection == radio_recto_only)
                {
                    MessageBox.Show("Impression recto par défaut");
                }
                else if (selection == radio_recto_verso)
                {
                    MessageBox.Show("Impression R/V par défaut");
                }
            }
        }

        private void Radio_default_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selection = (RadioButton)sender;

            if (selection.Checked)
            {
                if (selection == radio_default_color)
                {
                    MessageBox.Show("Impression en couleur par défaut");
                }
                else if (selection == radio_default_black)
                {
                    MessageBox.Show("Impression en N/B par défaut");
                }
            }
        }*/

        private void Text_model_search_TextChanged(object sender, EventArgs e)
        {
            ListViewItem foundItem = model_list.FindItemWithText(text_model_search.Text, false, 0, true);

            if (foundItem != null)
            {
                model_list.TopItem = foundItem;
            }
        }

        private async void Start_install_btn_Click(object sender, EventArgs e)
        {
            // Actions lors du clic sur le bouton

            // Vérification si activation du SMB seulement
            if (check_smb_only.Checked)
            {
                if (this.combo_smb != null && this.combo_smb.SelectedItem != null)
                {
                    // Paramétrage du SMB
                    Program.SetupSMB(this.combo_smb.SelectedItem.ToString());

                    /*// Vérification si activation du SMB v1.0
                    if (this.check_smb1.Checked)
                    {
                        Program.EnableSMB1();
                    }*/
                    return;
                }
                else
                {
                    MessageBox.Show("Please select an SMB option");
                    return;
                }
            }

            // Vérification du formulaire
            if (model_list.SelectedItems.Count == 1)
            {
                if (text_ip_address != null && text_ip_address.Text != "" && text_ip_address.Text != "Printer IP Address")
                {
                    if (this.combo_smb != null && this.combo_smb.SelectedItem != null)
                    {
                        if (!chk_offlineMode.Checked)
                        {
                            // Téléchargement du driver
                            await Program.DownloadDriver("Windows 10", model_list.SelectedItems[0].Text);

                            // Unzip du driver dans le répertoir Download
                            Program.UnzipArchive(Directory.GetCurrentDirectory() + @"\Download\driver.zip");
                        }

                        // Installation du driver
                        await Program.InstallDriver(model_list.SelectedItems[0].Text, text_ip_address.Text, chk_offlineMode.Checked);

                        if (this.combo_smb.SelectedItem != null)
                        {
                            // Paramétrage du SMB
                            Program.SetupSMB(this.combo_smb.SelectedItem.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Please select an SMB option");
                        }

                        /*// Vérification si activation du SMB v1.0
                        if (this.check_smb1.Checked)
                        {
                            Program.EnableSMB1();
                        }*/
                    }
                    else
                    {
                        MessageBox.Show("Please select an SMB option");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid IP address");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please select a model in the model list");
                return;
            }
        }

        private void check_smb1_CheckedChanged(object sender, EventArgs e)
        {
            /*if (this.check_smb1.Checked)
            {
                    MessageBox.Show("Please select this option if you want to use SMBv1.\n" +
                                    "- By activating this option the installation will take more time.\n" +
                                    "- At the end of the installation you may have to restart the computer\n" +
                                    "- You should only use this option on the older printer ranges");
            }*/
        }

        private void txt_adminPass_Click(object sender, EventArgs e)
        {
            txt_adminPass.SelectAll();
        }

        private void txt_contactName_Click(object sender, EventArgs e)
        {
            txt_contactName.SelectAll();
        }

        private void combo_smb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (combo_smb.SelectedIndex)
            {
                case 0:
                    /*txt_adminPass.Enabled = false;*/
                    txt_contactName.Enabled = false;
                    chk_sendToPrinter.Enabled = false;
                    chk_sendToPrinter.Checked = false;
                    break;
                case 1:
                    chk_sendToPrinter.Enabled = true;
                    break;
            }
        }

        private void chk_sendToPrinter_CheckedChanged(object sender, EventArgs e)
        {
            switch (chk_sendToPrinter.Checked)
            {
                case false:
                    /*txt_adminPass.Enabled = false;*/
                    txt_contactName.Enabled = false;
                    break;
                case true:
                    txt_contactName.Enabled = true;
                    break;
            }
        }

        private void btn_SearchPrinter_MouseEnter(object sender, EventArgs e)
        {
            this.tt_btn_SearchPrinter.SetToolTip(btn_SearchPrinter, "Find printers on network using SNMP.");
        }

        private void btn_SearchPrinter_Click(object sender, EventArgs e)
        {
            Program.FindPrinters();
        }
    }
}
