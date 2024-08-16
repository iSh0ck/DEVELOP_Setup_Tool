using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vela31_Ineo
{
    public partial class MFP_Found : Form
    {
        public MFP_Found()
        {
            InitializeComponent();
        }

        private void btn_select_found_printer_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                Home.text_ip_address.Text = listView1.SelectedItems[0].Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select 1 MFP");
            }
        }
    }
}
