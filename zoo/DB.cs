using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoo
{
    internal class DB
    {
        NpgsqlConnection connection;

        public DB(string connectionString)
        {
            connection = new NpgsqlConnection(connectionString);
        }

        public NpgsqlConnection getConnection()
        {
            return connection;
        }

        public int login(string username, string password)
        {
            string sql = $"SELECT * FROM users WHERE username='{username}' AND password='{password}'";
            var command = new NpgsqlCommand(sql, connection);
            connection.Open();
            int result = (int)command.ExecuteScalar();
            connection.Close();
            return result;
        }

        public DataTable getProducts()
        {
            string query = $"SELECT * FROM products";
            var command = new NpgsqlCommand(query, connection);
            connection.Open();
            var adapter = new NpgsqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            connection.Close();
            return table;
        }

        public void addProduct(string name, string description, string producer, int price, int discount, string img)
        {
            connection.Open();
            string query = "INSERT INTO products (name, description, producer, price, discount, img) " +
                              $"VALUES ('{name}', '{description}', '{producer}', '{price}', '{discount}', '{img}')";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void updateProduct(string name, string description, string producer, int price, int discount, string img)
        {
            connection.Open();
            string query = $"UPDATE products SET name='{name}', description='{description}', producer='{producer}', price='{price}', " +
                        $"discount='{discount}', img='{img}' WHERE name='{name}'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void deleteProduct(int id)
        {
            connection.Open();
            string query = $"DELETE FROM products WHERE id='{id}'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public DataTable getOrders()
        {
            string query = $"SELECT * FROM orders";
            var command = new NpgsqlCommand(query, connection);
            connection.Open();
            var adapter = new NpgsqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            connection.Close();
            return table;
        }
        public void addOrder(DateTime orderdate, int ordernumber, string productname, int sum, int discountsum, string point, int code)
        {
            connection.Open();
            string query = $"INSERT INTO orders (orderdate, ordernumber, productname, sum, discountsum, point, code) VALUES ('{orderdate}', '{ordernumber}', '{productname}', '{sum}', '{discountsum}', '{point}', '{code}')";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void updateOrder(int id, DateTime orderdate, int ordernumber, string productname, int sum, int discountsum, string point, int code)
        {
            connection.Open();
            string query = $"UPDATE orders SET orderdate='{orderdate}', ordernumber='{ordernumber}', productname='{productname}', sum='{sum}', discountsum='{discountsum}', point='{point}', code='{code}' WHERE ordernumber='{ordernumber}'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void deleteOrder(int id)
        {
            connection.Open();
            string query = $"DELETE FROM orders WHERE id='{id}'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
