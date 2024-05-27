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
    public partial class FormEkle : Form
    {
        public FormEkle()
        {
            InitializeComponent();
        }

        string baglanti = "Server=localhost;Database=barinak;Uid=root;Pwd='';";
        string hedefDosya = "";
        private void FormEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {

                string sql = "INSERT INTO hayvanlar (tur,cins,ad,dogumyil,saglikli,resim) VALUES (@tur,@cins,@ad,@dogumyil,@saglikli,@resim);";

                con.Open();



                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@tur", comboBox1.Text);
                cmd.Parameters.AddWithValue("@cins", textBox1.Text);
                cmd.Parameters.AddWithValue("@ad", textBox2.Text);
                cmd.Parameters.AddWithValue("@dogumyil", textBox3.Text);
                cmd.Parameters.AddWithValue("@saglikli", checkBox1.Checked);
                cmd.Parameters.AddWithValue("@resim", hedefDosya);




                DialogResult result = MessageBox.Show("Film eklensin mi?", "Film Ekle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();

                    comboBox1.SelectedIndex = -1;
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    checkBox1.Checked = false;
                    pictureBox1.Image = null;
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
    }
}
