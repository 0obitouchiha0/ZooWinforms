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
        public FormOpen()
        {
            InitializeComponent();
            SelectData();
            panel1.Visible = false;
        }
        private string connString = "Server=localhost;Port=5432;Database=database;Username=postgres;Password=0000;";
        private void SelectData()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string qsl = "select описание_товара, наименование_товара, производитель, цена, размер_скидки, фото from item";
                using (NpgsqlCommand cmd = new NpgsqlCommand(qsl, conn))
                {
                    using (NpgsqlDataAdapter reader = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        reader.Fill(dt);
                        dataGridView1.DataSource = dt;

                    }
                }
            };
        }

        private void FormOpen_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string imagePath = row.Cells["фото"].Value.ToString();

                if (!string.IsNullOrEmpty(imagePath))
                {
                    pictureBox1.ImageLocation = imagePath;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    MessageBox.Show("Изображение не найдено");
                }
            }
        }
        private int clickedRowIndex = -1;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Сохраняем индекс строки, на которую нажал пользователь правой кнопкой мыши
                clickedRowIndex = e.RowIndex;

                // Показываем panel1
                panel1.Visible = true;

            }
        }

        private void LookButton_Click(object sender, EventArgs e)
        {
            if (clickedRowIndex >= 0 && clickedRowIndex < dataGridView1.Rows.Count)
            {
                // Получаем данные из выбранной строки
                string description = dataGridView1.Rows[clickedRowIndex].Cells["описание_товара"].Value.ToString();
                string name = dataGridView1.Rows[clickedRowIndex].Cells["наименование_товара"].Value.ToString();
                string manufacturer = dataGridView1.Rows[clickedRowIndex].Cells["производитель"].Value.ToString();
                decimal price = Convert.ToDecimal(dataGridView1.Rows[clickedRowIndex].Cells["цена"].Value);
                decimal discount = Convert.ToDecimal(dataGridView1.Rows[clickedRowIndex].Cells["размер_скидки"].Value);

                NpgsqlConnection connection = new NpgsqlConnection("Server=localhost;Port=5432;Database=database;Username=postgres;Password=0000;");
                FormBuy formBuy = new FormBuy(description, name, manufacturer, price, discount, connection);
                formBuy.ShowDialog();

            }
        }


    }
}
