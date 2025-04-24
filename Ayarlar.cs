using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AnaSayfa
{
    public partial class Ayarlar : Form
    {

        private SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mamie\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30");

        public Ayarlar()
        {
            InitializeComponent();
        }
        string username;
        string login_email;
        public Ayarlar(string username, string login_email)
        {
            InitializeComponent();
            this.username = username;
            this.login_email = login_email;
        }

        private void Ayarlar_Load(object sender, EventArgs e)
        {
            label2.Text = username;
            label3.Text = login_email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxNewEmail.Enabled = true;
            textBoxNewEmail.Visible = true;
            btnUpdateEmail.Enabled = true;
            btnUpdateEmail.Visible = true;
            button6.Visible = true;
            button6.Enabled = true;
            label4.Visible = true;   
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBoxNewPassword.Enabled = true;
            textBoxNewPassword.Visible = true;
            btnUpdatePassword.Enabled = true;
            btnUpdatePassword.Visible = true;
            button4.Visible = true;
            button4.Enabled = true;
            label5.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBoxNewUsername.Enabled = true;
            textBoxNewUsername.Visible = true;
            btnUpdateUsername.Enabled = true;
            btnUpdateUsername.Visible = true;
            button5.Visible = true;
            button5.Enabled = true;
            label6.Visible = true;
        }

        private void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            string newEmail = textBoxNewEmail.Text; // TextBox'tan yeni email'i al

            if (!string.IsNullOrWhiteSpace(newEmail)) // Yeni email boş değilse devam et
            {
                if (connect.State != ConnectionState.Open) // Bağlantı açık değilse
                {
                    try
                    {
                        connect.Open(); // Bağlantıyı aç

                        // Güncelleme sorgusu oluştur
                        string updateEmailQuery = "UPDATE admin SET email = @NewEmail WHERE email = @CurrentEmail";

                        using (SqlCommand updateEmailCmd = new SqlCommand(updateEmailQuery, connect))
                        {
                            updateEmailCmd.Parameters.AddWithValue("@NewEmail", newEmail); // Yeni email parametresi ekle
                            updateEmailCmd.Parameters.AddWithValue("@CurrentEmail", login_email); // Mevcut oturumun email'i parametresi ekle

                            int rowsAffected = updateEmailCmd.ExecuteNonQuery(); // Sorguyu veritabanına gönder ve etkilenen satır sayısını al

                            if (rowsAffected > 0) // Eğer en az bir satır etkilendiyse
                            {
                                MessageBox.Show("Email başarıyla güncellendi.\nUygulamayı  yeniden başlatın", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit();
                            }
                            else
                            {
                                MessageBox.Show("Email güncellenirken bir hata oluştu.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanına erişim hatası: " + ex.Message, "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Bağlantıyı kapat
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir email girin.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text; // TextBox'tan yeni şifreyi al

            if (!string.IsNullOrWhiteSpace(newPassword)) // Yeni şifre boş değilse devam et
            {
                if (connect.State != ConnectionState.Open) // Bağlantı açık değilse
                {
                    try
                    {
                        connect.Open(); // Bağlantıyı aç

                        // Güncelleme sorgusu oluştur
                        string updatePasswordQuery = "UPDATE admin SET password_ = @NewPassword WHERE email = @CurrentEmail";

                        using (SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, connect))
                        {
                            updatePasswordCmd.Parameters.AddWithValue("@NewPassword", newPassword); // Yeni şifre parametresi ekle
                            updatePasswordCmd.Parameters.AddWithValue("@CurrentEmail", login_email); // Mevcut oturumun email'i parametresi ekle

                            int rowsAffected = updatePasswordCmd.ExecuteNonQuery(); // Sorguyu veritabanına gönder ve etkilenen satır sayısını al

                            if (rowsAffected > 0) // Eğer en az bir satır etkilendiyse
                            {
                                MessageBox.Show("Şifre başarıyla güncellendi.\nUygulamayı yeniden başlatın", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit();
                            }
                            else
                            {
                                MessageBox.Show("Şifre güncellenirken bir hata oluştu.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanına erişim hatası: " + ex.Message, "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Bağlantıyı kapat
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir şifre girin.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateUsername_Click(object sender, EventArgs e)
        {
            string newUsername = textBoxNewUsername.Text; // TextBox'tan yeni kullanıcı adını al

            if (!string.IsNullOrWhiteSpace(newUsername)) // Yeni kullanıcı adı boş değilse devam et
            {
                if (connect.State != ConnectionState.Open) // Bağlantı açık değilse
                {
                    try
                    {
                        connect.Open(); // Bağlantıyı aç

                        // Güncelleme sorgusu oluştur
                        string updateUsernameQuery = "UPDATE admin SET username = @NewUsername WHERE email = @CurrentEmail";

                        using (SqlCommand updateUsernameCmd = new SqlCommand(updateUsernameQuery, connect))
                        {
                            updateUsernameCmd.Parameters.AddWithValue("@NewUsername", newUsername); // Yeni kullanıcı adı parametresi ekle
                            updateUsernameCmd.Parameters.AddWithValue("@CurrentEmail", login_email); // Mevcut oturumun email'i parametresi ekle

                            int rowsAffected = updateUsernameCmd.ExecuteNonQuery(); // Sorguyu veritabanına gönder ve etkilenen satır sayısını al

                            if (rowsAffected > 0) // Eğer en az bir satır etkilendiyse
                            {
                                MessageBox.Show("Kullanıcı adı başarıyla güncellendi.\nUygulamayı yeniden başlatın ", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit();
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı adı güncellenirken bir hata oluştu.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanına erişim hatası: " + ex.Message, "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close(); // Bağlantıyı kapat
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir kullanıcı adı girin.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBoxNewEmail.Enabled = false;
            textBoxNewEmail.Visible = false;
            btnUpdateEmail.Enabled = false;
            btnUpdateEmail.Visible = false;
            button6.Visible = false;
            button6.Enabled = false;
            label4.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxNewPassword.Enabled = false;
            textBoxNewPassword.Visible = false;
            btnUpdatePassword.Enabled = false;
            btnUpdatePassword.Visible = false;
            button4.Visible = false;
            button4.Enabled = false;
            label5.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxNewUsername.Enabled = false;
            textBoxNewUsername.Visible = false;
            btnUpdateUsername.Enabled = false;
            btnUpdateUsername.Visible = false;
            button5.Visible = false;
            button5.Enabled = false;
            label6.Visible = false;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("EMİN MİSİNİZ?\nHESABINIZ KALICI OLARAK SİLİNECEKTİR!!!", "UYARI!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open();
                    }

                    string deleteQuery = "DELETE FROM admin WHERE email = @Email";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@Email", login_email);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Hesabınız başarıyla silinmiştir.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show("Hesabınız silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (connect.State == ConnectionState.Open)
                    {
                        connect.Close();
                    }
                }
            }
        }
    }
}
