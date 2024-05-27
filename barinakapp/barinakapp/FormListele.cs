using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace barinakapp
{
    public partial class FormListele : Form
    {
        public FormListele()
        {
            InitializeComponent();
        }
        string baglanti = "Server=localhost;Database=barinak;Uid=root;Pwd='';";
        string hedefDosya = "";
        private void VeriGetir( string sql)
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {


                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Invalidate();
                dataGridView1.Refresh();

            }
        }

        private void FormListele_Load(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM hayvanlar";
            VeriGetir(sorgu);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                comboBox1.Text = dataGridView1.CurrentRow.Cells["tur"].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells["cins"].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells["ad"].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells["dogumyil"].Value.ToString();
                checkBox1.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["saglikli"].Value);
                pictureBox1.ImageLocation = dataGridView1.CurrentRow.Cells["resim"].Value.ToString();

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {

                string sql = "UPDATE hayvanlar SET tur=@tur,cins=@cins,ad=@ad,dogumyil=@dogumyil,saglikli=@saglikli,resim=@resim WHERE id=@id";

                con.Open();



                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@tur", comboBox1.Text);
                cmd.Parameters.AddWithValue("@cins", textBox1.Text);
                cmd.Parameters.AddWithValue("@ad", textBox2.Text);
                cmd.Parameters.AddWithValue("@dogumyil", textBox3.Text);
                cmd.Parameters.AddWithValue("@saglikli", checkBox1.Checked);
                cmd.Parameters.AddWithValue("@resim", hedefDosya);
                cmd.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["id"].Value.ToString());



                DialogResult result = MessageBox.Show("Güncellensin mi?", "Güncelle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    VeriGetir("SELECT *FROM hayvanlar");

                }



            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png| Video|*.avi| Tüm Dosyalar |*.*";
            dosya.Title = "Dosya Seçiniz";

            if (dosya.ShowDialog() == DialogResult.OK)
            {

                if (!Directory.Exists("resimler"))
                {
                    Directory.CreateDirectory("resimler");
                }
                string kaynakDosya = dosya.FileName;
                hedefDosya = Path.Combine("resimler", Guid.NewGuid() + ".jpg");

                File.Copy(kaynakDosya, hedefDosya);

                pictureBox1.ImageLocation = hedefDosya;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                string sql = "DELETE FROM hayvanlar WHERE id=@id";
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", secilenId);

                //cmd.ExecuteNonQuery();

                DialogResult result = MessageBox.Show("Kayıt silinsin mi?", "Kayıt Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    VeriGetir("SELECT *FROM hayvanlar");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string aranan = textBox5.Text;
            string sql = "SELECT * FROM hayvanlar WHERE cins LIKE '%" + aranan + "%'";


            VeriGetir(sql);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                //A-Z Sırala

                string sql = "SELECT * FROM hayvanlar ORDER BY ad ASC";

                VeriGetir(sql);

                

            }
            else
            {
                //Z-A Sırala

                string sql = "SELECT * FROM hayvanlar ORDER BY ad DESC";

                VeriGetir(sql);
            }
        }
    }
}
