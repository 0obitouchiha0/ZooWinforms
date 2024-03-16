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
        DB db;
        public FormLog()
        {
            InitializeComponent();
            db = new DB("Server=localhost;Port=5432;Database=zoo;Username=postgres;Password=postgres;");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int result = db.login(logintext.Text, passtext.Text);
                if (result > 0)
                {
                    Hide();
                    FormOrders formOrders = new FormOrders();
                    formOrders.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Неправильно введены данные");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message);
            }
        }

        private void visitBtn_Click(object sender, EventArgs e)
        {
            Hide();
            FormOpen formOpen = new FormOpen();
            formOpen.ShowDialog();
        }
    }
}
