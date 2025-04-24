using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnaSayfa
{
    public partial class AnaSayfa : Form
    {

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private Diziler Diziler;
        private Filmler Filmler;
        private Listeler Listeler;
        private Favoriler Favoriler;
        private Ayarlar Ayarlar;

        public AnaSayfa()
        {
            InitializeComponent();
        }
        string login_email;
        string username;
        public AnaSayfa(string login_email, string username)
        {
            InitializeComponent();
            this.login_email = login_email;
            this.username = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void label1_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
            DizileriGoster();
            GizleForms(Filmler, Listeler, Favoriler, Ayarlar);   
        }

        private void label2_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
            FilmleriGoster();
            GizleForms(Diziler, Listeler, Favoriler, Ayarlar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnaSayfayiGoster();
            GizleForms(Diziler, Filmler, Listeler, Favoriler, Ayarlar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
            ListeleriGoster();
            GizleForms(Diziler, Filmler, Favoriler, Ayarlar);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
            FavorileriGoster();
            GizleForms(Diziler, Filmler, Listeler, Ayarlar);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel4.Enabled = false;
            AyarlarFormunuGoster();
            GizleForms(Diziler, Filmler, Listeler, Favoriler);
        }

        private void DizileriGoster()
        {
            if (Diziler == null)
            {
                Diziler = new Diziler(label3.Text);
                Diziler.TopLevel = false;
                Panel customPanel = panel3;
                this.Controls.Add(customPanel);
                customPanel.Controls.Add(Diziler);
            }
            Diziler.Show();
        }

        private void FilmleriGoster()
        {
            if (Filmler == null)
            {
                Filmler = new Filmler(label3.Text);
                Filmler.TopLevel = false;
                Panel customPanel = panel3;
                this.Controls.Add(customPanel);
                customPanel.Controls.Add(Filmler);       
            }
            Filmler.Show();
        }

        private void AnaSayfayiGoster()
        {
            panel4.Show();
            panel4.Visible = true;
            panel4.Enabled = true;
        }

        private void ListeleriGoster()
        {
            if (Listeler == null)
            {
                Listeler = new Listeler(label3.Text);
                Listeler.TopLevel = false;
                Panel customPanel = panel3;
                this.Controls.Add(customPanel);
                customPanel.Controls.Add(Listeler);
            }
            Listeler.Show();
        }

        private void FavorileriGoster()
        {
            if (Favoriler == null)
            {
                Favoriler = new Favoriler(label3.Text);
                Favoriler.TopLevel = false;
                Panel customPanel = panel3;
                this.Controls.Add(customPanel);
                customPanel.Controls.Add(Favoriler);
            }
            Favoriler.Show();
        }

        private void AyarlarFormunuGoster()
        {
            if (Ayarlar == null)
            {
                Ayarlar = new Ayarlar(username, login_email);
                Ayarlar.TopLevel = false;
                Panel customPanel = panel3;
                this.Controls.Add(customPanel);
                customPanel.Controls.Add(Ayarlar);
            }
            Ayarlar.Show();
        }

        private void GizleForms(params Form[] forms)
        {
            foreach (var form in forms)
            {
                form?.Hide();
            }
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            label3.Text = login_email;
            label9.Text = username + " Ne Yapmak İstersiniz?";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Köprünüz Yıkılmıştır. B003");
        }
    }
}
