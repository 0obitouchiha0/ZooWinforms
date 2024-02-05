using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoo
{
    public partial class FormBuy : Form
    {

        private decimal price;
        private decimal discount;
        private Random random = new Random();

        private string description;
        private string name;
        private string manufacturer;

        public FormBuy(string description, string name, string manufacturer, decimal price, decimal discount, NpgsqlConnection connection)
        {
            InitializeComponent();

            // Сохраняем переданные данные в полях класса
            this.description = description;
            this.name = name;
            this.manufacturer = manufacturer;
            conn = connection;

            // Передаем данные в richTextBoxDetails
            richTextBox1.Text = $"Описание товара: {this.description}\n" +
                                $"Наименование товара: {this.name}\n" +
                                $"Производитель: {this.manufacturer}\n" +
                                $"Цена: {price:C}\n" +
                                $"Размер скидки: {discount}\n" +
                                $"Дата заказа: {DateTime.Now:dd/MM/yyyy} \n" +
                                $"Номер заказа: {GenerateOrderNumber()}\n" +
                                $"Пункт выдачи:  {comboBox1.SelectedItem} \n" +
                                $"Код получение: {GenerateRandomCode()}\n";

            numericUpDown1.Value = 1;
            this.price = price;
            this.discount = discount;

            comboBox1.Items.Add("Пункт 1");
            comboBox1.Items.Add("Пункт 2");
            comboBox1.Items.Add("Пункт 3");
        }

        private void UpdateRichTextBox()
        {
            decimal quantity = numericUpDown1.Value;

            if (quantity <= 0)
            {
                MessageBox.Show("Заказ не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FormOpen formoOpen = new FormOpen();
                formoOpen.Show();
                this.Close();
            }
            else
            {
                decimal totalAmount = (this.price * quantity) - this.discount;

                // Обновляем содержимое richTextBox1 на основе сохраненных данных
                richTextBox1.Text = $"Описание товара: {this.description}\n" +
                                    $"Наименование товара: {this.name}\n" +
                                    $"Производитель: {this.manufacturer}\n" +
                                    $"Цена: {totalAmount:C}\n" +
                                    $"Размер скидки: {this.discount}\n" +
                                    $"Дата заказа: {DateTime.Now:dd/MM/yyyy} \n" +
                                    $"Номер заказа: {GenerateOrderNumber()}\n" +
                                    $"Пункт выдачи:  {comboBox1.SelectedItem} \n" +
                                    $"Код получение: {GenerateRandomCode()}\n";
                textBox1.Text = totalAmount.ToString();
            }
        }
        private string GenerateOrderNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private string GenerateRandomCode()
        {
            // Генерация трех случайных цифр
            return random.Next(100, 999).ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal quantity = numericUpDown1.Value;

            UpdateRichTextBox();
        }
        private NpgsqlConnection conn;
        private void Buy_Click(object sender, EventArgs e)
        {
            // Получаем значения из richTextBox1
            string names = this.name;
            decimal discount = this.discount;
            decimal totalAmount = decimal.Parse(textBox1.Text); // Общая сумма заказа
            string orderNumber = GenerateOrderNumber();
            string pickupLocation = comboBox1.SelectedItem.ToString();
            string retrievalCode = GenerateRandomCode();

            // Сохраняем данные в базу данных
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;

                // Запрос для вставки данных в таблицу orderuser
                string sql = "INSERT INTO orderuser (дата_заказа, номер_заказа, название_товара, сумма_заказа, сумма_скидки, пункт_выдачи, код_получения) " +
                             "VALUES (@OrderDate, @OrderNumber, @names, @TotalAmount, @discount, @PickupLocation, @RetrievalCode)";

                cmd.CommandText = sql;

                // Параметры запроса
                cmd.Parameters.Add(new NpgsqlParameter("@OrderDate", NpgsqlDbType.Date)).Value = DateTime.Now.Date;
                cmd.Parameters.AddWithValue("@OrderNumber", long.Parse(orderNumber));
                cmd.Parameters.AddWithValue("@names", names); // Предполагается, что название товара берется из поля name
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@discount", discount);
                cmd.Parameters.AddWithValue("@PickupLocation", pickupLocation);
                cmd.Parameters.AddWithValue("@RetrievalCode", int.Parse(retrievalCode));

                // Выполняем запрос
                cmd.ExecuteNonQuery();

                MessageBox.Show("Заказ успешно оформлен и сохранен в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            UpdateRichTextBox();
        }
    }
}
