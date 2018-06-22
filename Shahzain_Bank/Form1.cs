using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;

namespace Shahzain_Bank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Shahzain_Bank.Properties.Settings.Default.first == true)
            {
                Form2 pas = new Form2();
                pas.ShowDialog();
            }

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





        }



        private void refreshlist()
        {
            listView1.Items.Clear();
            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq;

            sq = new SQLiteCommand("select * from bankdb", scn);
            SQLiteDataReader dr = sq.ExecuteReader();
            dr.Read();
            while (dr.Read())
            {
                listView1.Items.Add(new ListViewItem(new[] { dr["id"].ToString(),
                                                             dr["name"].ToString(),
                                                             dr["bank"].ToString(),
                                                             dr["date"].ToString(),
                                                             dr["info"].ToString(),                                                             
                                                             dr["crd"].ToString(),
                                                             dr["deb"].ToString()}));
            }

            sq = new SQLiteCommand("select sum(deb) from bankdb", scn);
            debitsum.Text = sq.ExecuteScalar().ToString();

            sq = new SQLiteCommand("select sum(crd) from bankdb", scn);
            creditsum.Text = sq.ExecuteScalar().ToString();


            scn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
            scn.Open();
            SQLiteCommand sq;

            sq = new SQLiteCommand("select sum(crd) from bankdb where bank='" + bankbox.Text + "' and date < '" + dateTimePicker2.Text+"'", scn);
            int sum_credit_prev = int.Parse((sq.ExecuteScalar().ToString() == "") ? "0" : sq.ExecuteScalar().ToString());

            sq = new SQLiteCommand("select sum(deb) from bankdb where bank='" + bankbox.Text + "' and date < '" + dateTimePicker2.Text + "'", scn);
            int sum_debit_prev = int.Parse((sq.ExecuteScalar().ToString() == "") ? "0" : sq.ExecuteScalar().ToString());

            prevBal.Text = (sum_credit_prev - sum_debit_prev).ToString();

            sq = new SQLiteCommand("select * from bankdb where date between '"+dateTimePicker2.Text +"' and '"+dateTimePicker3.Text+"' and bank='" + bankbox.Text+"'", scn);
            SQLiteDataReader dr = sq.ExecuteReader();
         
            while (dr.Read())
            {
               
                listView1.Items.Add(new ListViewItem(new[] { dr["id"].ToString(),
                                                             dr["name"].ToString(),
                                                             dr["bank"].ToString(),
                                                             dr["date"].ToString(),
                                                             dr["info"].ToString(),
                                                             dr["crd"].ToString(),
                                                             dr["deb"].ToString(),"0"}));
            }
           

            sq = new SQLiteCommand("select sum(deb) from bankdb where bank='" + bankbox.Text + "'", scn);
            debitsum.Text = (sq.ExecuteScalar().ToString() == "") ? "0" : sq.ExecuteScalar().ToString();
            sq = new SQLiteCommand("select sum(crd) from bankdb where bank='" + bankbox.Text + "'", scn);
            creditsum.Text = (sq.ExecuteScalar().ToString() == "") ? "0" : sq.ExecuteScalar().ToString();

            scn.Close();

            int total;
            int recieve;
            int balanceBefore;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                total = Convert.ToInt32(listView1.Items[i].SubItems[5].Text);
                recieve = Convert.ToInt32(listView1.Items[i].SubItems[6].Text);

                if (i == 0)
                {
                    listView1.Items[0].SubItems[7].Text = (total + double.Parse(prevBal.Text) - recieve).ToString();
                }
                else
                {
                    balanceBefore = Convert.ToInt32(listView1.Items[i - 1].SubItems[7].Text);
                    listView1.Items[i].SubItems[7].Text = (balanceBefore + total - recieve).ToString();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Add_dc add = new Add_dc();
            add.Show();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Banks bnk = new Banks();
            bnk.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ExportToExcel("ok.csv", listView1);
        }

        private void ExportToExcel(string path, ListView listsource)
        {
            StringBuilder CVS = new StringBuilder();
            for (int i = 0; i < listsource.Columns.Count; i++)
            {
                CVS.Append(listsource.Columns[i].Text + ",");
            }
            CVS.Append(Environment.NewLine);
            for (int i = 0; i < listsource.Items.Count; i++)
            {
                for (int j = 0; j < listsource.Columns.Count; j++)
                {
                    CVS.Append(listsource.Items[i].SubItems[j].Text + ",");
                }
                CVS.Append(Environment.NewLine);
            }
            System.IO.File.WriteAllText(path, CVS.ToString());
            Process.Start(path);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Confrimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No)
            {
                return;
            }


            if (listView1.SelectedItems.Count > 0)
            {
                SQLiteConnection scn = new SQLiteConnection(@"data source = dbbank.db");
                scn.Open();
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    SQLiteCommand sq = new SQLiteCommand("delete from bankdb where id='" + listView1.SelectedItems[i].SubItems[0].Text + "'", scn);
                    sq.ExecuteNonQuery();
                }
                scn.Close();
                      
            }

            button4.PerformClick();
        }
    }
}
