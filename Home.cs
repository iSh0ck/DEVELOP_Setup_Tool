﻿using System;
using System.IO;
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
            text_ip_address.SelectAll();
        }

        private void Radio_color_CheckedChanged(object sender, EventArgs e)
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
        }

        private void Text_model_search_TextChanged(object sender, EventArgs e)
        {
            ListViewItem foundItem = model_list.FindItemWithText(text_model_search.Text, false, 0, true);

            if (foundItem != null)
            {
                model_list.TopItem = foundItem;
            }
        }

        private void Start_install_btn_Click(object sender, EventArgs e)
        {
            // Actions lors du clic sur le bouton

            // Vérification si activation du SMB seulement
            if (check_smb_only.Checked)
            {
                if (this.combo_smb != null && this.combo_smb.SelectedItem != null)
                {
                    // Paramétrage du SMB
                    Program.SetupSMB(this.combo_smb.SelectedItem.ToString());

                    // Vérification si activation du SMB v1.0
                    if (this.check_smb1.Checked)
                    {
                        Program.EnableSMB1();
                    }
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
                if (this.text_ip_address != null && this.text_ip_address.Text != "" && this.text_ip_address.Text != "Printer IP Address")
                {
                    if (this.combo_os != null && this.combo_os.SelectedItem != null && this.combo_os.SelectedItem.ToString() != "No")
                    {
                        if (this.combo_smb != null && this.combo_smb.SelectedItem != null)
                        {
                            // Téléchargement du driver
                            Program.DownloadDriver(combo_os.Text, model_list.SelectedItems[0].Text);

                            // Unzip du driver dans le répertoir Download
                            Program.UnzipArchive(Directory.GetCurrentDirectory() + @"\Download\driver.zip");

                            // Installation du driver
                            Program.InstallDriver(model_list.SelectedItems[0].Text, text_ip_address.Text, null, null);

                            if (this.combo_smb.SelectedItem != null)
                            {
                                // Paramétrage du SMB
                                Program.SetupSMB(this.combo_smb.SelectedItem.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Please select an SMB option");
                            }

                            // Vérification si activation du SMB v1.0
                            if (this.check_smb1.Checked)
                            {
                                Program.EnableSMB1();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select an SMB option");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an OS");
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
    }
}
