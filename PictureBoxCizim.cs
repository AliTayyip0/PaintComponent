using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GeometrikSoruCozucu.Components
{
    class PictureBoxCizim : Component
    {

        /*
         * Bu Class Başka Hiç Bir Class'a İhtiyaç Duymaz!
         * 
         * 
         * Kullanıcıya bir component verir ve componen içerisinden kullanıcının bir picturebox seçmesini ister.
         * Kullanıcı picturebox'ı seçtikten sonra class'da bulunan cizim turleri kısmını değişerek yapılacak çizime karar verebilir.
         * Çizimlerini yapan kullanıcı geri alma işlemlerini GeriAl fonksiyonu ile gerçekleştirebilir.
         * Class içerisinde yapılan tüm işlemler kullanıcının konumlandırdığı pictureBox a otomatik yerleştirilecektir.
         * En son çizim kaydedilmek istendiğinde ResmiKaydet Fonksiyonu kayıt edilmek istenilen konum parametresi ile çağırılarak otomatik kayıt işlemi gerçekleştirilebilir
         */



        public PictureBox SelectPictureBox
        {
            get
            {
                return cizimYapilacakPictureBox;
            }
            set
            {
                PictureBox gelenPictureBox = value;
                //cizim_Yapilacak_Pb = value;
                
                resim = new Bitmap(gelenPictureBox.Width / 2, gelenPictureBox.Width / 2);
                geciciResim = new Bitmap(gelenPictureBox.Width / 2, gelenPictureBox.Width / 2);

                using (Graphics g = Graphics.FromImage(resim))
                {
                    //Tüm resmi beyaza zemin yapmak için kullanılıyor
                    Rectangle ImageSize = new Rectangle(0, 0, resim.Width, resim.Height);
                    g.FillRectangle(Brushes.White, ImageSize);
                }
                using (Graphics g1 = Graphics.FromImage(geciciResim))
                {
                    //Tüm resmi beyaza zemin yapmak için kullanılıyor
                    Rectangle ImageSize = new Rectangle(0, 0, geciciResim.Width, geciciResim.Height);
                    g1.FillRectangle(Brushes.White, ImageSize);
                }

                geciciCizimGrafik = Graphics.FromImage(geciciResim);
                temel_Resim = new Bitmap(resim);



                cizimYapilacakPictureBox = PictureBoxOlustur(gelenPictureBox);
                CizimTuruDegistirme(0);
                BoyutOranlamaKatsayilariniTekrarHesapla();



            }
        }



        private PictureBox cizimYapilacakPictureBox;  // Kullanıcının AlacağıPictureBox // TODO: private yapılabilir
        

        
        Bitmap resim; // Ana Tuval

        Bitmap geciciResim; // Geçici Görsellerin Sürekli Üstüne Yazılacağı Tuval

        Graphics geciciCizimGrafik;

        Form form = new Form(); // mouse konumuna erişmek için




        /*
         * 2 adet BitMap ve Graphics olmasının sebebi çizim işleminin 2 sistemle biri geçici çizgileri oluşturan diğeri ise onaylanmış çizgileri tuttuğu için
         * Burada söylenen yanlış anlaşılmasın Resim kısmında yapılan çizimler GeriAl fonksiyonu sayesinde geri alınabilir 
         * Burada geçici bitmap Çizim Sırasında ölçüme yardımcı olan ve fare basılı oldukça değişen çizgiyi tutan ve durmadan değişen sistem
         * en son onaylandığında ise çizim otomatik Resim BitMap'ine yapılıyor
         */






        private bool geciciCizimYapiliyorMu = true;

        

        /// <summary>
        /// Tuvale resim eklenmesi ve resmin boyutunun tuvalle eşitlenmediği durumlarda boyut eşitlemesi yapmak gerekir. Bu işlemi otomatik gerçekleştiren fonksiyon
        /// </summary>
        private void BoyutOranlamaKatsayilariniTekrarHesapla()
        {
            /*
             Bu fonksiyon üzerinde işlem yapılan bitmapin boyutuna oranlamak için kullanılıyor.
             Bir resim eklendiğinde tuvalin boyutu resminkiyle aynı değilse çizimle ilgili kayma sıkıntısı yaşanıyor. 
             Bu sorunu aşmak için katsayı hesapları ve işlemlerin kaysayılarla çarpılarak yapılması gerekiyor.
             */
            katsayi_X = Convert.ToDouble(resim.Size.Width) / Convert.ToDouble(cizimYapilacakPictureBox.Width);

            katsayi_Y = Convert.ToDouble(resim.Size.Height) / Convert.ToDouble(cizimYapilacakPictureBox.Height);
        }



        /// <summary>
        /// Kullanıcının constructureda Pb vermediği durumlarda kullanılır. Sıfırdan Pb oluşturulur kullanıcı bu Pb yi alır ve istediği yere koyar
        /// </summary>
        /// <returns></returns>
        private PictureBox PictureBoxOlustur()
        {
            /*PictureBox oluşturucu fonksiyon*/
            PictureBox pbCalisma = new PictureBox();
            pbCalisma.BackColor = System.Drawing.Color.Transparent;
            pbCalisma.Location = new System.Drawing.Point(0, 309);
            pbCalisma.Dock = System.Windows.Forms.DockStyle.Fill;
            pbCalisma.Name = "pbCalisma";
            pbCalisma.Size = new System.Drawing.Size(93, 263);
            pbCalisma.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pbCalisma.TabIndex = 3;
            pbCalisma.TabStop = false;
            // pbCalisma.Parent = form;

            pbCalisma.MouseDown += pbCalisma_MouseDown;
            pbCalisma.MouseMove += pbCalisma_MouseMove;
            pbCalisma.MouseUp += pbCalisma_MouseUp;
            return pbCalisma;
        }
        /// <summary>
        /// Kullanıcı Pb girdiği durumlarda kullanılacak fonksiyon
        /// </summary>
        /// <param name="pbCalisma"></param>
        /// <returns></returns>
        private PictureBox PictureBoxOlustur(PictureBox pbCalisma)
        {
            /*PictureBox oluşturucu fonksiyon*/
            //pbCalisma.BackColor = System.Drawing.Color.Transparent;
            //pbCalisma.Location = new System.Drawing.Point(0, 309);
            //pbCalisma.Dock = System.Windows.Forms.DockStyle.Fill;
            //pbCalisma.Name = "pbCalisma";
            //pbCalisma.Size = new System.Drawing.Size(93, 263);
            //pbCalisma.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            //pbCalisma.TabIndex = 3;
            //pbCalisma.TabStop = false;
            // pbCalisma.Parent = form;

            //Resim_Ekle(pbCalisma.Image);
            pbCalisma.MouseDown += pbCalisma_MouseDown;
            pbCalisma.MouseMove += pbCalisma_MouseMove;
            pbCalisma.MouseUp += pbCalisma_MouseUp;
            return pbCalisma;
        }











        #region Cizim Türünü Ayarlama



        /*
        Çizim türleri yapılacak çizimin türünü belirlemeye yarar.
        çizgi ,düz çizgi ,yuvarlak, text...
        Burada belirlenen işlem bir enum değerdir. Yalnızca tür belirtir başka herhangi bir işlem yapmaz.
        */



        private enum Cizim_Turleri
        {
            //olası tüm çizim türleri
            none,
            cizgi,
            duz_cizgi,
            daire,
            kare,
            text,
            silgi
        }


        // O an yapılacak olan çizim türünün tutuldupu global değişken
        Cizim_Turleri secili_Cizim = Cizim_Turleri.none;

        Point Nokta1, Nokta2;
        /// <summary>
        /// 0 - seç,    
        /// 1 - çizgi,    
        /// 2 - düz çizgi,    
        /// 5 - Text,     
        /// 6 - Silgi,  
        /// </summary>
        /// <param name="islem_index"></param>
        public void CizimTuruDegistirme(int islem_index)
        {
            // O an yapılacak olan çizim türünü değiştirmek için kullanılan fonksiyon
            Nokta1 = new Point(0, 0);
            if (islem_index == 0)
            {
                secili_Cizim = Cizim_Turleri.none;
            }
            else if (islem_index == 1)
            {
                secili_Cizim = Cizim_Turleri.cizgi;
            }
            else if (islem_index == 2)
            {

                secili_Cizim = Cizim_Turleri.duz_cizgi;
            }
            else if (islem_index == 3)
            {
                secili_Cizim = Cizim_Turleri.daire;
            }
            else if (islem_index == 4)
            {
                secili_Cizim = Cizim_Turleri.kare;
            }
            else if (islem_index == 5)
            {
                secili_Cizim = Cizim_Turleri.text;
                //Cursor = Cursors.IBeam;
            }
            else if (islem_index == 6)
            {
                secili_Cizim = Cizim_Turleri.silgi;
            }
            geciciCizimYapiliyorMu = false;
        }



        #endregion







        #region Kalem Ayarları

        /*
          Bu fonksiyonlar çizim yapılan kalemin ayarlarının değiştirilmesinde kullanılır
          Bu değişiklik kalınlık veya renk türündedir.
        */

        private Pen Kalem = new Pen(Color.Black, 3);


        public void Set_Kalem_Rengi(Color renk)
        {
            Kalem.Color = renk;
        }

        public void Set_Kalem_Kalinligi(float kalinlik)
        {
            Kalem = new Pen(Kalem.Color, kalinlik);
        }


        #endregion

















        #region Çizimler


        /* AÇIKLAMA
         
            if (Gecici_Cizim == true)
            {
                Gecici_Cizim_Grafik.Dispose();
                Gecici_Cizim_Grafik = Graphics.FromImage(Gecici_Cizim_Bm);
            }
            else
            {
                Gecici_Cizim_Grafik.Dispose();
                Gecici_Cizim_Grafik = Graphics.FromImage(Resim);
            }
            
            
            her fonksiyonun içinde bulunan bu kod çizimin gecici yani her fare hareketinde silinip yeniden çizilecek bir çizim mi yoksa çizilip Resim BitMapinde kalıcı olarak yerleştirilecekk bir çizim mi olduğuna karar veriyor
            geçici çizimse GEcici_Cizim_Bm bizim pictureBox ımızda gösteriliyor sabit kalacak çizimse Resime çizilip ekrana Picturebox'a resim bitmap'i veriliyor.

            Kodların içinde çağırılan  BackUpIcin structure'ına veri yazılıp bir listeye ekleniyor
            adındanda anlaşılacağı gibi bu liste backuo yani geri al ctrl + z yapmamızı sağlıyor. 
            TODO: ctrl + y özelliği getmedim kolay hemen yapılabilir
       */


        double katsayi_X = 1.0;
        double katsayi_Y = 1.0;
        bool geri_al = false;


        public void Resim_Ekle(string yol)
        {
            try
            {


                Bitmap gecici = new Bitmap(yol);

                resim = new Bitmap(gecici);
                //resim = new Bitmap(gecici.Width, gecici.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                geciciResim = new Bitmap(resim);

                geciciCizimGrafik = Graphics.FromImage(geciciResim);
                //geciciCizimGrafik.DrawImage(gecici, new Point(0, 0));
                temel_Resim = new Bitmap(resim);



                cizimYapilacakPictureBox.Image = Return_BitMap();


                katsayi_X = Convert.ToDouble(resim.Size.Width) / Convert.ToDouble(cizimYapilacakPictureBox.Width);

                katsayi_Y = Convert.ToDouble(resim.Size.Height) / Convert.ToDouble(cizimYapilacakPictureBox.Height);


                if (geri_al == false && geciciCizimYapiliyorMu == false)
                {
                    BackUpIcin bi = new BackUpIcin();
                    bi.l_resim = new Bitmap(resim);
                    new_BackUpList.Add(bi);
                }
                
                

            }
            catch
            {
                MessageBox.Show("Resim yüklenirken bir sorun çıktı.");
            }
        }
        public void Resim_Ekle(Image resim)
        {

            geciciCizimGrafik = Graphics.FromImage(resim);
            geciciResim = new Bitmap(resim);



            cizimYapilacakPictureBox.Image = Return_BitMap();


            katsayi_X = Convert.ToDouble(resim.Size.Width) / Convert.ToDouble(cizimYapilacakPictureBox.Width);

            katsayi_Y = Convert.ToDouble(resim.Size.Height) / Convert.ToDouble(cizimYapilacakPictureBox.Height);


            if (geri_al == false && geciciCizimYapiliyorMu == false)
            {
                BackUpIcin bi = new BackUpIcin();
                bi.l_resim = new Bitmap(resim);
                new_BackUpList.Add(bi);
            }
        }

        private void Gecici_Duz_Cizgi_Cizi(Point Nokta1, Point Nokta2)
        {
            if (geciciCizimYapiliyorMu == true)
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(geciciResim);
            }
            else
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(resim);
            }


            if (geri_al == false && geciciCizimYapiliyorMu == false)
            {

                BackUpIcin bi = new BackUpIcin();
                bi.l_Tur = secili_Cizim;
                bi.l_Nokta1 = Nokta1;
                bi.l_Nokta2 = Nokta2;
                bi.l_Kalem = Kalem;
                geri_al = false;
                new_BackUpList.Add(bi);

            }


            Nokta1 = new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));
            Nokta2 = new Point(Convert.ToInt32(katsayi_X * Nokta2.X), Convert.ToInt32(katsayi_Y * Nokta2.Y));




            geciciCizimGrafik.DrawLine(Kalem, Nokta1, Nokta2);



        }

        private void Gecici_Daire_Ciz(Point Nokta1, Point Nokta2)
        {
            if (geciciCizimYapiliyorMu == true)
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(geciciResim);
            }
            else
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(resim);
            }

            if (geri_al == false && geciciCizimYapiliyorMu == false)
            {

                BackUpIcin bi = new BackUpIcin();
                bi.l_Tur = secili_Cizim;
                bi.l_Nokta1 = Nokta1;
                bi.l_Nokta2 = Nokta2;
                bi.l_Kalem = Kalem;
                geri_al = false;
                new_BackUpList.Add(bi);

            }




            Nokta1 = new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));
            Nokta2 = new Point(Convert.ToInt32(katsayi_X * Nokta2.X), Convert.ToInt32(katsayi_Y * Nokta2.Y));


            double x_toplam = Math.Abs(Nokta1.X - Nokta2.X);
            double y_toplam = Math.Abs(Nokta1.Y - Nokta2.Y);

            double uzaklık = Math.Sqrt((x_toplam * x_toplam) + (y_toplam * y_toplam));

            Point Nokta3 = Cemberin_Ortasini_Bulma(Nokta1, Nokta2);
            geciciCizimGrafik.DrawEllipse(Kalem, Nokta3.X - Convert.ToInt32(x_toplam / 2), Nokta3.Y - Convert.ToInt32(y_toplam / 2), Convert.ToInt32(uzaklık), Convert.ToInt32(uzaklık));




        }

        private void Gecici_Kare_Ciz(Point Nokta1, Point Nokta2)
        {
            if (geciciCizimYapiliyorMu == true)
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(geciciResim);
            }
            else
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(resim);
            }


            if (geri_al == false && geciciCizimYapiliyorMu == false)
            {

                BackUpIcin bi = new BackUpIcin();
                bi.l_Tur = secili_Cizim;
                bi.l_Nokta1 = Nokta1;//new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));
                bi.l_Nokta2 = Nokta2; //new Point(Convert.ToInt32(katsayi_X * Nokta2.X), Convert.ToInt32(katsayi_Y * Nokta2.Y));
                bi.l_Kalem = Kalem;
                geri_al = false;
                new_BackUpList.Add(bi);

            }


            Nokta1 = new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));
            Nokta2 = new Point(Convert.ToInt32(katsayi_X * Nokta2.X), Convert.ToInt32(katsayi_Y * Nokta2.Y));



            double x_toplam = Math.Abs(Nokta1.X - Nokta2.X);
            double y_toplam = Math.Abs(Nokta1.Y - Nokta2.Y);

            if (Nokta1.X < Nokta2.X)
            {
                if (Nokta1.Y < Nokta2.Y) geciciCizimGrafik.DrawRectangle(Kalem, Nokta1.X, Nokta1.Y, Convert.ToInt32(x_toplam), Convert.ToInt32(y_toplam));

                else geciciCizimGrafik.DrawRectangle(Kalem, Nokta1.X, Nokta2.Y, Convert.ToInt32(x_toplam), Convert.ToInt32(y_toplam));

            }
            else if (Nokta2.X < Nokta1.X)
            {
                if (Nokta1.Y > Nokta2.Y) geciciCizimGrafik.DrawRectangle(Kalem, Nokta2.X, Nokta2.Y, Convert.ToInt32(x_toplam), Convert.ToInt32(y_toplam));

                else geciciCizimGrafik.DrawRectangle(Kalem, Nokta2.X, Nokta1.Y, Convert.ToInt32(x_toplam), Convert.ToInt32(y_toplam));
            }


        }

        private void Gecici_Silgi(Point Nokta1, Color renk, int buyukluk)
        {
            if (geciciCizimYapiliyorMu == true)
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(geciciResim);
            }
            else
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(resim);
            }




            Nokta1 = new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));


            Pen SilmeyeOzelKalem = new Pen(renk, buyukluk);




            geciciCizimGrafik.FillEllipse(SilmeyeOzelKalem.Brush, Nokta1.X - buyukluk / 2, Nokta1.Y - buyukluk / 2, buyukluk, buyukluk);


        }

        private void Gecici_Cizgi_Ciz(Point Nokta1, Point Nokta2)
        {
            if (geciciCizimYapiliyorMu == true)
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(geciciResim);
            }
            else
            {
                geciciCizimGrafik.Dispose();
                geciciCizimGrafik = Graphics.FromImage(resim);
            }



            Nokta1 = new Point(Convert.ToInt32(katsayi_X * Nokta1.X), Convert.ToInt32(katsayi_Y * Nokta1.Y));
            Nokta2 = new Point(Convert.ToInt32(katsayi_X * Nokta2.X), Convert.ToInt32(katsayi_Y * Nokta2.Y));


            geciciCizimGrafik.DrawLine(Kalem, Nokta1, Nokta2);

        }

        Point Cemberin_Ortasini_Bulma(Point Nokta1, Point Nokta2)
        {
            /*
             * Çember çizimlerinde verdiğiniz nokta çemberin sol üstüne konumlandırılacak şekilde çiziliyor
             * Bunu ortaya almak adına yaptığımız bir işlem
             */
            Point Nokta3 = new Point();
            double x_toplam = Math.Abs(Nokta1.X - Nokta2.X);
            double y_toplam = Math.Abs(Nokta1.Y - Nokta2.Y);

            if (Nokta1.X > Nokta2.X)
            {
                Nokta3.X = Nokta1.X - Convert.ToInt32(x_toplam / 2);
            }
            else
            {
                Nokta3.X = Nokta1.X + Convert.ToInt32(x_toplam / 2);
            }

            if (Nokta1.Y > Nokta2.Y)
            {
                Nokta3.Y = Nokta1.Y - Convert.ToInt32(y_toplam / 2);
            }
            else
            {
                Nokta3.Y = Nokta1.Y + Convert.ToInt32(y_toplam / 2);
            }


            return Nokta3;
        }








        #region Text İşlemleri


        /*
         BURADA YAPILAN ÇİZİM ÜZERİNE YAZI EKLEME İŞLEMİ VE YAZILAN YAZIYI TAŞIMA ONDA DÜZENLEME YAPMA GİBİ İŞLEMLER YAPILIYOR
        */

        List<Label> texler = new List<Label>();

        int Secili_Label_Index = -1;

        private void Text_Ciz(Point l_Nokta1, string text, Font font)
        {
            Label l_label = Text_Tutucu_Label_Olusturucu(text, l_Nokta1, font);
            cizimYapilacakPictureBox.Controls.Add(l_label);
            texler.Add(l_label);
            //Grafik = Graphics.FromImage(Resim);

            //Grafik.DrawString(text, font, Kalem.Brush, Nokta1);
        }

        private void Texti_Yaz(Point l_Nokta1, string yazilacak)
        {
            Font font = new Font(FontFamily.GenericSansSerif, Convert.ToInt32(18 /** katsayi_X*/), FontStyle.Regular);
            this.Text_Ciz(Nokta1, yazilacak, font);
        }

        private Label Text_Tutucu_Label_Olusturucu(string text, Point location, Font fnt)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.BackColor = Color.Transparent;
            lb.Font = fnt;
            lb.AutoSize = true;
            lb.Location = location;
            lb.Name = "label1";
            lb.Size = new System.Drawing.Size(31, 13);
            lb.MouseCaptureChanged += label1_MouseCaptureChanged;
            lb.MouseDown += label1_MouseDown;
            lb.ContextMenuStrip = Create_Eleman_Resim_ContextMenuStrip();

            return lb;
        }

        private void label1_MouseCaptureChanged(object sender, EventArgs e)
        {
            //TODO: Yeniden konumlandırmasını iyi yarla

            if (Secili_Label_Index != -1 && secili_label_konum_degisimi == true)
            {
                texler[Secili_Label_Index].Location = new Point(form.DesktopLocation.X + cizimYapilacakPictureBox.PointToClient(PictureBox.MousePosition).X, form.DesktopLocation.Y + cizimYapilacakPictureBox.PointToClient(PictureBox.MousePosition).Y);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            Label Secili_Label = sender as Label;
            Secili_Label_Index = texler.IndexOf(Secili_Label);

            //for (int i = 0; i < texler.Count; i++)
            //{
            //    texler[i].BorderStyle = BorderStyle.None;
            //}

            Secili_Label.BorderStyle = BorderStyle.None;
        }


        #endregion





        #endregion





        #region Yazı Labeline Sağ Tık

        private void SeciliLabelSilme(object sender, EventArgs e)
        {
            if (Secili_Label_Index == -1) return;
            texler[Secili_Label_Index].Dispose();
        }

        bool secili_label_konum_degisimi = false;
        private void SeciliLabelKonumuDuzenle(object sender, EventArgs e)
        {
            if (Secili_Label_Index == -1) return;
            ToolStripMenuItem tsm2 = sender as ToolStripMenuItem;
            if (secili_label_konum_degisimi)
            {
                tsm2.Text = "Finish Editing The Location";
            }
            else
            {
                tsm2.Text = "Edit Location";
            }
            secili_label_konum_degisimi = !secili_label_konum_degisimi;
        }

        private void SeciliLabelFontunuDegisme(object sender, EventArgs e)
        {
            if (Secili_Label_Index == -1) return;
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                texler[Secili_Label_Index].Font = fd.Font;
            }
        }

        private void SeciliLabelRengiDegisme(object sender, EventArgs e)
        {
            if (Secili_Label_Index == -1) return;
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                texler[Secili_Label_Index].ForeColor = cd.Color;
            }
        }

        ContextMenuStrip Create_Eleman_Resim_ContextMenuStrip()
        {
            ToolStripMenuItem tsm1 = new ToolStripMenuItem();

            tsm1.Name = "denemeToolStripMenuItem";
            tsm1.Size = new System.Drawing.Size(180, 22);
            tsm1.Text = "Font Change";
            tsm1.Click += SeciliLabelFontunuDegisme;



            ToolStripMenuItem tsm2 = new ToolStripMenuItem();


            tsm2.Name = "düzenlemeToolStripMenuItem";
            tsm2.Size = new System.Drawing.Size(180, 22);
            tsm2.Text = "Edit Location";
            tsm2.Click += SeciliLabelKonumuDuzenle;

            ToolStripMenuItem tsm3 = new ToolStripMenuItem();


            tsm3.Name = "düzenlemeToolStripMenuItem";
            tsm3.Size = new System.Drawing.Size(180, 22);
            tsm3.Text = "Delete Text";
            tsm3.Click += SeciliLabelSilme;

            ToolStripMenuItem tsm4 = new ToolStripMenuItem();


            tsm4.Name = "düzenlemeToolStripMenuItem";
            tsm4.Size = new System.Drawing.Size(180, 22);
            tsm4.Text = "Color Change";
            tsm4.Click += SeciliLabelSilme;


            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsm1,
            tsm2,
            tsm3,
            tsm4});
            cms.Name = "contextMenuStrip1";
            cms.Size = new System.Drawing.Size(181, 70);

            return cms;
        }

        #endregion





        #region Mouse Eventler



        Bitmap gecicinin_gecicisi_BM;

        private void pbCalisma_MouseDown(object sender, MouseEventArgs e)
        {

            if (secili_Cizim == Cizim_Turleri.text)
            {
                geciciCizimYapiliyorMu = false;
                string donen = "";
                donen = Interaction.InputBox("Bir Harf Giriniz","","", e.Location.X, e.Location.Y);
                if (donen != "")
                {
                    Texti_Yaz(e.Location, donen);
                }
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                cizimYapilacakPictureBox.Image = this.Return_BitMap();
                Nokta1 = new Point(0, 0);
            }
            else
            {
                this.geciciCizimYapiliyorMu = true;
                if (gecicinin_gecicisi_BM != null)
                    gecicinin_gecicisi_BM.Dispose();
                gecicinin_gecicisi_BM = new Bitmap(geciciResim);
                Nokta1 = e.Location;
                geciciNokta1 = e.Location;
            }
        }


        Point geciciNokta1;
        int movesayisi = 0;

        private void pbCalisma_MouseMove(object sender, MouseEventArgs e)
        {
            Nokta1 = e.Location;
            if (geciciNokta1 != new Point(0, 0) && geciciCizimYapiliyorMu)
            {
                if (secili_Cizim == Cizim_Turleri.duz_cizgi)
                {
                    if (movesayisi % 2 == 0)
                    {
                        geciciResim.Dispose();
                        geciciResim = new Bitmap(gecicinin_gecicisi_BM);

                        this.Gecici_Duz_Cizgi_Cizi(geciciNokta1, e.Location);
                        cizimYapilacakPictureBox.Image = this.Gecici_Return_BitMap();
                    }
                    movesayisi++;
                }
                else if (secili_Cizim == Cizim_Turleri.daire)
                {
                    geciciResim.Dispose();
                    geciciResim = new Bitmap(gecicinin_gecicisi_BM);

                    this.Gecici_Daire_Ciz(geciciNokta1, e.Location);
                    cizimYapilacakPictureBox.Image = this.Gecici_Return_BitMap();
                }
                else if (secili_Cizim == Cizim_Turleri.kare)
                {
                    geciciResim.Dispose();
                    geciciResim = new Bitmap(gecicinin_gecicisi_BM);
                    this.Gecici_Kare_Ciz(geciciNokta1, e.Location);
                    cizimYapilacakPictureBox.Image = this.Gecici_Return_BitMap();
                }
                else if (secili_Cizim == Cizim_Turleri.silgi)
                {

                    this.Gecici_Silgi(e.Location,/* Color.FromArgb(240, 240, 240) */Color.White /*cizim_Yapilacak_Pb.BackColor*/, 10);
                    cizimYapilacakPictureBox.Image = this.Gecici_Return_BitMap();
                }
                else if (secili_Cizim == Cizim_Turleri.cizgi)
                {
                    this.Gecici_Cizgi_Ciz(geciciNokta1, e.Location);
                    geciciNokta1 = e.Location;
                    cizimYapilacakPictureBox.Image = this.Gecici_Return_BitMap();
                    //Gecici_Cizimi_Ana_Cizime_Aktar();
                }
            }
        }


        private void pbCalisma_MouseUp(object sender, MouseEventArgs e)
        {
            if (secili_Cizim == Cizim_Turleri.text)
            {
                form.Cursor = Cursors.Default;
                secili_Cizim = Cizim_Turleri.none;
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                cizimYapilacakPictureBox.Image = this.Return_BitMap();
                Nokta1 = new Point(0, 0);
                geciciNokta1 = new Point(0, 0);
                return;
            }
            if (gecicinin_gecicisi_BM != null)
                gecicinin_gecicisi_BM.Dispose();


            if (geciciNokta1 != new Point(0, 0))
            {



                if (secili_Cizim == Cizim_Turleri.duz_cizgi)
                {
                    this.geciciCizimYapiliyorMu = false;
                    this.Gecici_Duz_Cizgi_Cizi(geciciNokta1, e.Location);
                    cizimYapilacakPictureBox.Image = resim;
                }
                else if (secili_Cizim == Cizim_Turleri.daire)
                {
                    this.geciciCizimYapiliyorMu = false;
                    this.Gecici_Daire_Ciz(geciciNokta1, e.Location);
                    cizimYapilacakPictureBox.Image = this.Return_BitMap();
                }
                else if (secili_Cizim == Cizim_Turleri.kare)
                {
                    this.geciciCizimYapiliyorMu = false;
                    this.Gecici_Kare_Ciz(geciciNokta1, e.Location);
                    cizimYapilacakPictureBox.Image = this.Return_BitMap();
                }
                else if (secili_Cizim == Cizim_Turleri.silgi)
                {
                    Gecici_Cizimi_Ana_Cizime_Aktar();
                    cizimYapilacakPictureBox.Image = this.Return_BitMap();
                    this.geciciCizimYapiliyorMu = false;

                    BackUpIcin bi = new BackUpIcin();
                    bi.l_resim = new Bitmap(Return_BitMap());
                    new_BackUpList.Add(bi);
                }
                else if (secili_Cizim == Cizim_Turleri.cizgi)
                {
                    Gecici_Cizimi_Ana_Cizime_Aktar();
                    cizimYapilacakPictureBox.Image = this.Return_BitMap();
                    this.geciciCizimYapiliyorMu = false;

                    BackUpIcin bi = new BackUpIcin();
                    bi.l_resim = new Bitmap(Return_BitMap());
                    new_BackUpList.Add(bi);
                }
                geciciResim = new Bitmap(resim);


            }
        }



        #endregion


        #region BACKUP

        /*
         * CTRL+Z VE CTRL+Y İŞLEMLERİ İÇİN TUTULAN BACKUP
         */

        struct BackUpIcin
        {
            public Cizim_Turleri l_Tur;
            public Point l_Nokta1;
            public Point l_Nokta2;
            public Pen l_Kalem;
            public Bitmap l_resim;
        }


        List<BackUpIcin> new_BackUpList = new List<BackUpIcin>();

        int bulunulanNokta = 0;

        public void Geri_Al()
        {
            // CTRL+Z İŞLEMİ
            if (new_BackUpList.Count == 0) return;
            geciciCizimYapiliyorMu = false;
            geri_al = true;
            resim = new Bitmap(temel_Resim);

            cizimYapilacakPictureBox.Image = resim;
            new_BackUpList.RemoveAt(new_BackUpList.Count - 1);

            for (int i = 0; i < new_BackUpList.Count; i++)
            {
                BackupIcinCagirma(new_BackUpList[i]);
            }
            cizimYapilacakPictureBox.Image = resim;
            geciciResim.Dispose();
            geciciResim = new Bitmap(resim);
            geri_al = false;



            #region KATSAYI HESAPLARI

            //  RESİM SİLİNDİĞİNDE BOYUTUN YENİDEN AYARLANMASI İLE İLGİLİ BİR SIKINTI ÇIKIYORDU BUNUN DÜZELTİLMESİ İÇİN


            BoyutOranlamaKatsayilariniTekrarHesapla();

            #endregion
        }

        public void Ileri_Al()
        {

        }

        #endregion



        #region Dönecekler


        /*
         BURADAKİ İŞLEMLER GENEL OLARAK SANAL SİSTEMDE YAPILAN ÇİZİMLERİ KULLANCININ EKRANINDA BULUNAN PB YE YANSITMAYA YARIYOR
        */



        private Bitmap temel_Resim;


        private void BackupIcinCagirma(BackUpIcin gelen)
        {
            /*
             * KAYIT YAPILIRKEN 2 TÜR KAYIT VAR
             * 1. TÜRÜ BİLİNEN KAYIT ÖRNEĞİN DÜZ ÇİZGİ KARE VE YUVARLAKTA KULLANILAN 
             * 2. RESİM OLARAK KAYIT
             * BURADA 1. İF RESİM OLAN KAYITLARLA ALAKALI ALTINDAKİ KISIMLAR İSE TÜRÜ BİLİNEN KAYITLARLA ALAKALI
             */


            if (gelen.l_resim != null)
            {
                resim = new Bitmap(gelen.l_resim);

                return;
            }

            else if (gelen.l_Tur == Cizim_Turleri.duz_cizgi)
            {
                Kalem = gelen.l_Kalem;
                Gecici_Duz_Cizgi_Cizi(gelen.l_Nokta1, gelen.l_Nokta2);
            }

            else if (gelen.l_Tur == Cizim_Turleri.daire)
            {
                Kalem = gelen.l_Kalem;
                Gecici_Daire_Ciz(gelen.l_Nokta1, gelen.l_Nokta2);
            }

            else if (gelen.l_Tur == Cizim_Turleri.kare)
            {
                Kalem = gelen.l_Kalem;
                Gecici_Kare_Ciz(gelen.l_Nokta1, gelen.l_Nokta2);
            }

        }


        private Bitmap Gecici_Return_BitMap()
        {
            return geciciResim;
        }


        private void Gecici_Cizimi_Ana_Cizime_Aktar()
        {
            try
            {
                resim.Dispose();
                resim = new Bitmap(geciciResim);
            }
            catch (Exception)
            {

            }
        }


        private Bitmap Return_BitMap()
        {
            return resim;
        }

        #endregion





        #region Kayıt

        /*
         OLUŞTURULAN ÇİZİMİ KAYDETME İŞLEMLERİYLE ALAKALI BÖLÜM
        */

        private void Kayit_Oncesi_Yazilari_Resme_Gomme()
        {
            geciciCizimGrafik.Dispose();
            geciciCizimGrafik = Graphics.FromImage(resim);

            for (int i = 0; i < texler.Count; i++)
            {
                geciciCizimGrafik.DrawString(texler[i].Text, texler[i].Font, new Pen(texler[i].ForeColor).Brush, Convert.ToInt32(texler[i].Location.X * katsayi_X), Convert.ToInt32(texler[i].Location.Y * katsayi_Y));
            }
            cizimYapilacakPictureBox.Image = resim;
        }



        public Bitmap GetImage()
        {
            return resim;
        }
        public void Resmi_Kaydet(string konum)
        {
            Kayit_Oncesi_Yazilari_Resme_Gomme();

            resim.Save(konum);
        }

        #endregion







    }
}