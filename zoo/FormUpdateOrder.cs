using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace zoo
{
    public partial class FormUpdateOrder : Form
    {
        DB db;
        FormOrders form;
        int id;
        public FormUpdateOrder(FormOrders form, int id, DateTime orderdate, int ordernumber, string productname, int sum, int discountsum, string point, int code)
        {
            InitializeComponent();
            db = new DB("Server=localhost;Port=5432;Database=zoo;Username=postgres;Password=postgres;");
            this.form = form;
            this.id = id;

            textBox1.Text = orderdate.ToString();
            textBox2.Text = ordernumber.ToString();
            textBox3.Text = productname;
            textBox4.Text = sum.ToString();
            textBox5.Text = discountsum.ToString();
            textBox6.Text = point;
            textBox7.Text = code.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string orderdate = textBox1.Text;
            string ordernumber = textBox2.Text;
            string productname = textBox3.Text;
            string sum = textBox4.Text;
            string discountsum = textBox5.Text;
            string point = textBox6.Text;
            string code = textBox7.Text;
            try
            {
                db.updateOrder(id, DateTime.Parse(orderdate), int.Parse(ordernumber), productname, int.Parse(sum), int.Parse(discountsum), point, int.Parse(code));
                MessageBox.Show("Данные успешно обновлены в таблице.");
                form.fillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                db.deleteOrder(id);
                MessageBox.Show("Запись успешно удалена из таблицы.");
                form.fillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении записи: {ex.Message}");
            }
        }
    }
}
