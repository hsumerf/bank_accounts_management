using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shahzain_Bank
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text.EndsWith(DateTime.Today.Date.Day.ToString("d")))
            {
                Shahzain_Bank.Properties.Settings.Default.first = false;
                Shahzain_Bank.Properties.Settings.Default.Save();
                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
