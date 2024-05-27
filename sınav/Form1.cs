using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sınav
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=muzik;Uid=root;Pwd='';";

        void VeriGoster()
        {
            using (MySqlConnection baglanti = new MySqlConnection(connectionString))
            {
                string sorgu = "SELECT *FROM sarkilar";

                baglanti.Open();

                MySqlCommand komut = new MySqlCommand(sorgu, baglanti);

                MySqlDataAdapter adapter = new MySqlDataAdapter(komut);

                DataTable tablo = new DataTable();

                adapter.Fill(tablo);

                dataGridView1.DataSource = tablo;

                dataGridView1.Invalidate();
                dataGridView1.Refresh();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VeriGoster();
        }
    }
}
