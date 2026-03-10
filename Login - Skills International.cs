using System;
using System.Windows.Forms;
using Skills_International_School_Management_System.Database;

namespace Skills_International_School_Management_System
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
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

        private void clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); 
            textBox2.Clear(); 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Please enter both Username and Password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (DbHelper.ValidateLogin(username, password))
                {
                    this.Hide();
                    RegistrationForm rf = new RegistrationForm();
                    rf.Show();
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else
                {
                    MessageBox.Show(
                        "Invalid Login Credentials, Please Check User Name and Password then Try Again.",
                        "Invalid Login Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
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
