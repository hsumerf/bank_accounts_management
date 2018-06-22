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
    public partial class Add_dc : Form
    {
        public Add_dc()
        {
            InitializeComponent();
        }

        private void Add_dc_Load(object sender, EventArgs e)
        {
            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq;
            bankbox.Items.Clear();
            sq = new SQLiteCommand("select * from banknames", scn);
            SQLiteDataReader dr = sq.ExecuteReader();
     
            while (dr.Read())
            {
                bankbox.Items.Add(dr["bankname"].ToString());
            }
            scn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int debit = 0, credit = 0;
            if (radioButton1.Checked)
                debit = Convert.ToInt32(amountbox.Text);
            else
                credit = Convert.ToInt32(amountbox.Text);

            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq = new SQLiteCommand(String.Format("insert into bankdb (name,deb,info,bank,date,crd) values ('{0}','{1}','{2}','{3}','{4}','{5}')", namebox.Text, debit, infobox.Text, bankbox.Text, dateTimePicker1.Text, credit), scn);
            sq.ExecuteNonQuery();
            scn.Close();
        }
    }
}
