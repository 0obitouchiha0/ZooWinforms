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
        DB db;
        Random random = new Random();
        int price;
        int discount;
        string description;
        string name;
        string producer;

        public FormBuy(string description, string name, string producer, int price, int discount)
        {
            InitializeComponent();
            db = new DB("Server=localhost;Port=5432;Database=zoo;Username=postgres;Password=postgres;");
            this.description = description;
            this.name = name;
            this.producer = producer;
            this.price = price;
            this.discount = discount;

            richTextBox1.Text = $"Описание товара: {this.description}\n" +
                                $"Наименование товара: {this.name}\n" +
                                $"Производитель: {this.producer}\n" +
                                $"Цена: {price:C}\n" +
                                $"Размер скидки: {discount}\n" +
                                $"Дата заказа: {DateTime.Now:dd/MM/yyyy} \n" +
                                $"Номер заказа: {generateOrderNumber()}\n" +
                                $"Пункт выдачи:  {comboBox1.SelectedItem} \n" +
                                $"Код получение: {generateRandomCode()}\n";
            numericUpDown1.Value = 1;
        }

        private void UpdateRichTextBox()
        {
            decimal quantity = numericUpDown1.Value;

            if (quantity <= 0)
            {
                MessageBox.Show("Заказ не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FormOpen formoOpen = new FormOpen();
                formoOpen.Show();
                Close();
            }
            else
            {
                decimal totalAmount = (price - discount) * quantity;

                richTextBox1.Text = $"Описание товара: {this.description}\n" +
                                    $"Наименование товара: {this.name}\n" +
                                    $"Производитель: {this.producer}\n" +
                                    $"Цена: {totalAmount:C}\n" +
                                    $"Размер скидки: {this.discount}\n" +
                                    $"Дата заказа: {DateTime.Now:dd/MM/yyyy} \n" +
                                    $"Номер заказа: {generateOrderNumber()}\n" +
                                    $"Пункт выдачи:  {comboBox1.SelectedItem} \n" +
                                    $"Код получение: {generateRandomCode()}\n";
                textBox1.Text = totalAmount.ToString();
            }
        }
        private int generateOrderNumber()
        {
            return random.Next(100, 9999);
        }

        private int generateRandomCode()
        {
            return random.Next(100, 9999);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateRichTextBox();
        }
        private void Buy_Click(object sender, EventArgs e)
        {
            try
            {
                int fullDiscount = discount * int.Parse(numericUpDown1.Value.ToString());
                int sum = int.Parse(textBox1.Text);
                int orderNumber = generateOrderNumber();
                string point = comboBox1.SelectedItem.ToString();
                int code = generateRandomCode();

                db.addOrder(DateTime.Now, orderNumber, name, sum, fullDiscount, point, code);

                MessageBox.Show("Заказ успешно оформлен и сохранен в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных: " + ex.Message);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateRichTextBox();
        }
    }
}
