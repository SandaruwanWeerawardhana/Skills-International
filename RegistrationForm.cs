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
            LoadRegNos();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
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
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(nic) || string.IsNullOrWhiteSpace(contactNo))
                {
                    MessageBox.Show("Please Fill form.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Registration
                        (firstName, lastName, dateOfBirth, gender, address, mobilePhone, email, homePhone, parentName, nic, contactNo)
                        VALUES
                        (@firstName, @lastName, @dateOfBirth, @gender, @address, @mobilePhone, @email, @homePhone, @parentName, @nic, @contactNo)";
                    cmd.Parameters.AddWithValue("@firstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                    cmd.Parameters.AddWithValue("@lastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", DateofBirth);
                    cmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
                    cmd.Parameters.AddWithValue("@mobilePhone", int.TryParse(mobile, out int mob) ? (object)mob : DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                    cmd.Parameters.AddWithValue("@homePhone", int.TryParse(homePhone, out int hp) ? (object)hp : DBNull.Value);
                    cmd.Parameters.AddWithValue("@parentName", string.IsNullOrEmpty(parentName) ? (object)DBNull.Value : parentName);
                    cmd.Parameters.AddWithValue("@nic", string.IsNullOrEmpty(nic) ? (object)DBNull.Value : nic);
                    cmd.Parameters.AddWithValue("@contactNo", int.TryParse(contactNo, out int cn) ? (object)cn : DBNull.Value);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("Record Added successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllTextBoxes(this);
                        PostClearLogic();
                        LoadRegNos();

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
            
                string key = comboBox1?.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(key))
                {
                    MessageBox.Show("Please select a Reg No to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(key, out int regId))
                {
                    MessageBox.Show("InComplete or Invalide data.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var yesno = MessageBox.Show($"Are you sure you want to delete record Reg No: {regId}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (yesno != DialogResult.Yes) return;

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Registration WHERE regNo = @regNo";
                    cmd.Parameters.AddWithValue("@regNo", regId);

                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (affected > 0)
                    {
                        MessageBox.Show("Record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllTextBoxes(this);
                        PostClearLogic();
                        LoadRegNos();
                    }
                    else
                    {
                        MessageBox.Show("No record found to delete.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
          
        }

        private void Updatebutton1_Click(object sender, System.EventArgs e)
        {
            try
            {
                string key = comboBox1?.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(key))
                {
                    MessageBox.Show("Please select a Reg No to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string firstName = textBox1?.Text ?? string.Empty;
                string lastName = textBox2?.Text ?? string.Empty;
                DateTime dateOfBirth = dateTimePicker1?.Value ?? DateTime.Today;
                string gender = radioButton1 != null && radioButton1.Checked ? radioButton1.Text :
                                radioButton2 != null && radioButton2.Checked ? radioButton2.Text : string.Empty;
                string address = textBox4?.Text ?? string.Empty;
                string mobile = textBox5?.Text ?? string.Empty;
                string email = textBox8?.Text ?? string.Empty;
                string homePhone = textBox6?.Text ?? string.Empty;
                string parentName = textBox3?.Text ?? string.Empty;
                string nic = textBox7?.Text ?? string.Empty;
                string contactNo = textBox9?.Text ?? string.Empty;

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"UPDATE Registration SET
                                firstName=@firstName, lastName=@lastName, dateOfBirth=@dateOfBirth, gender=@gender,
                                address=@address, mobilePhone=@mobilePhone, email=@email, homePhone=@homePhone,
                                parentName=@parentName, nic=@nic, contactNo=@contactNo
                                WHERE regNo=@regNo";
                        cmd.Parameters.AddWithValue("@regNo", key);
                        cmd.Parameters.AddWithValue("@firstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                        cmd.Parameters.AddWithValue("@lastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                        cmd.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
                        cmd.Parameters.AddWithValue("@mobilePhone", int.TryParse(mobile, out int mob) ? (object)mob : DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                        cmd.Parameters.AddWithValue("@homePhone", int.TryParse(homePhone, out int hp) ? (object)hp : DBNull.Value);
                        cmd.Parameters.AddWithValue("@parentName", string.IsNullOrEmpty(parentName) ? (object)DBNull.Value : parentName);
                        cmd.Parameters.AddWithValue("@nic", string.IsNullOrEmpty(nic) ? (object)DBNull.Value : nic);
                        cmd.Parameters.AddWithValue("@contactNo", int.TryParse(contactNo, out int cn) ? (object)cn : DBNull.Value);

                    int affected = cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Update successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRegNos();
                    ClearAllTextBoxes(this);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clearbutton3_Click(object sender, System.EventArgs e)
        {
            ClearAllTextBoxes(this);
            PostClearLogic();
        }

        // Recursively clear textboxes inside the given parent control
        private void ClearAllTextBoxes(Control parent)
        {
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = string.Empty;
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Today;
            radioButton2.Checked = false;
            radioButton1.Checked = false;
            textBox4.Clear();
            textBox8.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox3.Clear();
            textBox7.Clear();
            textBox9.Clear();
            textBox1.Focus();
        }

       
        private void PostClearLogic()
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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selected))
                LoadRegistrationByRegNo(selected);
        }

        private void LoadRegNos()
        {
          
                comboBox1.Items.Clear();
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT regNo FROM Registration ORDER BY regNo";
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                                comboBox1.Items.Add(rdr.GetValue(0).ToString());
                        }
                    }
                }
           
        }

        private void LoadRegistrationByRegNo(string id)
        {
            if (!int.TryParse(id, out int regId)) return;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Registration WHERE regNo = @regNo";
                    cmd.Parameters.AddWithValue("@regNo", regId);
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                            FillFormFromReader(rdr);
                        else
                            MessageBox.Show("No record found for Reg No: " + id, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillFormFromReader(SqlDataReader rdr)
        {
            // Helper: safely read a string column by name
            string GetStr(string col)
            {
                try
                {
                    int i = rdr.GetOrdinal(col);
                    return rdr.IsDBNull(i) ? string.Empty : rdr.GetValue(i).ToString();
                }
                catch { return string.Empty; }
            }

            textBox1.Text  = GetStr("firstName");
            textBox2.Text  = GetStr("lastName");
            textBox4.Text  = GetStr("address");
            textBox5.Text  = GetStr("mobilePhone");
            textBox6.Text  = GetStr("homePhone");
            textBox7.Text  = GetStr("nic");
            textBox8.Text  = GetStr("email");
            textBox3.Text  = GetStr("parentName");
            textBox9.Text  = GetStr("contactNo");

            int di = rdr.GetOrdinal("dateOfBirth");
            if (!rdr.IsDBNull(di))
                dateTimePicker1.Value = rdr.GetDateTime(di);

            string gender = GetStr("gender");
            if (radioButton1 != null)
                radioButton1.Checked = radioButton1.Text.Equals(gender, StringComparison.OrdinalIgnoreCase);
            if (radioButton2 != null)
                radioButton2.Checked = radioButton2.Text.Equals(gender, StringComparison.OrdinalIgnoreCase);
        }
    }
}
