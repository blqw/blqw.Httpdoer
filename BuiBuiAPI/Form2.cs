using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuiBuiAPI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            cbbLocation.SelectedIndex = 0;
            txtParamString.Focus();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void txtParamString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
            else if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
