using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace database
{
    public partial class Form1 : Form
    {
        DatabaseHandler db;
        public Form1()
        {
            InitializeComponent();
            start();
        }
        void start()
        {
            guna2Button2.Text = "delet selected item";
            guna2Button1.Text = "add item";
            label1.Text = "name";
            label2.Text = "price";
            db = new DatabaseHandler();
            updatedrinks();
            guna2Button2.Click += (s, e) => {
                db.deleteone(listBox1.SelectedIndex);
                updatedrinks();
            };
            guna2Button1.Click += (s, e) => {
                if (guna2TextBox1.Text.Length >=3 && guna2TextBox2.Text.Length >= 3)
                {
                    db.addone(guna2TextBox1.Text, Convert.ToInt32(guna2TextBox2.Text));
                    updatedrinks();
                }
            };
        }
        void updatedrinks()
        {
            listBox1.Items.Clear();
            foreach(drink item in db.readall())
            {
                listBox1.Items.Add(item.name + "; " + item.price);
            }

        }


    }
    public class drink
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
    }
    public class DatabaseHandler
    {
        string serveraddress;
        string username;
        string password;
        string dbname;
        string connectionstring;
        MySqlConnection connection;
        public DatabaseHandler()
        {
            //szerver címe
            serveraddress = "localhost";
            username = "root";
            password = "";
            dbname = "energydrinks";
            connectionstring = $"Server={serveraddress};Database={dbname};User={username};Password={password}";
            connection = new MySqlConnection(connectionstring);
        }
        public List<drink> readall()
        {
            List<drink> drinks = new List<drink>();
            try
            {
                connection.Open();
                string query = "select * from drinks";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    drink onedrink = new drink();
                    onedrink.id = read.GetInt32(read.GetOrdinal("ID"));
                    onedrink.name = read.GetString(read.GetOrdinal("name"));
                    onedrink.price = read.GetInt32(read.GetOrdinal("price"));
                    drinks.Add(onedrink);
                }
                read.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return drinks;
        }
        public void deleteone(int id)
        {
            try
            {
                connection.Open();
                string query = $"DELETE FROM drinks WHERE ID =  '{id + 1}'";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }

        }
        public void addone(string name, int price)
        {
            try
            {
                connection.Open();
                string query = $"insert into drinks (    name, price) values ('{name}','{price}')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }
    }
}
