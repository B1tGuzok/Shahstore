using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShahStore
{
    public partial class Form2 : Form
    {
        private string role;
        private Form1 form1;
        public bool showSearch;

        public Form2(string role, Form1 form1)
        {
            InitializeComponent();
            this.role = role;
            this.form1 = form1;
        }

        public void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shahstoreDataSet.Users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter.Fill(this.shahstoreDataSet.Users);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shahstoreDataSet.PurchaseList". При необходимости она может быть перемещена или удалена.
            this.purchaseListTableAdapter.Fill(this.shahstoreDataSet.PurchaseList);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shahstoreDataSet.Orders". При необходимости она может быть перемещена или удалена.
            this.ordersTableAdapter.Fill(this.shahstoreDataSet.Orders);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shahstoreDataSet.Clients". При необходимости она может быть перемещена или удалена.
            this.clientsTableAdapter.Fill(this.shahstoreDataSet.Clients);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shahstoreDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.shahstoreDataSet.Products);

            if (role == "storekeeper")
            {
                tabPage2.Parent = null;
                tabPage3.Parent = null;
                tabPage4.Parent = null;
                tabPage5.Parent = null;
            }
            else if (role == "customer")
            {
                tabPage1.Parent = null;
                tabPage5.Parent = null;
            }
        }

        public SqlConnection GetConnection()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=LAPTOPPRO15;Initial Catalog=Shahstore;Integrated Security=True;Encrypt=False");
            return sqlcon;
        }

        //-------------------------Товары----------------------------

        private void button1_Click(object sender, EventArgs e)  //изменить
        {
                if (CheckPrice(textBox4.Text))
                {
                    SqlConnection MyConnection = GetConnection();
                    string ComDel = "UPDATE Products SET Name = @name, Description = @description, Price = @price, Quantity = @quantity WHERE ProductId = @ProductId";
                    SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                    SqlParameter pr1 = new SqlParameter("@name", textBox2.Text);
                    SqlParameter pr2 = new SqlParameter("@description", textBox3.Text);
                    SqlParameter pr3 = new SqlParameter("@price", textBox4.Text);
                    SqlParameter pr4 = new SqlParameter("@quantity", textBox5.Text);
                    SqlParameter pr5 = new SqlParameter("@ProductId", textBox1.Text);
                    cmd1.Parameters.Add(pr1);
                    cmd1.Parameters.Add(pr2);
                    cmd1.Parameters.Add(pr3);
                    cmd1.Parameters.Add(pr4);
                    cmd1.Parameters.Add(pr5);

                    MyConnection.Open();
                    cmd1.ExecuteNonQuery();
                    MyConnection.Close();

                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox1.Text = "";
                    this.productsTableAdapter.Fill(this.shahstoreDataSet.Products);
                }
                else
                {
                    MessageBox.Show("Правильный формат цены: 999.99", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Text = "";
                }
            
        }

            private void button2_Click(object sender, EventArgs e) //добавить
        {
            if (CheckPrice(textBox7.Text))
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "INSERT INTO Products VALUES (@name, @description, @price, @quantity)";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@name", textBox9.Text);
                SqlParameter pr2 = new SqlParameter("@description", textBox8.Text);
                SqlParameter pr3 = new SqlParameter("@price", textBox7.Text);
                SqlParameter pr4 = new SqlParameter("@quantity", textBox6.Text);
                cmd1.Parameters.Add(pr1);
                cmd1.Parameters.Add(pr2);
                cmd1.Parameters.Add(pr3);
                cmd1.Parameters.Add(pr4);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox9.Text = "";
                textBox8.Text = "";
                textBox7.Text = "";
                textBox6.Text = "";
                this.productsTableAdapter.Fill(this.shahstoreDataSet.Products);
            }
            else
            {
                MessageBox.Show("Правильный формат цены: 999.99", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox7.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e) //удалить
        {
            DialogResult result = MessageBox.Show("Вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.No)
            {
                textBox10.Text = "";
                return;
            }
            else
            {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "DELETE FROM Products WHERE ProductId = @productId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@productId", textBox10.Text);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox10.Text = "";
            this.productsTableAdapter.Fill(this.shahstoreDataSet.Products);
            }
        }

        private void button4_Click(object sender, EventArgs e) //найти
        {
            ProductSearch(textBox11.Text);
        }

        public void ProductSearch(string id)
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "SELECT * FROM Products WHERE ProductId = @productId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@productId", id);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!reader.HasRows)
            {
                showSearch = true;
                MessageBox.Show("Товаров с таким Id не найдено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                showSearch = false;
            }
            MyConnection.Close();

            textBox11.Text = "";
        }

        private void button21_Click(object sender, EventArgs e) //обновить
        {
            dataGridView1.DataSource = this.shahstoreDataSet.Products;
            this.productsTableAdapter.Fill(this.shahstoreDataSet.Products);
        }

        //-------------------------Клиенты----------------------------

        private void button8_Click(object sender, EventArgs e) //изменить
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "UPDATE Clients SET FIO = @fio, Number = @number, Email = @email WHERE ClientId = @clientId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@fio", textBox21.Text);
            SqlParameter pr2 = new SqlParameter("@number", textBox20.Text);
            SqlParameter pr3 = new SqlParameter("@email", textBox19.Text);
            SqlParameter pr4 = new SqlParameter("@clientId", textBox22.Text);
            cmd1.Parameters.Add(pr1);
            cmd1.Parameters.Add(pr2);
            cmd1.Parameters.Add(pr3);
            cmd1.Parameters.Add(pr4);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox21.Text = "";
            textBox20.Text = "";
            textBox19.Text = "";
            textBox22.Text = "";
            this.clientsTableAdapter.Fill(this.shahstoreDataSet.Clients);
        }

        private void button7_Click(object sender, EventArgs e) //добавить
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "INSERT INTO Clients VALUES (@fio, @number, @email)";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@fio", textBox17.Text);
            SqlParameter pr2 = new SqlParameter("@number", textBox16.Text);
            SqlParameter pr3 = new SqlParameter("@email", textBox15.Text);
            cmd1.Parameters.Add(pr1);
            cmd1.Parameters.Add(pr2);
            cmd1.Parameters.Add(pr3);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox17.Text = "";
            textBox16.Text = "";
            textBox15.Text = "";
            this.clientsTableAdapter.Fill(this.shahstoreDataSet.Clients);
        }

        private void button6_Click(object sender, EventArgs e) //удалить
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "DELETE FROM Clients WHERE ClientId = @clientId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@clientId", textBox13.Text);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox13.Text = "";
            this.clientsTableAdapter.Fill(this.shahstoreDataSet.Clients);
        }

        private void button5_Click(object sender, EventArgs e) //найти
        {
            DialogResult result = MessageBox.Show("Вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.No)
            {
                textBox12.Text = "";
                return;
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "SELECT * FROM Clients WHERE ClientId = @clientId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@clientId", textBox12.Text);
                cmd1.Parameters.Add(pr1);

                MyConnection.Open();
                SqlDataReader reader = cmd1.ExecuteReader();
                if (!reader.HasRows)
                {
                    MessageBox.Show("Клиентов с таким Id не найдено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView2.DataSource = dt;
                }
                MyConnection.Close();

                textBox12.Text = "";
            }
        }

        private void button22_Click(object sender, EventArgs e) //обновить
        {
            dataGridView2.DataSource = this.shahstoreDataSet.Clients;
            this.clientsTableAdapter.Fill(this.shahstoreDataSet.Clients);
        }

        //-------------------------Заказы----------------------------

        private void button12_Click(object sender, EventArgs e) //изменить
        {
            if (CheckDate(textBox27.Text))
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "UPDATE Orders SET ClientId = @clientId, OrderDate = @orderDate, Status = @status WHERE OrderId = @orderId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@clientId", textBox28.Text);
                SqlParameter pr2 = new SqlParameter("@orderDate", textBox27.Text);
                SqlParameter pr3 = new SqlParameter("@status", textBox26.Text);
                SqlParameter pr4 = new SqlParameter("@orderId", textBox29.Text);
                cmd1.Parameters.Add(pr1);
                cmd1.Parameters.Add(pr2);
                cmd1.Parameters.Add(pr3);
                cmd1.Parameters.Add(pr4);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox28.Text = "";
                textBox27.Text = "";
                textBox26.Text = "";
                textBox29.Text = "";
                this.ordersTableAdapter.Fill(this.shahstoreDataSet.Orders);
            }
            else
            {
                MessageBox.Show("Правильный формат даты: \"yyyy-mm-dd\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e) //добавить
        {
            if (CheckDate(textBox24.Text))
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "INSERT INTO Orders VALUES (@clientId, @orderDate, @status)";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@clientId", textBox25.Text);
                SqlParameter pr2 = new SqlParameter("@orderDate", textBox24.Text);
                SqlParameter pr3 = new SqlParameter("@status", textBox23.Text);
                cmd1.Parameters.Add(pr1);
                cmd1.Parameters.Add(pr2);
                cmd1.Parameters.Add(pr3);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox25.Text = "";
                textBox24.Text = "";
                textBox23.Text = "";
                this.ordersTableAdapter.Fill(this.shahstoreDataSet.Orders);
            }
            else
            {
                MessageBox.Show("Правильный формат даты: \"yyyy-mm-dd\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e) //удалить
        {
            DialogResult result = MessageBox.Show("Вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.No)
            {
                textBox18.Text = "";
                return;
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "DELETE FROM Orders WHERE OrderId = @orderId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@orderId", textBox18.Text);
                cmd1.Parameters.Add(pr1);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox18.Text = "";
                this.ordersTableAdapter.Fill(this.shahstoreDataSet.Orders);
            }
        }

        private void button9_Click(object sender, EventArgs e) //найти
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "SELECT * FROM Orders WHERE OrderId = @orderId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@orderId", textBox14.Text);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!reader.HasRows)
            {
                MessageBox.Show("Заказов с таким Id не найдено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
            }
            MyConnection.Close();

            textBox14.Text = "";
        }

        private void button23_Click(object sender, EventArgs e) //обновить
        {
            dataGridView3.DataSource = this.shahstoreDataSet.Orders;
            this.ordersTableAdapter.Fill(this.shahstoreDataSet.Orders);
        }

        //-------------------------Список Покупок----------------------------

        private void button16_Click(object sender, EventArgs e) //изменить
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "UPDATE PurchaseList SET OrderId = @orderId, ProductId = @productId WHERE PurchaseId = @purchaseId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@purchaseId", textBox38.Text);
            SqlParameter pr2 = new SqlParameter("@orderId", textBox45.Text);
            SqlParameter pr3 = new SqlParameter("@productId", textBox37.Text);
            cmd1.Parameters.Add(pr1);
            cmd1.Parameters.Add(pr2);
            cmd1.Parameters.Add(pr3);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox38.Text = "";
            textBox45.Text = "";
            textBox37.Text = "";
            this.purchaseListTableAdapter.Fill(this.shahstoreDataSet.PurchaseList);
        }

        private void button15_Click(object sender, EventArgs e) //добавить
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "INSERT INTO PurchaseList VALUES (@orderId, @clientId)";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@orderId", textBox34.Text);
            SqlParameter pr2 = new SqlParameter("@clientId", textBox33.Text);
            cmd1.Parameters.Add(pr1);
            cmd1.Parameters.Add(pr2);

            MyConnection.Open();
            cmd1.ExecuteNonQuery();
            MyConnection.Close();

            textBox34.Text = "";
            textBox33.Text = "";
            this.purchaseListTableAdapter.Fill(this.shahstoreDataSet.PurchaseList);
        }

        private void button14_Click(object sender, EventArgs e) //удалить
        {
            DialogResult result = MessageBox.Show("Вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.No)
            {
                textBox31.Text = "";
                return;
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "DELETE FROM PurchaseList WHERE PurchaseId = @purchaseId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@purchaseId", textBox31.Text);
                cmd1.Parameters.Add(pr1);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox31.Text = "";
                this.purchaseListTableAdapter.Fill(this.shahstoreDataSet.PurchaseList);
            }
        }

        private void button13_Click(object sender, EventArgs e) //найти
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "SELECT * FROM PurchaseList WHERE PurchaseId = @purchaseId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@purchaseId", textBox30.Text);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!reader.HasRows)
            {
                MessageBox.Show("Покупок с таким Id не найдено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView4.DataSource = dt;
            }
            MyConnection.Close();

            textBox30.Text = "";
        }

        private void button24_Click(object sender, EventArgs e) //обновить
        {
            dataGridView4.DataSource = this.shahstoreDataSet.PurchaseList;
            this.purchaseListTableAdapter.Fill(this.shahstoreDataSet.PurchaseList);
        }

        //-------------------------Пользователи----------------------------

        private void button20_Click(object sender, EventArgs e) //изменить
        {
            if (textBox41.Text != "admin" || textBox41.Text != "storekeeper" || textBox41.Text != "customer")
            {
                MessageBox.Show("Такой роли нет!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox43.Text = "";
                textBox42.Text = "";
                textBox41.Text = "";
                textBox44.Text = "";
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "UPDATE Users SET Login = @login, Password = @password, Role = @role WHERE UserId = @userId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@login", textBox43.Text);
                SqlParameter pr2 = new SqlParameter("@password", textBox42.Text);
                SqlParameter pr3 = new SqlParameter("@role", textBox41.Text);
                SqlParameter pr4 = new SqlParameter("@userId", textBox44.Text);
                cmd1.Parameters.Add(pr1);
                cmd1.Parameters.Add(pr2);
                cmd1.Parameters.Add(pr3);
                cmd1.Parameters.Add(pr4);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox43.Text = "";
                textBox42.Text = "";
                textBox41.Text = "";
                textBox44.Text = "";
                this.usersTableAdapter.Fill(this.shahstoreDataSet.Users);
            }
        }

        private void button19_Click(object sender, EventArgs e) //добавить
        {
            if (textBox36.Text != "admin" || textBox36.Text != "storekeeper" || textBox36.Text != "customer")
            {
                MessageBox.Show("Такой роли нет!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox40.Text = "";
                textBox39.Text = "";
                textBox36.Text = "";
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "INSERT INTO Users VALUES (@login, @password, @role)";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@login", textBox40.Text);
                SqlParameter pr2 = new SqlParameter("@password", textBox39.Text);
                SqlParameter pr3 = new SqlParameter("@role", textBox36.Text);
                cmd1.Parameters.Add(pr1);
                cmd1.Parameters.Add(pr2);
                cmd1.Parameters.Add(pr3);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox40.Text = "";
                textBox39.Text = "";
                textBox36.Text = "";
                this.usersTableAdapter.Fill(this.shahstoreDataSet.Users);
            }
        }

        private void button18_Click(object sender, EventArgs e) //удалить
        {
            DialogResult result = MessageBox.Show("Вы уверены?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result == DialogResult.No)
            {
                textBox35.Text = "";
                return;
            }
            else
            {
                SqlConnection MyConnection = GetConnection();
                string ComDel = "DELETE FROM Users WHERE UserId = @userId";
                SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
                SqlParameter pr1 = new SqlParameter("@userId", textBox35.Text);
                cmd1.Parameters.Add(pr1);

                MyConnection.Open();
                cmd1.ExecuteNonQuery();
                MyConnection.Close();

                textBox35.Text = "";
                this.usersTableAdapter.Fill(this.shahstoreDataSet.Users);
            }
        }

        private void button17_Click(object sender, EventArgs e) //найти
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "SELECT * FROM Users WHERE UserId = @userId";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@userId", textBox32.Text);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!reader.HasRows)
            {
                MessageBox.Show("Пользователей с таким Id не найдено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView5.DataSource = dt;
            }
            MyConnection.Close();

            textBox32.Text = "";
        }

        private void button25_Click(object sender, EventArgs e) //обновить
        {
            dataGridView5.DataSource = this.shahstoreDataSet.Users;
            this.usersTableAdapter.Fill(this.shahstoreDataSet.Users);
        }

        private void form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Show();
            this.Hide();
        }

        private void textBox_KeyPress_Text(object sender, KeyPressEventArgs e) //только для ввода текста
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'z'))
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                return;
            e.KeyChar = '\0';
        }

        private void textBox_KeyPress_Number(object sender, KeyPressEventArgs e) //только для ввода чисел
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            e.KeyChar = '\0';
        }

        private void textBox_KeyPress_TextNumber(object sender, KeyPressEventArgs e) //для текста и чисел
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'z'))
                return;
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                return;
            e.KeyChar = '\0';
        }

        private void textBox_KeyPress_Date(object sender, KeyPressEventArgs e) //только для даты
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == '-')
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            e.KeyChar = '\0';
        }

        private void textBox_KeyPress_Price(object sender, KeyPressEventArgs e) //только для цены
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == '.')
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            e.KeyChar = '\0';
        }

        public bool CheckDate(string data)
        {
            if (DateTime.TryParse(data, out DateTime date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckPrice(string price)
        {
            int count = 0;
            foreach (char c in price)
            {
                if (c == '.')
                    count++;
            }
            if (count > 1)
                return false;
            else
                return true;
        }
    }
}
