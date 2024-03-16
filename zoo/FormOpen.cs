using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace zoo
{
    public partial class FormOpen : Form
    {
        DB db;
        int clickedRowIndex;
        public FormOpen()
        {
            InitializeComponent();
            db = new DB("Server=localhost;Port=5432;Database=zoo;Username=postgres;Password=postgres;");
            dataGridView1.RowHeadersVisible = false;
            fillDataGridView();
        }
        private void fillDataGridView()
        {
            DataTable dt = db.getProducts();
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string imagePath = row.Cells[6].Value.ToString();
            if (imagePath != null)
            {
                pictureBox1.Load(imagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                MessageBox.Show("Изображение не найдено");
            }
        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                clickedRowIndex = e.RowIndex;
                panel1.Visible = true;

            }
        }

        private void LookButton_Click(object sender, EventArgs e)
        {
            string name = dataGridView1.Rows[clickedRowIndex].Cells[1].Value.ToString();
            string description = dataGridView1.Rows[clickedRowIndex].Cells[2].Value.ToString();
            string manufacturer = dataGridView1.Rows[clickedRowIndex].Cells[3].Value.ToString();
            int price = int.Parse(dataGridView1.Rows[clickedRowIndex].Cells[4].Value.ToString());
            int discount = int.Parse(dataGridView1.Rows[clickedRowIndex].Cells[5].Value.ToString());

            FormBuy formBuy = new FormBuy(description, name, manufacturer, price, discount);
            formBuy.ShowDialog();
        }
    }
}
