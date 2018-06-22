using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shahzain_Bank
{
    public partial class Banks : Form
    {
        public Banks()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Confrimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq = new SQLiteCommand("delete from banknames where bankname='" + listBox1.Text + "'", scn);
            sq.ExecuteNonQuery();

            refreshlist();
        }

        private void refreshlist()
        {
            listBox1.Items.Clear();
            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq;
            sq = new SQLiteCommand("select * from banknames", scn);
            SQLiteDataReader dr = sq.ExecuteReader();
  
            listBox1.BeginUpdate();
            while (dr.Read())
            {
                listBox1.Items.Add(dr["bankname"].ToString());
            }
            listBox1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq = new SQLiteCommand("insert into banknames(bankname) values('" + textBox1.Text + "')", scn);
            try
            {
                sq.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("This name already exisit");
            }
            


            refreshlist();

        }

        private void Banks_Load(object sender, EventArgs e)
        {
            refreshlist();
        }
    }
}
