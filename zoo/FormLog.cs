using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace zoo
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();

        }
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=database;Username=postgres;Password=0000;");
        private void but_login(object sender, EventArgs e)
        {
            bool blnfound = false;
            try
            {
                conn.Open();

                NpgsqlCommand npgsql = new NpgsqlCommand("Select * from users where username ='" + logintext.Text + "'and pass='" + passtext.Text + "'", conn);
                NpgsqlDataReader dr = npgsql.ExecuteReader();

                if (dr.Read())
                {
                    blnfound = true;
                    this.Hide();
                    FormOpen formOpen = new FormOpen();
                    formOpen.ShowDialog();
                }
                else if (blnfound == false)
                {
                    MessageBox.Show("Неправильно введены данные");
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message, "");
                conn.Close();
            }
        }

        private void but_visit(object sender, EventArgs e)
        {
            this.Hide();
            FormOpen frmOpen = new FormOpen();
            frmOpen.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
