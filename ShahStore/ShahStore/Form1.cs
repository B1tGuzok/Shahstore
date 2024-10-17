using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ShahStore
{
    public partial class Form1 : Form
    {
        Form2 form2;
        public string role;

        public Form1()
        {
            InitializeComponent();
        }

        public SqlConnection GetConnection()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=LAPTOPPRO15;Initial Catalog=Shahstore;Integrated Security=True;Encrypt=False");
            return sqlcon;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'z'))
                return;
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == (char)Keys.Back)
                return;
            e.KeyChar = '\0';
        }

        public void button1_Click(object sender, EventArgs e)
        {
            SignIn(textBox1.Text, textBox2.Text);
        }

        public void SignIn(string login, string password)
        {
            SqlConnection MyConnection = GetConnection();
            string ComDel = "SELECT * FROM Users WHERE Login = @login";
            SqlCommand cmd1 = new SqlCommand(ComDel, MyConnection);
            SqlParameter pr1 = new SqlParameter("@login", login);
            cmd1.Parameters.Add(pr1);

            MyConnection.Open();
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!reader.HasRows)
            {
                label5.Text = "Wrong Login!";
            }
            else
            {
                reader.Read();
                if (password == reader.GetString(2))
                {
                    role = reader.GetString(3);
                    form2 = new Form2(role, this);
                    this.Hide();
                    form2.Show();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    label5.Text = "";
                }
                else
                {
                    label5.Text = "Wrong Password!";
                }
            }
            MyConnection.Close();
        }
    }
}
