using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AnaSayfa
{
    public partial class Filmler : Form
    {
        private int currentMovieId;
        private SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mamie\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30");

        public Filmler()
        {
            InitializeComponent();
        }
        string email;
        public Filmler(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void Filmler_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 5; i++)
            {
                Button btn = new Button() { Tag = i, Text = i.ToString(), Dock = DockStyle.Fill, Font = new Font("Microsoft YaHei UI", 11), ForeColor = Color.White };
                btn.Click += getPopularMovies;
                tableLayoutPanel1.Controls.Add(btn);
            }

            // İlk butona otomatik tıklama yapmak için
            getPopularMovies(tableLayoutPanel1.Controls[0], EventArgs.Empty);
         
        }
        
        private void CheckFavoriteStatus()
        {   
                // Favori butonunun metnini belirle
                if (IsMovieInFavorites(currentMovieId))
                {
                    // Eğer film favorilerde ise "Favorilerden Kaldır" yaz
                    label4.Text = "Favorilerden Kaldır";
                }
                else
                {
                    // Eğer film favorilerde değilse "Favorilere Ekle" yaz
                    label4.Text = "Favorilere Ekle";
                }
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


        private async void getPopularMovies(object sender, EventArgs e)
        {  
            flowLayoutPanel1.Controls.Clear();

            // Sender'ı bir butona dönüştür
            Button btn = sender as Button;

            // Butonun Tag özelliğinden sayfa numarasını al
            string page = btn.Tag.ToString();

            var client = new RestClient($"https://api.themoviedb.org/3/movie/popular?language=tr-TR&page={page}");
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
            var response = await client.ExecuteAsync(request);

            dynamic jsonData = JObject.Parse(response.Content);
            dynamic data = jsonData.results;

            for (int i = 0; i < 20; i++)
            {
                Panel panel = new Panel() { Width = 250, Height = 350, Tag = i };
                PictureBox pictureBox = new PictureBox() { Width = 188, Height = 282, Tag = i, ImageLocation = "https://image.tmdb.org/t/p/w188_and_h282_bestv2" + data[i].poster_path, Dock = DockStyle.Top };

                // PictureBox'a tıklama olayını ekle
                pictureBox.Click += (s, ev) =>
                {
                    // PictureBox'a tıklandığında, tıklanan resmin indisini al
                    int index = (int)((PictureBox)s).Tag;

                    // PictureBox'a tıklandığında, ilgili filmin ID'sini al
                    int movieId = int.Parse(data[index].id.ToString());

                    CheckFavoriteStatus();
                    // Ve bu ID ile ilgili film detaylarını göster
                    ShowMovieDetails(movieId);
                };

                Label label = new Label() { ForeColor = Color.White, Font = new Font("Microsoft YaHei UI", 11), Width = 188, Height = 50, Tag = i, Text = data[i].title, Dock = DockStyle.Bottom };
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(panel);
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
            request.AddHeader("Authorization", "Bearer (Your The Movie Database Token)");
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


        private async void btnFavorite_Click(object sender, EventArgs e)
        {
                if (label4.Text == "Favorilerden Kaldır")
                {
                    // Favorilerden kaldırma işlemini gerçekleştirin
                    await RemoveFromFavorites();
                }
                else if (label4.Text == "Favorilere Ekle")
                {
                    // Favorilere ekleme işlemini gerçekleştirin
                    await AddToFavorites();
                }
                // Favori durumunu kontrol et
                CheckFavoriteStatus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}
