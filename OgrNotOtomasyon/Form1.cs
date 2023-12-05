using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OgrNotOtomasyon
{
    public partial class Form1 : Form
    {
        OgrNotOtomasyonEntities db = new OgrNotOtomasyonEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDersListele_Click(object sender, EventArgs e)
        {
            // 1 - Sql Command kullanarak
            SqlConnection connection = new SqlConnection(@"Data Source=S001PGVT02\S001PGVT01;Initial Catalog=AdventureWorks;Integrated Security=True");
            SqlCommand command = new SqlCommand("Select * from M_DERSLER", connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            // !!!!! Yukarıdaki datırlarda Sql Command ile yapılan iş, entity framework kullanarak 1 satırda aşağıdaki gibi yapılabiliyor
            //dataGridView1.DataSource = db.M_DERSLER.ToList();
        }

        private void btnOgrListele_Click(object sender, EventArgs e)
        {
            // 2 - Entity FM kullanarak
            dataGridView1.DataSource = db.M_OGRENCI.ToList();

            // data grid viewda bazı kolonları göstermek istemeyebiliriz. Bunun için;
            // YÖNTEM 1
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            // YÖNTEM 2 - Anonymous type tanımlayıp onun üzerinde entity framework sorgusu yazarak da yapabiliriz.
            // private void btnNotListele_Click(object sender, EventArgs e) metodunda olduğu gibi

        }
        private void btnSpNotListesi_Click(object sender, EventArgs e)
        {
            // sp çağırarak not listesini çek
            dataGridView1.DataSource = db.NOTLISTESI();
        }
        private void btnNotListeleLinq_Click(object sender, EventArgs e)
        {
            // dataGridView1.DataSource = db.M_NOTLAR.ToList();

            /********** LINQ QUERY SYNTAX **********/
            var query = from item in db.M_NOTLAR
                        select new
                        {
                            item.NOTID,
                            ÖĞRENCİ = item.M_OGRENCI.AD + " " + item.M_OGRENCI.SOYAD,
                            item.M_DERSLER.DERSAD,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA,
                            DURUM = item.DURUM == true ? "Geçti" : "Kaldı"
                        };
            dataGridView1.DataSource = query.ToList();


            /********** LINQ METHOD SYNTAX **********/
            //var notlist = db.M_NOTLAR
            //    .Select(not => new
            //    {
            //        not.NOTID,
            //        ÖĞRENCİ = not.M_OGRENCI.AD + " " + not.M_OGRENCI.SOYAD,
            //        DERS = not.M_DERSLER.DERSAD,
            //        SINAV1 = not.SINAV1,
            //        SINAV2 = not.SINAV2,
            //        SINAV3 = not.SINAV3,
            //        ORTALAMA = not.ORTALAMA,
            //        Durumu = not.DURUM == true ? "Geçti" : "Kaldı"
            //    }).ToList();
            //dataGridView1.DataSource = notlist;
        }

        private void btnNotListeleJoin_Click(object sender, EventArgs e)
        {
            var query = from not in db.M_NOTLAR
                        join ogr in db.M_OGRENCI
                         on not.OGR equals ogr.ID
                        join ders in db.M_DERSLER
                         on not.DERS equals ders.DERSID
                        select new
                        {
                            NOTID = not.NOTID,
                            ÖĞRENCİ = ogr.AD + " " + ogr.SOYAD,
                            DERS = ders.DERSAD,
                            SINAV1 = not.SINAV1,
                            SINAV2 = not.SINAV2,
                            SINAV3 = not.SINAV3,
                            ORTALAMA = not.ORTALAMA,
                            Durumu = not.DURUM == true ? "Geçti" : "Kaldı"
                        };
            dataGridView1.DataSource = query.ToList();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            M_OGRENCI ogr = new M_OGRENCI();
            ogr.AD = txtOgrAd.Text;
            ogr.SOYAD = txtOgrSoyad.Text;

            db.M_OGRENCI.Add(ogr);
            db.SaveChanges();

            MessageBox.Show("Öğrenci kaydı yapılmıştır.");
            btnOgrListele_Click(this, new EventArgs());
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrID.Text);
            var ogr = db.M_OGRENCI.Find(id);

            db.M_OGRENCI.Remove(ogr);
            db.SaveChanges();

            MessageBox.Show("Öğrenci kaydı silinmiştir.");
            btnOgrListele_Click(this, new EventArgs());
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrID.Text);
            var ogr = db.M_OGRENCI.Find(id);

            ogr.AD = txtOgrAd.Text;
            ogr.SOYAD = txtOgrSoyad.Text;
            ogr.FOTOGRAF = txtOgrFotograf.Text;

            db.SaveChanges();

            MessageBox.Show("Öğrenci kaydı güncellenmiştir.");
            btnOgrListele_Click(this, new EventArgs());
        }

        private void txtOgrAd_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtOgrAd.Text;
            var ogrList = from ogr in db.M_OGRENCI
                          where ogr.AD.Contains(searchText)
                          select ogr;
            dataGridView1.DataSource = ogrList.ToList();
        }

        private void btnLinqEntity_Click(object sender, EventArgs e)
        {
            if (rbAdaGoreSirala.Checked)
            {
                // ascending
                List<M_OGRENCI> ogrList = db.M_OGRENCI.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbAdaGoreTersten.Checked)
            {
                // descending
                List<M_OGRENCI> ogrList = db.M_OGRENCI.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbIlkOn.Checked)
            {
                List<M_OGRENCI> ogrList = db.M_OGRENCI.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbIDyeGore.Checked)
            {
                List<M_OGRENCI> ogrList = db.M_OGRENCI.Where(x => x.ID.ToString() == txtOgrID.Text).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbAdAIleBaslayan.Checked)
            {
                List<M_OGRENCI> ogrList = db.M_OGRENCI.Where(x => x.AD.StartsWith("A")).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbAdAIleBiten.Checked)
            {
                List<M_OGRENCI> ogrList = db.M_OGRENCI.Where(x => x.AD.EndsWith("A")).ToList();
                dataGridView1.DataSource = ogrList;
            }
            else if (rbOgrVarMi.Checked)
            {
                bool varMi = db.M_OGRENCI.Any();
                if (!varMi)
                {
                    MessageBox.Show("Öğrenci bulunamadı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    List<M_OGRENCI> ogrList = db.M_OGRENCI.ToList();
                    dataGridView1.DataSource = ogrList;
                    MessageBox.Show("Öğrenciler listelenmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (rbOgrSay.Checked)
            {
                List<M_OGRENCI> ogrList = db.M_OGRENCI.ToList();
                dataGridView1.DataSource = ogrList;

                // int sayi = ogrList.Count;
                MessageBox.Show("Öğrenci Sayısı : " + db.M_OGRENCI.Count().ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rbSinav1Sum.Checked)
            {
                var sum = db.M_NOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show("Sınav 1 Notlar Toplamı : " + sum, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rbSinav1Average.Checked)
            {
                var average = db.M_NOTLAR.Average(p => p.SINAV1);
                MessageBox.Show("Sınav 1 Notlar Ortalaması : " + average, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rbNotuS1OrtdanBuyuk.Checked)
            {

                // List<M_NOTLAR> ogrList = db.M_NOTLAR.Where(p => p.SINAV1 > db.M_NOTLAR.Average(x => x.SINAV1)).ToList();

                var average = db.M_NOTLAR.Average(p => p.SINAV1);
                var notlist = db.M_NOTLAR
                    .Where(p => p.SINAV1 > average)
                    .Select(not => new
                    {
                        not.NOTID,
                        ÖĞRENCİ = not.M_OGRENCI.AD + " " + not.M_OGRENCI.SOYAD,
                        DERS = not.M_DERSLER.DERSAD,
                        SINAV1 = not.SINAV1,
                        SINAV2 = not.SINAV2,
                        SINAV3 = not.SINAV3,
                        ORTALAMA = not.ORTALAMA,
                        DURUM = not.DURUM == true ? "Geçti" : "Kaldı"
                    }).ToList();

                dataGridView1.DataSource = notlist;
                MessageBox.Show("Sınav 1 Notlar Ortalaması : " + average, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (rbSinav1Max.Checked)
            {
                List<M_NOTLAR> notList = db.M_NOTLAR.OrderByDescending(p => p.SINAV1).Take(1).ToList();
                dataGridView1.DataSource = notList;
                MessageBox.Show("En Yüksek Sınav 1 Notu Olan Öğrenci : " + notList[0].M_OGRENCI.AD + ' ' + notList[0].M_OGRENCI.SOYAD, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOrtalamaHesapla_Click(object sender, EventArgs e)
        {
            List<M_NOTLAR> notList = db.M_NOTLAR.ToList();
            foreach (M_NOTLAR item in notList)
            {
                db.M_NOTLAR.Attach(item);
                item.ORTALAMA = (item.SINAV1 + item.SINAV2) /* + item.SINAV3*/ / 2;
            }
            db.SaveChanges();    
        }
    }
}
