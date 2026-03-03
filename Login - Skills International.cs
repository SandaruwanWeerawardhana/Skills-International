using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Skills_International_School_Management_System
{
    public partial class Login : Form
    {
    
        SqlConnection DB_Con = new SqlConnection(
            @"Data Source=YOUR_PC_NAME\SQLEXPRESS;" +
            "Initial Catalog=Student;" +
            "Integrated Security=True");

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //txtun.Focus();
        }



        private void btnclear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();  // Username
            textBox2.Clear();  // Password
            textBox1.Focus();
        }

      
        private void btnexit_Click(object sender, EventArgs e)
        {
            DialogResult yesno = MessageBox.Show(
                "Are You Sure, Do You Really Want to Exit...?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (yesno == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (yesno == DialogResult.No)
            {
              
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

   
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult yesno = MessageBox.Show(
                "Are You Sure, Do You Really Want to Exit...?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (yesno == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (yesno == DialogResult.No)
            {
              
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); 
            textBox2.Clear(); 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //string query = "SELECT * FROM Logins WHERE username=@user AND password=@pw";
                //SqlDataAdapter sda = new SqlDataAdapter(query, DB_Con);
                //sda.SelectCommand.Parameters.AddWithValue("@user", txtun.Text);
                //sda.SelectCommand.Parameters.AddWithValue("@pw", txtpw.Text);

                //DataTable dt = new DataTable();
                //sda.Fill(dt);

                //if (dt.Rows.Count == 1)
                //{
                // Login success - hide this form and open Registration
                this.Hide();
                RegistrationForm rf = new RegistrationForm();
                rf.Show();
                //}
                //else
                //{
                //    // Login failed - show error message
                //    MessageBox.Show(
                //        "Invalid Login Credentials, Please Check User Name and Password then Try Again",
                //        "Invalid Login Details",
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection error: " + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
