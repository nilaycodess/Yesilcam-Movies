using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Windows.Forms;

namespace YesilcamFilmler
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection baglanti;
        private NpgsqlDataAdapter da;
        private DataSet ds;

        // PostgreSQL baðlantý bilgileri
        private string connectionString = "Host=localhost;Username=postgres;Password=1234;Database=yesilcamfilmleri";



        public Form1()
        {
            InitializeComponent();
            baglanti = new NpgsqlConnection(connectionString);

        }

        // film ekleme
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string ad = textBox1.Text;
                string rating = textBox2.Text;
                double yapim_yili = double.Parse(textBox3.Text);
                string tur = textBox4.Text;
                double butce = double.Parse(textBox5.Text);
                double gise = double.Parse(textBox6.Text);
                string oyuncular = textBox7.Text;
                string yonetmen = textBox8.Text;
                int odul_sayisi = int.Parse(textBox26.Text);

                string insertQuery = "INSERT INTO filmler(ad, rating, yapim_yili, tur, butce, gise, oyuncular, yonetmen, odul_sayisi) VALUES (@ad, @rating, @yapim_yili, @tur, @butce, @gise, @oyuncular, @yonetmen, @odul_sayisi)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@yapim_yili", yapim_yili);
                    cmd.Parameters.AddWithValue("@tur", tur);
                    cmd.Parameters.AddWithValue("@butce", butce);
                    cmd.Parameters.AddWithValue("@gise", gise);
                    cmd.Parameters.AddWithValue("@oyuncular", oyuncular);
                    cmd.Parameters.AddWithValue("@yonetmen", yonetmen);
                    cmd.Parameters.AddWithValue("@odul_sayisi", odul_sayisi);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Bilgiler baþarýyla eklendi.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }


        }

        // film listeleme
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string selectQuery = "SELECT * FROM filmler";

                da = new NpgsqlDataAdapter(selectQuery, baglanti);
                ds = new DataSet();

                da.Fill(ds, "filmler");
                dataGridView2.DataSource = ds.Tables["filmler"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void SavePhotoToDatabase(byte[] photoBytes)
        {
            string connectionString = "server=localHost; port=5432; Database=yesilcamfilmleri; user ID=postgres; password=1234";
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO afis (image_column) VALUES (@photo)", conn))
                {
                    // Parametre olarak byte dizisini ekleyin
                    cmd.Parameters.Add(new NpgsqlParameter("photo", NpgsqlDbType.Bytea));
                    cmd.Parameters["photo"].Value = photoBytes;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }

        // film bilgilerini label da deðiþtirme
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string filmAdiToDisplay = textBox9.Text;


                string selectFilmQuery = "SELECT ad, rating, yapim_yili, tur, butce, gise, oyuncular, yonetmen FROM film WHERE ad = @ad";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectFilmQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", filmAdiToDisplay);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label1.Text = "" + reader["ad"].ToString();
                            label15.Text = "" + reader["rating"].ToString();
                            label4.Text = "Yapým Yýlý: " + reader["yapim_yili"].ToString();
                            label5.Text = "Tür: " + reader["tur"].ToString();
                            label3.Text = "Bütçe: " + reader["butce"].ToString();
                            label14.Text = "Giþe: " + reader["gise"].ToString();
                            label21.Text = "" + reader["oyuncular"].ToString();
                            label6.Text = "Yönetmen:" + reader["yonetmen"].ToString();


                        }
                        else
                        {
                            MessageBox.Show("Belirtilen film adýnda bir film bulunamadý.");
                            label1.Text = "Film Adý: ";
                            label15.Text = "Rating: ";
                            label4.Text = "Yapým Yýlý: ";
                            label5.Text = "Tür: ";
                            label3.Text = "Bütçe: ";
                            label14.Text = "Giþe: ";
                            label21.Text = "Oyuncular: ";
                            label6.Text = "Yönetmen: ";


                        }
                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }


        }

        // oyuncular Listeleme
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string selectQuery = "SELECT * FROM oyuncular";

                da = new NpgsqlDataAdapter(selectQuery, baglanti);
                ds = new DataSet();

                da.Fill(ds, "oyuncular");
                dataGridView3.DataSource = ds.Tables["oyuncular"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // film güncelleme
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string filmAdToUpdate = textBox28.Text;

                string updateQuery = "UPDATE filmler SET ad = @ad, rating = @rating, yapim_yili = @yapim_yili, tur = @tur, butce = @butce, gise = @gise, oyuncular = @oyuncular, yonetmen = @yonetmen, odul_sayisi = @odul_sayisi WHERE ad = @filmAd";

                using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", textBox1.Text);
                    cmd.Parameters.AddWithValue("@rating", textBox2.Text);
                    cmd.Parameters.AddWithValue("@yapim_yili", double.Parse(textBox3.Text));
                    cmd.Parameters.AddWithValue("@tur", textBox4.Text);
                    cmd.Parameters.AddWithValue("@butce", double.Parse(textBox5.Text));
                    cmd.Parameters.AddWithValue("@gise", double.Parse(textBox6.Text));
                    cmd.Parameters.AddWithValue("@oyuncular", textBox7.Text);
                    cmd.Parameters.AddWithValue("@yonetmen", textBox8.Text);
                    cmd.Parameters.AddWithValue("@odul_sayisi", int.Parse(textBox26.Text));
                    cmd.Parameters.AddWithValue("@filmAd", filmAdToUpdate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Film bilgileri baþarýyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen ID'ye sahip bir film bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        // oyuncu ekleme
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string sahneadi = textBox11.Text;
                string gercekadi = textBox12.Text;
                string dogum_tarihi = textBox13.Text;
                string cinsiyet = textBox14.Text;
                int odul_sayisi = int.Parse(textBox15.Text);
                string insertQuery = "INSERT INTO oyuncular(sahneadi, gercekadi, dogum_tarihi, cinsiyet, odul_sayisi) VALUES (@sahneadi, @gercekadi, @dogum_tarihi, @cinsiyet, @odul_sayisi)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@sahneadi", sahneadi);
                    cmd.Parameters.AddWithValue("@gercekadi", gercekadi);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", dogum_tarihi);
                    cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    cmd.Parameters.AddWithValue("@odul_sayisi", odul_sayisi);


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Oyuncu baþarýyla eklendi.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // film silme
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string filmAdiToDelete = textBox28.Text;

                string deleteFilmQuery = "DELETE FROM filmler WHERE ad = @ad";

                using (NpgsqlCommand cmd = new NpgsqlCommand(deleteFilmQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", filmAdiToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Film baþarýyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen isimde bir film bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // oyuncu silme
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string oyuncuAdiToDelete = textBox16.Text;

                string deleteOyuncuQuery = "DELETE FROM oyuncular WHERE sahneadi = @sahneadi";

                using (NpgsqlCommand cmd = new NpgsqlCommand(deleteOyuncuQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@sahneadi", oyuncuAdiToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Oyuncu baþarýyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen isimde bir oyuncu bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        // oyuncu güncelleme
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string oyuncularSahneadiToUpdate = textBox16.Text;

                string updateQuery = "UPDATE oyuncular SET sahneadi = @sahneadi, gercekadi = @gercekadi, dogum_tarihi = @dogum_tarihi, cinsiyet = @cinsiyet, odul_sayisi = @odul_sayisi WHERE sahneadi = @oyuncularSahneadi";

                using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@sahneadi", textBox11.Text);
                    cmd.Parameters.AddWithValue("@gercekadi", textBox12.Text);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", double.Parse(textBox13.Text));
                    cmd.Parameters.AddWithValue("@cinsiyet", textBox14.Text);

                    cmd.Parameters.AddWithValue("@odul_sayisi", int.Parse(textBox15.Text));
                    cmd.Parameters.AddWithValue("@oyuncularSahneadi", oyuncularSahneadiToUpdate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("oyuncu bilgileri baþarýyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen ad'a sahip bir oyuncu bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string oyuncuAdiToDisplay = textBox10.Text;


                string selectOyuncuQuery = "SELECT sahneadi, gercekadi, dogum_tarihi, cinsiyet, odul_sayisi FROM oyuncular WHERE gercekadi = @gercekadi";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectOyuncuQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@gercekadi", oyuncuAdiToDisplay);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label40.Text = "" + reader["sahneadi"].ToString();
                            label46.Text = "" + reader["gercekadi"].ToString();
                            label29.Text = "Doðum Tarihi: " + reader["dogum_tarihi"].ToString();
                            label30.Text = "Cinsiyet: " + reader["cinsiyet"].ToString();
                            label31.Text = "Ödül Sayýsý: " + reader["odul_sayisi"].ToString();


                        }
                        else
                        {
                            MessageBox.Show("Belirtilen oyuncu adýnda bir oyuncu bulunamadý.");
                            label40.Text = "Sahne Adý: ";
                            label46.Text = "Gerçek Adý: ";
                            label29.Text = "Doðum Tarihi: ";
                            label30.Text = "Cinsiyet: ";
                            label31.Text = "Ödül Sayýsý: ";

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }


        private void button11_Click(object sender, EventArgs e)
        {

        }

        // yönetmen listele

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string selectQuery = "SELECT * FROM yonetmen";

                da = new NpgsqlDataAdapter(selectQuery, baglanti);
                ds = new DataSet();

                da.Fill(ds, "yonetmen");
                dataGridView4.DataSource = ds.Tables["yonetmen"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // yönetmen ekle
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string ad = textBox17.Text;
                string dogum_tarihi = textBox18.Text;
                string cinsiyet = textBox19.Text;
                int odul_sayisi = int.Parse(textBox20.Text);
                string insertQuery = "INSERT INTO yonetmen(ad, dogum_tarihi, cinsiyet, odul_sayisi) VALUES (@ad, @dogum_tarihi, @cinsiyet, @odul_sayisi)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, baglanti))
                {

                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", dogum_tarihi);
                    cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    cmd.Parameters.AddWithValue("@odul_sayisi", odul_sayisi);


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Veri baþarýyla eklendi.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // yönetmen güncelle
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // textBox10'un içeriðini kontrol et
                if (!string.IsNullOrEmpty(textBox23.Text))
                {
                    int yonetmenAdToUpdate = int.Parse(textBox23.Text);

                    string updateQuery = "UPDATE yonetmen SET ad = @ad, dogum_tarihi = @dogum_tarihi, cinsiyet = @cinsiyet, odul_sayisi= @odul_sayisi WHERE ad = @yonetmenAd";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, baglanti))
                    {
                        cmd.Parameters.AddWithValue("@ad", textBox17.Text);
                        cmd.Parameters.AddWithValue("@dogum_tarihi", textBox18.Text);
                        cmd.Parameters.AddWithValue("@cinsiyet", textBox19.Text);
                        cmd.Parameters.AddWithValue("@odul_sayisi", int.Parse(textBox20.Text));
                        cmd.Parameters.AddWithValue("@yonetmenAd", yonetmenAdToUpdate);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Oyuncu bilgileri baþarýyla güncellendi.");
                        }
                        else
                        {
                            MessageBox.Show("Belirtilen Ad'a sahip bir oyuncu bulunamadý.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Oyuncu Adý boþ olamaz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }


        // yönetmen sil
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string yonetmenAdiToDelete = textBox23.Text;

                string deleteYonetmenQuery = "DELETE FROM yonetmen WHERE ad = @ad";

                using (NpgsqlCommand cmd = new NpgsqlCommand(deleteYonetmenQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", yonetmenAdiToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Oyuncu baþarýyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen isimde bir oyuncu bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string oyuncuAdi = textBox24.Text.Trim();

            if (!string.IsNullOrEmpty(oyuncuAdi))
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Film tablosundan oyuncunun adýný içeren tüm kayýtlarý getir
                        string query = "SELECT ad, rating, yapim_yili, tur, butce, gise, oyuncular, yonetmen " +
                                       "FROM film " +
                                       "WHERE oyuncular LIKE @oyuncuAdi";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@oyuncuAdi", "%" + oyuncuAdi + "%");

                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // DataGridView'e verileri yükle
                                dataGridView1.DataSource = dataTable;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen oyuncu adýný giriniz.");
            }
        }

        // yönetmenin film ve odul sayýsý
        private void button17_Click(object sender, EventArgs e)
        {
            string yonetmenAdi = textBox25.Text.Trim();

            if (!string.IsNullOrEmpty(yonetmenAdi))
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Film tablosundan yonetmenin filmlerini ve aldýðý ödüllerin sayýsýný getir
                        string query = "SELECT ad, odul_sayisi " +
                                       "FROM filmler " +
                                       "WHERE yonetmen = @yonetmenAdi";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@yonetmenAdi", yonetmenAdi);

                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // DataGridView'e verileri yükle
                                dataGridView5.DataSource = dataTable;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen yönetmen adýný giriniz.");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {

        }


        // yöetmenlerin birl oyunccular + film sayýsý
        private void button24_Click(object sender, EventArgs e)
        {

        }



        // birl oynadýklarý film ve sayýsý 4. madde X
        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string oyuncuAdi = textBox22.Text;

                // Belirli bir oyuncunun birlikte oynadýðý filmleri ve film sayýsýný getir
                string selectFilmsQuery = "SELECT DISTINCT f.ad AS film_adi, f.rating, f.yapim_yili, f.tur, f.butce, f.gise, f.yonetmen, f.oyuncular " +
                                          "FROM filmler f " +
                                          "WHERE @oyuncuAdi = ANY(string_to_array(oyuncular, ','))";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectFilmsQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@oyuncuAdi", oyuncuAdi);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Oyuncunun birlikte oynadýðý filmlerin toplam sayýsýný hesapla
                        int filmSayisi = GetOyuncuBirlikteOynadigiFilmSayisi(oyuncuAdi);

                        // DataTable'a "film_sayisi" sütununu ekle ve deðerleri doldur
                        dataTable.Columns.Add("film_sayisi", typeof(int));
                        foreach (DataRow row in dataTable.Rows)
                        {
                            row["film_sayisi"] = filmSayisi;
                        }

                        // DataGridView6'ya verileri yükle
                        dataGridView7.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private int GetOyuncuBirlikteOynadigiFilmSayisi(string oyuncuAdi)
        {
            try
            {
                string filmSayisiQuery = "SELECT COUNT(*) " +
                                         "FROM filmler " +
                                         "WHERE @oyuncuAdi = ANY(string_to_array(oyuncular, ','))";

                using (NpgsqlCommand filmSayisiCommand = new NpgsqlCommand(filmSayisiQuery, baglanti))
                {
                    filmSayisiCommand.Parameters.AddWithValue("@oyuncuAdi", oyuncuAdi);

                    int filmSayisi = Convert.ToInt32(filmSayisiCommand.ExecuteScalar());
                    return filmSayisi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oyuncunun birlikte oynadýðý film sayýsý alýnýrken hata oluþtu: " + ex.Message);
                return 0;
            }
        }




        // 5. madde
        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string turAdi = textBox27.Text;

                // Belirli bir türdeki en yüksek ratinge sahip 10 filmi getir
                string selectTopFilmsQuery = "SELECT ad AS ad, yonetmen, oyuncular, rating " +
                                              "FROM filmler " +
                                              "WHERE tur = @turAdi " +
                                              "ORDER BY rating DESC " +
                                              "LIMIT 10";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectTopFilmsQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@turAdi", turAdi);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridViev yazdýr
                        dataGridView8.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

        }

        private void button22_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

        }

        private void button21_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

        }

        private void button20_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

        }

        private void button19_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;

        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;

        }

        // oyuncu listeleme
        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string selectQuery = "SELECT * FROM oyuncular";

                da = new NpgsqlDataAdapter(selectQuery, baglanti);
                ds = new DataSet();

                da.Fill(ds, "oyuncular");
                dataGridView3.DataSource = ds.Tables["oyuncular"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        // yönetmen listele
        private void button12_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string selectQuery = "SELECT * FROM yonetmen";

                da = new NpgsqlDataAdapter(selectQuery, baglanti);
                ds = new DataSet();

                da.Fill(ds, "yonetmen");
                dataGridView4.DataSource = ds.Tables["yonetmen"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        // yönetmen ekle
        private void button13_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string ad = textBox17.Text;
                string dogum_tarihi = textBox18.Text;
                string cinsiyet = textBox19.Text;
                int odul_sayisi = int.Parse(textBox20.Text);
                string insertQuery = "INSERT INTO yonetmen(ad, dogum_tarihi, cinsiyet, odul_sayisi) VALUES (@ad, @dogum_tarihi, @cinsiyet, @odul_sayisi)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, baglanti))
                {

                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", dogum_tarihi);
                    cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    cmd.Parameters.AddWithValue("@odul_sayisi", odul_sayisi);


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Yönetmen baþarýyla eklendi.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        // yönetmen silme
        private void button14_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string oyuncuAdiToDelete = textBox23.Text;

                string deleteOyuncuQuery = "DELETE FROM yonetmen WHERE ad = @ad";

                using (NpgsqlCommand cmd = new NpgsqlCommand(deleteOyuncuQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", oyuncuAdiToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Yönetmen baþarýyla silindi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen isimde bir yönetmen bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string yonetmenAdiToDisplay = textBox23.Text;


                string selectYonetmenQuery = "SELECT ad, dogum_tarihi, cinsiyet, odul_sayisi FROM yonetmen WHERE ad = @ad";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectYonetmenQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", yonetmenAdiToDisplay);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            label38.Text = "" + reader["ad"].ToString();
                            label42.Text = "Doðum Tarihi: " + reader["dogum_tarihi"].ToString();
                            label43.Text = "Cinsiyet: " + reader["cinsiyet"].ToString();
                            label44.Text = "Ödül Sayýsý: " + reader["odul_sayisi"].ToString();


                        }
                        else
                        {
                            MessageBox.Show("Belirtilen oyuncu adýnda bir oyuncu bulunamadý.");

                            label38.Text = "Gerçek Adý: ";
                            label42.Text = "Doðum Tarihi: ";
                            label43.Text = "Cinsiyet: ";
                            label44.Text = "Ödül Sayýsý: ";

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // bir oyuncunun oynadýðý filmlerin listesi
        private void button16_Click_1(object sender, EventArgs e)
        {
            string oyuncuAdi = textBox24.Text.Trim();

            if (!string.IsNullOrEmpty(oyuncuAdi))
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Film tablosundan oyuncunun adýný içeren tüm kayýtlarý getir
                        string query = "SELECT ad, rating, yapim_yili, tur, butce, gise, oyuncular, yonetmen " +
                                       "FROM film " +
                                       "WHERE oyuncular LIKE @oyuncuAdi";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@oyuncuAdi", "%" + oyuncuAdi + "%");

                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // DataGridView'e verileri yükle
                                dataGridView1.DataSource = dataTable;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen oyuncu adýný giriniz.");
            }

        }

        // 3. madde: yönetmenlerin birl çalýþtýðý oyuncular + film sayýsý
        private void button17_Click_1(object sender, EventArgs e)
        {
            string yonetmenAdi = textBox25.Text.Trim();

            if (!string.IsNullOrEmpty(yonetmenAdi))
            {
                try
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        // Yönetmenin film tablosundan oyuncularýný getir
                        string query = "SELECT unnest(string_to_array(oyuncular, ',')) AS oyuncu " +
                                       "FROM filmler " +
                                       "WHERE yonetmen = @yonetmenAdi";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@yonetmenAdi", yonetmenAdi);

                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // Oyuncularýn film sayýsýný hesapla
                                List<OyuncuFilmSayisi> oyuncuFilmSayilari = new List<OyuncuFilmSayisi>();
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string oyuncuAdi = row["oyuncu"].ToString();
                                    int filmSayisi = GetFilmSayisi(oyuncuAdi, yonetmenAdi);
                                    oyuncuFilmSayilari.Add(new OyuncuFilmSayisi { OyuncuAdi = oyuncuAdi, FilmSayisi = filmSayisi });
                                }

                                // DataGridView'e verileri yükle
                                dataGridView5.DataSource = oyuncuFilmSayilari;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen yönetmen adýný giriniz.");
            }
        }

        private int GetFilmSayisi(string oyuncuAdi, string yonetmenAdi)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Belirli bir oyuncunun ve yönetmenin film sayýsýný getir
                    string filmSayisiQuery = "SELECT COUNT(*) " +
                                             "FROM filmler " +
                                             "WHERE @oyuncuAdi = ANY(string_to_array(oyuncular, ',')) " +
                                             "AND yonetmen = @yonetmenAdi";

                    using (NpgsqlCommand filmSayisiCommand = new NpgsqlCommand(filmSayisiQuery, connection))
                    {
                        filmSayisiCommand.Parameters.AddWithValue("@oyuncuAdi", oyuncuAdi);
                        filmSayisiCommand.Parameters.AddWithValue("@yonetmenAdi", yonetmenAdi);

                        int filmSayisi = Convert.ToInt32(filmSayisiCommand.ExecuteScalar());
                        return filmSayisi;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Film sayýsý alýnýrken hata oluþtu: " + ex.Message);
                return 0;
            }
        }

        private class OyuncuFilmSayisi
        {
            public string OyuncuAdi { get; set; }
            public int FilmSayisi { get; set; }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string yonetmenAdi = textBox21.Text.Trim();

                // Yönetmenin çektiði filmlerin adý ve yönetmen tablosundaki ödül sayýsý
                string selectFilmsQuery = "SELECT filmler.ad, yonetmen.odul_sayisi " +
                                          "FROM filmler " +
                                          "INNER JOIN yonetmen ON filmler.yonetmen = yonetmen.ad " +
                                          "WHERE filmler.yonetmen = @yonetmenAdi";

                using (NpgsqlCommand cmd = new NpgsqlCommand(selectFilmsQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@yonetmenAdi", yonetmenAdi);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // DataGridView5'e verileri yükle
                        dataGridView6.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void button28_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                string yonetmenAdToUpdate = textBox29.Text;

                string updateQuery = "UPDATE yonetmen SET ad = @ad, dogum_tarihi = @dogum_tarihi, cinsiyet = @cinsiyet, odul_sayisi = @odul_sayisi WHERE ad = @yonetmenAd";

                using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", textBox17.Text);

                    cmd.Parameters.AddWithValue("@dogum_tarihi", double.Parse(textBox18.Text));
                    cmd.Parameters.AddWithValue("@cinsiyet", textBox19.Text);

                    cmd.Parameters.AddWithValue("@odul_sayisi", int.Parse(textBox20.Text));
                    cmd.Parameters.AddWithValue("@yonetmenAd", yonetmenAdToUpdate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("yönetmen bilgileri baþarýyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen ad'a sahip bir yönetmen bulunamadý.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}











