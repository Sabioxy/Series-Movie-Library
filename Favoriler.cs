using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnaSayfa
{
    public partial class Favoriler : Form
    {
        private int currentMovieId;
        private int currentSeriesId;

        private SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mamie\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True");

        public Favoriler()
        {
            InitializeComponent();
        }
        string email;
        public Favoriler(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void CheckFavoriteStatus()
        {
            // Favori butonunun metnini belirle
            if (IsMovieInFavorites(currentMovieId))
            {
                // Eğer film favorilerde ise "Favorilerden Kaldır" yaz
                label6.Text = "Favorilerden Kaldır";
            }
            else
            {
                // Eğer film favorilerde değilse "Favorilere Ekle" yaz
                label6.Text = "Favorilere Ekle";
            }
        }

        private void CheckFavoriteStatus2()
        {
            // Favori butonunun metnini belirle
            if (IsSeriesInFavorites(currentSeriesId))
            {
                // Eğer dizi favorilerde ise "Favorilerden Kaldır" yaz
                label6.Text = "Favorilerden Kaldır";
            }
            else
            {
                // Eğer dizi favorilerde değilse "Favorilere Ekle" yaz
                label6.Text = "Favorilere Ekle";
            }
        }

        private bool IsSeriesInFavorites(int seriesId)
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // SQL sorgusunu oluştur
                string selectFavoritesQuery = "SELECT favorites_series FROM admin WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(selectFavoritesQuery, connect))
                {
                    // Parametreyi ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Sorguyu çalıştırın
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string favorites = reader["favorites_series"].ToString();
                        // Favorilerde olan dizileri kontrol et
                        if (favorites.Contains(seriesId.ToString()))
                        {
                            // Dizi favorilerde ise true döndür
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            // Dizi favorilerde değilse false döndür
            return false;
        }
        private bool IsMovieInFavorites(int movieId)
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // SQL sorgusunu oluştur
                string selectFavoritesQuery = "SELECT favorites_movie FROM admin WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(selectFavoritesQuery, connect))
                {
                    // Parametreyi ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Sorguyu çalıştırın
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string favorites = reader["favorites_movie"].ToString();
                        // Favorilerde olan filmleri kontrol et
                        if (favorites.Contains(movieId.ToString()))
                        {
                            // Film favorilerde ise true döndür
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            // Film favorilerde değilse false döndür
            return false;
        }
        private async Task RemoveFromFavorites2()
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol edin
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // Favori dizi ID'sini kaldırmak için SQL sorgusunu oluşturun
                string removeFromFavoritesQuery = "UPDATE admin SET favorites_series = REPLACE(REPLACE(favorites_series, @FavoriteSeriesID + ',', ''), @FavoriteSeriesID, '') WHERE email = @Email;";

                using (SqlCommand cmd = new SqlCommand(removeFromFavoritesQuery, connect))
                {
                    // Parametreleri ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FavoriteSeriesID", currentSeriesId.ToString());

                    // Sorguyu çalıştırın ve asenkron olarak bekleyin
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Dizi favorilerden kaldırılırken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        private async Task RemoveFromFavorites()
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol edin
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // Favori film ID'sini kaldırmak için SQL sorgusunu oluşturun
                string removeFromFavoritesQuery = "UPDATE admin SET favorites_movie = REPLACE(REPLACE(favorites_movie, @FavoriteMovieID + ',', ''), @FavoriteMovieID, '') WHERE email = @Email;";

                using (SqlCommand cmd = new SqlCommand(removeFromFavoritesQuery, connect))
                {
                    // Parametreleri ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FavoriteMovieID", currentMovieId.ToString());

                    // Sorguyu çalıştırın ve asenkron olarak bekleyin
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Film favorilerden kaldırılırken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
        private async Task AddToFavorites2()
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // Favori dizi ID'sini eklemek için SQL sorgusunu oluşturun
                string updateFavoritesQuery = "UPDATE admin SET favorites_series = COALESCE(NULLIF(favorites_series, '') + ',', '') + @FavoriteSeriesID WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(updateFavoritesQuery, connect))
                {
                    // Parametreleri ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FavoriteSeriesID", currentSeriesId.ToString());

                    // Sorguyu çalıştırın ve asenkron olarak bekleyin
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Dizi favorilere eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
        private async Task AddToFavorites()
        {
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // Favori film ID'sini eklemek için SQL sorgusunu oluşturun
                string updateFavoritesQuery = "UPDATE admin SET favorites_movie = COALESCE(NULLIF(favorites_movie, '') + ',', '') + @FavoriteMovieID WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(updateFavoritesQuery, connect))
                {
                    // Parametreleri ekleyin
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FavoriteMovieID", currentMovieId.ToString());

                    // Sorguyu çalıştırın ve asenkron olarak bekleyin
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Film favorilere eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }


        private async void getSeriesbyID()
        {
            flowLayoutPanel3.Controls.Clear();
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }
                // SQL sorgusunu oluştur
                string selectFavoritesQuery = $"SELECT favorites_series FROM admin WHERE email = '{email}'";

                using (SqlCommand cmd = new SqlCommand(selectFavoritesQuery, connect))
                {

                    // Sorguyu çalıştırın
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string favorites = reader["favorites_series"].ToString();
                        string[] favoritesArray = favorites.Split(',');
                        foreach (string favoriteId in favoritesArray)
                        {
                            if (string.IsNullOrEmpty(favoriteId)) continue;

                            var options = new RestClientOptions($"https://api.themoviedb.org/3/tv/{favoriteId}?language=tr-TR");
                            var client = new RestClient(options);
                            var request = new RestRequest("");
                            request.AddHeader("accept", "application/json");
                            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
                            var response = await client.GetAsync(request);

                            dynamic jsonData = JObject.Parse(response.Content);
                            dynamic data = jsonData;

                            // Panel ve içeriği oluşturulmadan önce aynı dizi için panelin zaten oluşturulup oluşturulmadığını kontrol ediyoruz
                            Panel existingPanel = flowLayoutPanel3.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag.ToString() == favoriteId);
                            if (existingPanel != null)
                            {
                                // Eğer panel zaten varsa, içeriği güncelliyoruz ve döngüyü bir sonraki favori diziyi için devam ettiriyoruz
                                UpdatePanelContentSeries(existingPanel, data);
                                continue;
                            }

                            // Eğer panel yoksa, yeni panel oluşturup içeriği ekliyoruz
                            Panel panel = CreatePanelSeries(favoriteId, data);
                            flowLayoutPanel3.Controls.Add(panel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        // Panel oluşturma ve içeriğini ayarlama işlemlerini ayrı metotlar olarak tanımlıyoruz

        private Panel CreatePanelSeries(string favoriteId, dynamic data)
        {
            Panel panel = new Panel() { Width = 250, Height = 350, Tag = favoriteId };
            PictureBox pictureBox = new PictureBox() { Width = 188, Height = 282, Tag = favoriteId, ImageLocation = "https://image.tmdb.org/t/p/w188_and_h282_bestv2" + data.poster_path, Dock = DockStyle.Top };

            // PictureBox'a tıklama olayını ekle
            pictureBox.Click += (s, ev) =>
            {
                // PictureBox'a tıklandığında, tıklanan resmin indisini al
                int index = (int)((PictureBox)s).Tag;

                // PictureBox'a tıklandığında, ilgili dizinin ID'sini al
                int seriesId = int.Parse(data.id.ToString());

                // Ve bu ID ile ilgili dizinin detaylarını göster
                ShowSeriesDetails(seriesId);
            };

            Label label = new Label() { ForeColor = Color.White, Font = new Font("Microsoft YaHei UI", 11), Width = 188, Height = 50, Tag = favoriteId, Text = data.name, Dock = DockStyle.Bottom };
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(label);
            return panel;
        }

        private void UpdatePanelContentSeries(Panel panel, dynamic data)
        {
            PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();
            if (pictureBox != null)
            {
                pictureBox.ImageLocation = "https://image.tmdb.org/t/p/w188_and_h282_bestv2" + data.poster_path;
            }

            Label label = panel.Controls.OfType<Label>().FirstOrDefault();
            if (label != null)
            {
                label.Text = data.name;
            }
        }


        private async void ShowSeriesDetails(int seriesId)
        {
            // Detay panelinde gösterilecek film ID'sini güncelleyin
            currentSeriesId = seriesId;

            panel1.Visible = true;
            // Detayları almak için API isteği oluştur
            var client = new RestClient($"https://api.themoviedb.org/3/tv/{seriesId}?language=tr-TR");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // API'den gelen veriyi işle
                dynamic jsonData = JObject.Parse(response.Content);

                // Dizinin başlığını ve açıklamasını alalım
                string name = jsonData.name;
                string overview = jsonData.overview;
                string posterPath = "https://image.tmdb.org/t/p/original" + jsonData.poster_path;

                pictureBox1.ImageLocation = posterPath;

                label1.Text = name;
                richTextBox1.Text = overview;

                // Favori durumunu kontrol et
                CheckFavoriteStatus2();
                // Önerilen filmleri göster
                ShowSimilarSeries(seriesId);

                label7.Text = "Dizi";
            }
            else
            {
                MessageBox.Show("Dizi detayları alınamadı. Lütfen daha sonra tekrar deneyin.", "Hata");
            }
        }

        private async void ShowSimilarSeries(int seriesId)
        {
            flowLayoutPanel2.Controls.Clear();

            // Benzer dizileri almak için API isteği oluştur
            var client = new RestClient($"https://api.themoviedb.org/3/tv/{seriesId}/similar?language=tr-TR&page=1");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // API'den gelen veriyi işle
                dynamic jsonData = JObject.Parse(response.Content);
                dynamic similarSeries = jsonData.results;

                // Önerilen dizileri panel içerisine ekleyin
                for (int i = 0; i < 5; i++)
                {
                    // Panel oluştur
                    Panel pn = new Panel()
                    {
                        Width = 120,
                        Height = 167,
                    };

                    // PictureBox oluştur
                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = 94,
                        Height = 141,
                        Tag = similarSeries[i].id,
                        ImageLocation = "https://image.tmdb.org/t/p/w94_and_h141_bestv2" + similarSeries[i].poster_path,
                        Dock = DockStyle.Top
                    };

                    // Picturebox'a click eventi atandı
                    pictureBox.Click += (object sender, EventArgs e) =>
                    {
                        PictureBox pb = sender as PictureBox;
                        int recommendedSeriesId = Convert.ToInt32(pb.Tag);
                        ShowSeriesDetails(recommendedSeriesId);
                    };

                    //Label oluştur
                    Label lbl = new Label()
                    {
                        Text = similarSeries[i].name,
                        Dock = DockStyle.Bottom,
                        ForeColor = Color.White,
                        Width = 120,
                        Height = 25,
                    };

                    pn.Controls.Add(pictureBox);
                    pn.Controls.Add(lbl);

                    flowLayoutPanel2.Controls.Add(pn);
                }
            }
            else
            {
                MessageBox.Show("Benzer diziler alınamadı. Lütfen daha sonra tekrar deneyin.", "Hata");
            }
        }

        private void RefreshFav()
        {
            flowLayoutPanel3.Controls.Clear();
            flowLayoutPanel1.Controls.Clear();        
                getMoviesbyID();
                getSeriesbyID();
        }

        private async void getMoviesbyID()
        {
            flowLayoutPanel3.Controls.Clear();
            try
            {
                // Bağlantının açık olup olmadığını kontrol ediyoruz
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }
                // SQL sorgusunu oluştur
                string selectFavoritesQuery = $"SELECT favorites_movie FROM admin WHERE email = '{email}'";

                using (SqlCommand cmd = new SqlCommand(selectFavoritesQuery, connect))
                {

                    // Sorguyu çalıştırın
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string favorites = reader["favorites_movie"].ToString();
                        string[] favoritesArray = favorites.Split(',');
                        foreach (string favoriteId in favoritesArray)
                        {
                            if (string.IsNullOrEmpty(favoriteId)) continue;

                            var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{favoriteId}?language=tr-TR");
                            var client = new RestClient(options);
                            var request = new RestRequest("");
                            request.AddHeader("accept", "application/json");
                            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
                            var response = await client.GetAsync(request);

                            dynamic jsonData = JObject.Parse(response.Content);
                            dynamic data = jsonData;

                            // Panel ve içeriği oluşturulmadan önce aynı film için panelin zaten oluşturulup oluşturulmadığını kontrol ediyoruz
                            Panel existingPanel = flowLayoutPanel1.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag.ToString() == favoriteId);
                            if (existingPanel != null)
                            {
                                // Eğer panel zaten varsa, içeriği güncelliyoruz ve döngüyü bir sonraki favori film için devam ettiriyoruz
                                UpdatePanelContentMovie(existingPanel, data);
                                continue;
                            }

                            // Eğer panel yoksa, yeni panel oluşturup içeriği ekliyoruz
                            Panel panel = CreatePanelMovie(favoriteId, data);
                            flowLayoutPanel1.Controls.Add(panel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı kapatın
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        // Panel oluşturma ve içeriğini ayarlama işlemlerini ayrı metotlar olarak tanımlıyoruz

        private Panel CreatePanelMovie(string favoriteId, dynamic data)
        {
            Panel panel = new Panel() { Width = 250, Height = 350, Tag = favoriteId };
            PictureBox pictureBox = new PictureBox() { Width = 188, Height = 282, Tag = favoriteId, ImageLocation = "https://image.tmdb.org/t/p/w188_and_h282_bestv2" + data.poster_path, Dock = DockStyle.Top };

            // PictureBox'a tıklama olayını ekle
            pictureBox.Click += (s, ev) =>
            {
                // PictureBox'a tıklandığında, tıklanan resmin indisini al
                int index = (int)((PictureBox)s).Tag;

                // PictureBox'a tıklandığında, ilgili dizinin ID'sini al
                int movieId = int.Parse(data.id.ToString());

                // Ve bu ID ile ilgili dizinin detaylarını göster
                ShowMovieDetails(movieId);
            };

            Label label = new Label() { ForeColor = Color.White, Font = new Font("Microsoft YaHei UI", 11), Width = 188, Height = 50, Tag = favoriteId, Text = data.title, Dock = DockStyle.Bottom };
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(label);
            return panel;
        }

        private void UpdatePanelContentMovie(Panel panel, dynamic data)
        {
            PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();
            if (pictureBox != null)
            {
                pictureBox.ImageLocation = "https://image.tmdb.org/t/p/w188_and_h282_bestv2" + data.poster_path;
            }

            Label label = panel.Controls.OfType<Label>().FirstOrDefault();
            if (label != null)
            {
                label.Text = data.title;
            }
        }


        private async void ShowMovieDetails(int movieId)
        {
            // Detay panelinde gösterilecek film ID'sini güncelleyin
            currentMovieId = movieId;

            panel1.Visible = true;
            // Detayları almak için API isteği oluştur
            var client = new RestClient($"https://api.themoviedb.org/3/movie/{movieId}?language=tr-TR");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // API'den gelen veriyi işle
                dynamic jsonData = JObject.Parse(response.Content);

                // Örneğin, filmin başlığını ve açıklamasını alalım
                string title = jsonData.title;
                string overview = jsonData.overview;
                string posterPath = "https://image.tmdb.org/t/p/original" + jsonData.poster_path;

                pictureBox1.ImageLocation = posterPath;

                label1.Text = title;
                richTextBox1.Text = overview;

                // Favori durumunu kontrol et
                CheckFavoriteStatus();
                // Önerilen filmleri göster
                ShowSimilarMovies(movieId);

                label7.Text = "Film";

            }
            else
            {
                MessageBox.Show("Film detayları alınamadı. Lütfen daha sonra tekrar deneyin.", "Hata");
            }
        }

        private async void ShowSimilarMovies(int movieId)
        {
            flowLayoutPanel2.Controls.Clear();

            // Benzer filmleri almak için API isteği oluştur
            var client = new RestClient($"https://api.themoviedb.org/3/movie/{movieId}/similar?language=tr-TR&page=1");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)  ");
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // API'den gelen veriyi işle
                dynamic jsonData = JObject.Parse(response.Content);
                dynamic similarMovies = jsonData.results;

                // Önerilen filmleri panel içerisine ekleyin
                for (int i = 0; i < 5; i++)
                {
                    // Panel oluştur
                    Panel pn = new Panel()
                    {
                        Width = 120,
                        Height = 167,
                    };

                    // PictureBox oluştur
                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = 94,
                        Height = 141,
                        Tag = similarMovies[i].id,
                        ImageLocation = "https://image.tmdb.org/t/p/w94_and_h141_bestv2" + similarMovies[i].poster_path,
                        Dock = DockStyle.Top
                    };

                    // Picturebox'a click eventi atandı
                    pictureBox.Click += (object sender, EventArgs e) =>
                    {
                        PictureBox pb = sender as PictureBox;
                        int recommendedMovieId = Convert.ToInt32(pb.Tag);
                        ShowMovieDetails(recommendedMovieId);
                    };

                    //Label oluştur
                    Label lbl = new Label()
                    {
                        Text = similarMovies[i].title,
                        Dock = DockStyle.Bottom,
                        ForeColor = Color.White,
                        Width = 120,
                        Height = 25,
                    };

                    pn.Controls.Add(pictureBox);
                    pn.Controls.Add(lbl);

                    flowLayoutPanel2.Controls.Add(pn);
                }
            }
            else
            {
                MessageBox.Show("Benzer filmler alınamadı. Lütfen daha sonra tekrar deneyin.", "Hata");
            }
        }

        private async void BtnFavorite_Click(object sender, EventArgs e)
        {
            if (label6.Text == "Favorilerden Kaldır")
            {
                if (label7.Text == "Film")
                {
                    // Favorilerden kaldırma işlemini gerçekleştirin
                    await RemoveFromFavorites();
                }
                else if (label7.Text == "Dizi")
                {
                    // Favorilerden kaldırma işlemini gerçekleştirin
                    await RemoveFromFavorites2();
                }
            }
            else if (label6.Text == "Favorilere Ekle")
            {
                if (label7.Text == "Film")
                {
                    // Favorilere ekleme işlemini gerçekleştirin
                    await AddToFavorites();
                }
                else if (label7.Text == "Dizi")
                {
                    // Favorilere ekleme işlemini gerçekleştirin
                    await AddToFavorites2();
                }
            }

            // Favori durumunu kontrol et
            if (label7.Text == "Film")
            {
                CheckFavoriteStatus();
            }
            else if (label7.Text == "Dizi")
            {
                CheckFavoriteStatus2();
            }
        }


        private void Favoriler_VisibleChanged(object sender, EventArgs e)
        {
            flowLayoutPanel3.Controls.Clear();
            flowLayoutPanel1.Controls.Clear();
            RefreshFav();
        }

        private async void btnFavorite_Click(object sender, EventArgs e)
        {
            if (label6.Text == "Favorilerden Kaldır")
            {
                if (label7.Text == "Film")
                {
                    // Favorilerden kaldırma işlemini gerçekleştirin
                    await RemoveFromFavorites();
                }
                else if (label7.Text == "Dizi")
                {
                    // Favorilerden kaldırma işlemini gerçekleştirin
                    await RemoveFromFavorites2();
                }
            }
            else if (label6.Text == "Favorilere Ekle")
            {
                if (label7.Text == "Film")
                {
                    // Favorilere ekleme işlemini gerçekleştirin
                    await AddToFavorites();
                }
                else if (label7.Text == "Dizi")
                {
                    // Favorilere ekleme işlemini gerçekleştirin
                    await AddToFavorites2();
                }
            }

            // Favori durumunu kontrol et
            if (label7.Text == "Film")
            {
                CheckFavoriteStatus();
            }
            else if (label7.Text == "Dizi")
            {
                CheckFavoriteStatus2();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}
        