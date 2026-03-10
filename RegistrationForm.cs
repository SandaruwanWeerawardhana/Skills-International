using System;
using System.Windows.Forms;
using Skills_International_School_Management_System.Database;
using Skills_International_School_Management_System.Database.Models;

namespace Skills_International_School_Management_System
{
    public partial class RegistrationForm : Form
    {

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
                var s = BuildStudentRecordFromForm();
                if (s == null) return;

                if (DbHelper.InsertStudent(s))
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

            if (DbHelper.DeleteStudent(regId))
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

                if (!int.TryParse(key, out int regId))
                {
                    MessageBox.Show("InComplete or Invalide data.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var s = BuildStudentRecordFromForm();
                if (s == null) return;

                s.RegNo = regId;
                DbHelper.UpdateStudent(s);
                MessageBox.Show("Record Update successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegNos();
                ClearAllTextBoxes(this);
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
            foreach (var regNo in DbHelper.GetAllRegNos())
                comboBox1.Items.Add(regNo);
        }

        private void LoadRegistrationByRegNo(string id)
        {
            if (!int.TryParse(id, out int regId)) return;
            try
            {
                var s = DbHelper.GetByRegNo(regId);
                if (s != null)
                    FillFormFromRecord(s);
                else
                    MessageBox.Show("No record found for Reg No: " + id, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private StudentRecord BuildStudentRecordFromForm()
        {
            string firstName  = textBox1?.Text ?? string.Empty;
            string lastName   = textBox2?.Text ?? string.Empty;
            string gender     = radioButton1 != null && radioButton1.Checked ? radioButton1.Text :
                                radioButton2 != null && radioButton2.Checked ? radioButton2.Text : string.Empty;
            string address    = textBox4?.Text ?? string.Empty;
            string mobile     = textBox5?.Text ?? string.Empty;
            string email      = textBox8?.Text ?? string.Empty;
            string homePhone  = textBox6?.Text ?? string.Empty;
            string parentName = textBox3?.Text ?? string.Empty;
            string nic        = textBox7?.Text ?? string.Empty;
            string contactNo  = textBox9?.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(address)   || string.IsNullOrWhiteSpace(email)    ||
                string.IsNullOrWhiteSpace(mobile)    || string.IsNullOrWhiteSpace(nic)      ||
                string.IsNullOrWhiteSpace(contactNo))
            {
                MessageBox.Show("Please Fill form.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new StudentRecord
            {
                FirstName   = firstName,
                LastName    = lastName,
                DateOfBirth = dateTimePicker1?.Value ?? DateTime.Today,
                Gender      = gender,
                Address     = address,
                MobilePhone = mobile,
                Email       = email,
                HomePhone   = homePhone,
                ParentName  = parentName,
                Nic         = nic,
                ContactNo   = contactNo
            };
        }

        private void FillFormFromRecord(StudentRecord s)
        {
            textBox1.Text = s.FirstName;
            textBox2.Text = s.LastName;
            textBox4.Text = s.Address;
            textBox5.Text = s.MobilePhone;
            textBox6.Text = s.HomePhone;
            textBox7.Text = s.Nic;
            textBox8.Text = s.Email;
            textBox3.Text = s.ParentName;
            textBox9.Text = s.ContactNo;

            if (s.DateOfBirth.HasValue)
                dateTimePicker1.Value = s.DateOfBirth.Value;

            if (radioButton1 != null)
                radioButton1.Checked = radioButton1.Text.Equals(s.Gender, StringComparison.OrdinalIgnoreCase);
            if (radioButton2 != null)
                radioButton2.Checked = radioButton2.Text.Equals(s.Gender, StringComparison.OrdinalIgnoreCase);
        }
    }
}
