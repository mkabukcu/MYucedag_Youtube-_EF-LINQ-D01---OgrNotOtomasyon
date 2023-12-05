using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OgrNotOtomasyon
{
    public partial class LinqExercisesOnOgrNot : Form
    {
        public LinqExercisesOnOgrNot()
        {
            InitializeComponent();
        }
        OgrNotOtomasyonEntities db = new OgrNotOtomasyonEntities();

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbSinav150denKucuk.Checked)
            {
                //dataGridView1.DataSource = db.M_NOTLAR.Where(p => p.SINAV1 < 50).ToList();

                var sorgu = from not in db.M_NOTLAR
                            join ogr in db.M_OGRENCI
                             on not.OGR equals ogr.ID
                            join ders in db.M_DERSLER
                             on not.DERS equals ders.DERSID
                            where not.SINAV1 < 50
                            select new
                            {
                                ÖĞRENCİ = ogr.AD + " " + ogr.SOYAD,
                                DERS = ders.DERSAD,
                                SINAV1 = not.SINAV1,
                                SINAV2 = not.SINAV2,
                                SINAV3 = not.SINAV3,
                                ORTALAMA = not.ORTALAMA,
                                DURUM = not.DURUM
                            };
                dataGridView1.DataSource = sorgu.ToList();
            }

            else if (rbAdaGoreNotSorgula.Checked)
            {
                var sorgu = from not in db.M_NOTLAR
                            join ogr in db.M_OGRENCI
                             on not.OGR equals ogr.ID
                            join ders in db.M_DERSLER
                             on not.DERS equals ders.DERSID
                            where ogr.AD.Contains(txtSearchText.Text)
                            select new
                            {
                                ÖĞRENCİ = ogr.AD + " " + ogr.SOYAD,
                                DERS = ders.DERSAD,
                                SINAV1 = not.SINAV1,
                                SINAV2 = not.SINAV2,
                                SINAV3 = not.SINAV3,
                                ORTALAMA = not.ORTALAMA,
                                DURUM = not.DURUM
                            };
                dataGridView1.DataSource = sorgu.ToList();
            }
            else if (rbAdSoyadaGoreOgrSorgula.Checked)
            {
                //Ada Soyada göre filtreleyip tabloyu olduğu gibi listele
                //dataGridView1.DataSource = db.M_OGRENCI
                //    .Where(ogr => ogr.AD.Contains(txtSearchText.Text) || ogr.SOYAD.Contains(txtSearchText.Text)).ToList();

                // Ada Soyada göre filtreleyip anonymous type ile istediğim alanları getir
                dataGridView1.DataSource = db.M_OGRENCI
                    .Where(ogr => ogr.AD.Contains(txtSearchText.Text) || ogr.SOYAD.Contains(txtSearchText.Text))
                    .Select(ogr => new { AdSoyad = ogr.AD + " " + ogr.SOYAD })
                    .ToList();
            }
            else if (rbGectiKaldi.Checked)
            {
                var query = db.M_NOTLAR.Select(not =>
                new
                {
                    OgrenciAd = not.M_OGRENCI.AD + " " + not.M_OGRENCI.SOYAD,
                    Ortalama = not.ORTALAMA,
                    Durumu = not.DURUM == true ? "Geçti" : "Kaldı"
                }
                );
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbBirlestir.Checked)
            {
                var query = db.M_NOTLAR.SelectMany(not => db.M_OGRENCI.Where(ogr => ogr.ID == not.OGR),
                    (not, ogr) =>
                    new
                    {
                        not.NOTID,
                        ogr.AD,
                        ogr.SOYAD,
                        not.SINAV1,
                        not.SINAV2,
                        not.SINAV3,
                        not.ORTALAMA,
                        Durumu = not.DURUM == true ? "Geçti" : "Kaldı"
                    }
                    );
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbIlkUc.Checked)
            {
                var query = db.M_OGRENCI.OrderBy(ogr => ogr.ID).Take(3);
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbSonUc.Checked)
            {
                var query = db.M_OGRENCI.OrderByDescending(ogr => ogr.ID).Take(3);
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbAdaGoreSirala.Checked)
            {
                var query = db.M_OGRENCI.OrderBy(ogr => ogr.AD).ThenBy(ogr2 => ogr2.SOYAD);
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbIlkBesDegeriAtla.Checked)
            {
                var query = db.M_OGRENCI.OrderBy(ogr => ogr.ID).Skip(5);
                dataGridView1.DataSource = query.ToList();
            }
            else if (rbHerSehirdeKacOgrenci.Checked)
            {
                var query = db.M_OGRENCI.GroupBy(ogr => ogr.SEHIR).Select(x => new { Şehir = x.Key, OgrenciSayisi = x.Count() }).OrderBy(y => y.OgrenciSayisi);
                dataGridView1.DataSource = query.ToList();
            }
        }

        private void btnMaxOrt_Click(object sender, EventArgs e)
        {
            lblResult.Text = db.M_NOTLAR.Max(x => x.ORTALAMA).ToString();
        }

        private void btnMinOrt_Click(object sender, EventArgs e)
        {
            lblResult.Text = db.M_NOTLAR.Min(x => x.SINAV1).ToString();
        }

        private void btnGecenMinOrt_Click_1(object sender, EventArgs e)
        {
            lblResult.Text = db.M_NOTLAR.Where(not => not.DURUM == true).Min(gecen => gecen.ORTALAMA).ToString();
        }

        private void btnNotSay_Click(object sender, EventArgs e)
        {
            lblResult.Text = db.M_NOTLAR.Count(not => not.SINAV1 != null && not.SINAV2 != null /*&& not.SINAV3 != null */).ToString();
        }

        private void btnOrtHesaplaWithSearch_Click(object sender, EventArgs e)
        {
            lblResult.Text = db.M_NOTLAR.Where(a => a.M_OGRENCI.AD == txtSearchText.Text).Average(x => x.SINAV1).ToString();
        }

        private void
            btnMaxs1s2_Click(object sender, EventArgs e)
        {
            //lblResult.Text = db.M_NOTLAR.Max(x => x.SINAV1 + x.SINAV2).ToString();

            lblResult.Text =
                (from y in db.M_OGRENCI
                 where y.ID ==
(from x in db.M_NOTLAR orderby (x.SINAV1 + x.SINAV2) descending select x.OGR).FirstOrDefault()
                 select y.AD + " " + y.SOYAD
                ).FirstOrDefault().ToString();

            lblResult.Text = db.M_OGRENCI
                .Where(o => o.ID == db.M_NOTLAR.OrderByDescending(n => n.SINAV1 + n.SINAV2).Select(n => n.OGR).FirstOrDefault())
                .Select(o => o.AD + " " + o.SOYAD)
                .FirstOrDefault();

            var sonuc = db.M_NOTLAR.Join(db.M_OGRENCI, n => n.OGR, o => o.ID, (n, o) => new { Ogrenci = o, Not = n })
                // Kulübü de eklemek için ---> .Join(db.M_KULUPLER, onceki => onceki.Ogrenci.OGRKULUP, k => k.KULUPID, (onceki, k) => new { Ogrenci = onceki.Ogrenci, Not = onceki.Not, Kulup = k })
                .OrderByDescending(x=> x.Not.SINAV1 + x.Not.SINAV2)
                .FirstOrDefault();

            

            var result = (from o in db.M_OGRENCI
                          join n in db.M_NOTLAR on o.ID equals n.OGR
                          orderby (n.SINAV1 + n.SINAV2) descending
                          select new { Ogrenci = o, Not = n }
                ).FirstOrDefault();



        }
    }
}
