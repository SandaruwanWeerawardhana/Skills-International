using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Skills_International_School_Management_System
{
    public partial class RegistrationForm : Form
    {
        private readonly string _connectionString =
            @"Data Source=DESKTOP-T8RBHGC\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";

        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void RegistrationForm_Load(object sender, System.EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void label7_Click(object sender, System.EventArgs e)
        {

        }

        private void label4_Click(object sender, System.EventArgs e)
        {

        }

        private void label10_Click(object sender, System.EventArgs e)
        {

        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void label8_Click(object sender, System.EventArgs e)
        {

        }

        private void DateofBirthlabel9_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void HomePhonelabel11_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            // Register button: collect fields and insert into Registration table
            try
            {
                string regNo = comboBox1?.Text ?? string.Empty;
                string firstName = textBox1?.Text ?? string.Empty;
                string lastName = textBox2?.Text ?? string.Empty;
                DateTime DateofBirth = dateTimePicker1?.Value ?? DateTime.Today;
                string gender = radioButton1 != null && radioButton1.Checked ? radioButton1.Text :
                                radioButton2 != null && radioButton2.Checked ? radioButton2.Text : string.Empty;
                string address = textBox4?.Text ?? string.Empty;
                string mobile = textBox5?.Text ?? string.Empty;
                string email = textBox8?.Text ?? string.Empty;
                string homePhone = textBox6?.Text ?? string.Empty;
                string parentName = textBox3?.Text ?? string.Empty;
                string nic = textBox7?.Text ?? string.Empty;
                string contactNo = textBox9?.Text ?? string.Empty;

                // Basic validation
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("Please enter first name and last name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Registration
                        (FirstName, LastName, DateofBirth, Gender, Address, MobilePhone, Email, HomePhone, ParentName, NIC, ContactNo)
                        VALUES
                        (@FirstName, @LastName, @DateofBirth, @Gender, @Address, @MobilePhone, @Email, @HomePhone, @ParentName, @NIC, @ContactNo)";
                    cmd.Parameters.AddWithValue("@FirstName", (object)firstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", (object)lastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateofBirth", DateofBirth);
                    cmd.Parameters.AddWithValue("@Gender", (object)gender ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobilePhone", (object)mobile ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HomePhone", (object)homePhone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ParentName", (object)parentName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIC", (object)nic ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContactNo", (object)contactNo ?? DBNull.Value);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("Registration successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllTextBoxes(this);
                        PostClearLogic();
                    }
                    else
                    {
                        MessageBox.Show("No rows were inserted. Check database and try again.", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton4_Click(object sender, System.EventArgs e)
        {

        }

        private void Updatebutton1_Click(object sender, System.EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        private void Clearbutton3_Click(object sender, System.EventArgs e)
        {
            // Clear all textboxes and perform any additional reset logic
            ClearAllTextBoxes(this);
            PostClearLogic();
        }

        // Recursively clear textboxes inside the given parent control
        private void ClearAllTextBoxes(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb)
                {
                    tb.Clear();
                }
                else if (c.HasChildren)
                {
                    ClearAllTextBoxes(c);
                }
            }
        }

       
        private void PostClearLogic()
        {
            // Reset selection controls
            try
            {
                if (this.comboBox1 != null)
                    this.comboBox1.SelectedIndex = -1;

                if (this.radioButton1 != null)
                    this.radioButton1.Checked = false;
                if (this.radioButton2 != null)
                    this.radioButton2.Checked = false;

                if (this.dateTimePicker1 != null)
                    this.dateTimePicker1.Value = DateTime.Today;

                
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox tb)
                    {
                        tb.Focus();
                        break;
                    }
                }
            }
            catch
            {
               
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Logout: show the login form and hide this registration form
            try
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f is Login)
                    {
                        f.Show();
                        this.Hide();
                        return;
                    }
                }

                var login = new Login();
                login.Show();
                this.Hide();
            }
            catch
            {
                // If any error occurs, fallback to creating a new login form
                var login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var yesno = MessageBox.Show(
                "Are You Sure, Do You Really Want to Exit...?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (yesno == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
