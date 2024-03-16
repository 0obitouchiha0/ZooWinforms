using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoo
{
    public partial class FormOrders : Form
    {
        DB db;
        public FormOrders()
        {
            InitializeComponent();
            db = new DB("Server=localhost;Port=5432;Database=zoo;Username=postgres;Password=postgres;");
            dataGridView1.RowHeadersVisible = false;
            fillDataGridView();
        }

        public void fillDataGridView()
        {
            DataTable dt = db.getOrders();
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = dataGridView1.Rows[e.RowIndex];
            MessageBox.Show(DateTime.TryParse(selectedRow.Cells[1].Value.ToString(), out DateTime a).ToString());
            int id = int.Parse(selectedRow.Cells[0].Value.ToString());
            DateTime orderdate = DateTime.Parse(selectedRow.Cells[1].Value.ToString());
            int ordernumber = int.Parse(selectedRow.Cells[2].Value.ToString());
            string productname = selectedRow.Cells[3].Value.ToString();
            int sum = int.Parse(selectedRow.Cells[4].Value.ToString());
            int discountsum = int.Parse(selectedRow.Cells[5].Value.ToString());
            string point = selectedRow.Cells[6].Value.ToString();
            int code = int.Parse(selectedRow.Cells[7].Value.ToString());
            FormUpdateOrder formUpdateOrder = new FormUpdateOrder(this, id, orderdate, ordernumber, productname, sum, discountsum, point, code);
            formUpdateOrder.Show();
        }
    }
}
