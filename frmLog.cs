using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AnaSayfa
{
    public partial class frmLog : Form
    {
        //Sql Login için
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mamie\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30");

        //Taşınabilirlik için
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public frmLog()
        {
            InitializeComponent();
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

        private void label2_Click(object sender, EventArgs e)
        {
            frmReg reg1 = new frmReg();
            reg1.Show();
            this.Hide();
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (login_showPass.Checked)
            {
                login_password.PasswordChar = '\0';
            }
            else
            {
                login_password.PasswordChar = '•';
            }
        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            login_password.PasswordChar = '•';
        }

        public void btnGiris_Click(object sender, EventArgs e)
        {
            if (login_email.Text == "" || login_password.Text == "")
            {
                MessageBox.Show("Lütfen tüm bilgilerinizi giriniz", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    // Bağlantının açık olup olmadığını kontrol ediyoruz
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open();
                    }

                    String selectData = "SELECT * FROM admin WHERE email = @email AND password_ = @password";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@email", login_email.Text);
                        cmd.Parameters.AddWithValue("@password", login_password.Text);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {
                            // Giriş başarılı, kullanıcı adını al
                            string username = "";
                            string selectUsernameQuery = "SELECT username FROM admin WHERE email = @Email";

                            using (SqlCommand selectUsernameCmd = new SqlCommand(selectUsernameQuery, connect))
                            {
                                selectUsernameCmd.Parameters.AddWithValue("@Email", login_email.Text);

                                using (SqlDataReader reader = selectUsernameCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        username = reader["username"].ToString();
                                    }
                                }
                            }

                            // AnaSayfa formunu aç
                            AnaSayfa anasayfa = new AnaSayfa(login_email.Text, username);
                            anasayfa.Show();
                            this.Hide(); // Giriş formunu gizle, kapatma
                        }
                        else
                        {
                            MessageBox.Show("Email/Şifre Hatalı", "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı Hatası: " + ex.Message, "HATA MESAJI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Bağlantıyı kapatıyoruz
                    if (connect.State == ConnectionState.Open)
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void login_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnGiris.PerformClick();
            }
        }
    }
}
