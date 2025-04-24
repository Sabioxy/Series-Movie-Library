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
using System.Text.RegularExpressions;

namespace AnaSayfa
{
    public partial class frmReg : Form
    {
        //Sql Sign Up için
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mamie\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30");

        //Taşınabilirlik için
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmReg()
        {
            InitializeComponent();
        }

        private void frmReg_Load(object sender, EventArgs e)
        {
            signup_password.PasswordChar = '•';
            kryptonTextBox1.PasswordChar = '•';
        }

        private void signup_showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (signup_showPass.Checked)
            {
                signup_password.PasswordChar = '\0';
            }
            else
            {
                signup_password.PasswordChar = '•';
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            //Gerçek email stilinde olmasını sağlar
            string email = signup_email.Text.Trim();


            if (ValidateEmail(email))
            {
                if (signup_password.Text != kryptonTextBox1.Text)
                {
                    MessageBox.Show("Şifreleriniz Uyuşmuyor.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
           if (signup_email.Text == "" || signup_password.Text == "" || signup_username.Text == "")
                {
                    MessageBox.Show("Lütfen tüm bilgilerinizi giriniz", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            connect.Open();

                            // Girilen email adresinin zaten veritabanında kayıtlı olup olmadığını kontrol et
                            string checkEmailQuery = "SELECT COUNT(*) FROM admin WHERE email = @email";

                            using (SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, connect))
                            {
                                checkEmailCmd.Parameters.AddWithValue("@email", signup_email.Text.Trim());

                                int emailCount = (int)checkEmailCmd.ExecuteScalar();

                                if (emailCount > 0)
                                {
                                    MessageBox.Show(signup_email.Text + " zaten kayıtlı.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    // Email adresi veritabanında bulunmadığı için yeni kayıt ekleme işlemine devam et
                                    string insertData = "INSERT INTO admin (email, username, password_, date_created ) " +
                                                        "VALUES(@email, @username, @password, @date)";

                                    DateTime date = DateTime.Today;

                                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@email", signup_email.Text.Trim());
                                        cmd.Parameters.AddWithValue("@username", signup_username.Text.Trim());
                                        cmd.Parameters.AddWithValue("@password", signup_password.Text.Trim());
                                        cmd.Parameters.AddWithValue("@date", date);

                                        cmd.ExecuteNonQuery();

                                        MessageBox.Show("Başarıyla Kayıt Olundu", "Bilgilendirme Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        frmLog flog = new frmLog();
                                        flog.Show();
                                        this.Hide();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Veritabanına erişim hatası: " + ex.Message, "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Geçersiz email adresi.", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }


        private void label7_Click(object sender, EventArgs e)
        {
            frmLog form2 = new frmLog();
            form2.Show();
            this.Hide();
        }


        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (kryptonCheckBox1.Checked)
            {
                kryptonTextBox1.PasswordChar = '\0';
            }
            else
            {
                kryptonTextBox1.PasswordChar = '•';
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void kryptonTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
              kryptonButton1.PerformClick();
            }
        }

        private bool ValidateEmail(string email)
        {
            string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
