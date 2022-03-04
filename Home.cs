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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Model_search_MouseClick(object sender, MouseEventArgs e)
        {
            this.Model_search.SelectAll();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            
        }
    }
}
