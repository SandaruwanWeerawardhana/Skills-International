using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Skills_International_School_Management_System.Database.Models;

namespace Skills_International_School_Management_System.Database
{
    /// <summary>
    /// Centralised data-access layer (ADO.NET).
    /// All SQL for the SkillInternationalDB database lives here.
    /// </summary>
    internal static class DbHelper
    {
        // Allows test projects (which compile this file as a file-link) to
        // override the connection string without an App.config.
        private static string _connectionStringOverride;

        internal static string ConnectionString
        {
            get => _connectionStringOverride
                ?? ReadConnectionStringFromConfig("Student")
                ?? throw new InvalidOperationException(
                    "Connection string 'SkillInternationalDB' was not found in App.config.");
            set => _connectionStringOverride = value;
        }

        private static string ReadConnectionStringFromConfig(string name)
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string configPath = exePath + ".config";
            if (!File.Exists(configPath))
                return null;

            var doc = XDocument.Load(configPath);
            return doc.Descendants("connectionStrings")
                      .Descendants("add")
                      .Where(e => (string)e.Attribute("name") == name)
                      .Select(e => (string)e.Attribute("connectionString"))
                      .FirstOrDefault();
        }

        // ── Authentication ─────────────────────────────────────────────────────

        /// <summary>Returns true when username + password match a row in [Logins].</summary>
        internal static bool ValidateLogin(string username, string password)
        {
            const string sql =
                "SELECT COUNT(1) FROM [Logins] WHERE [username] = @u AND [password] = @p";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 100).Value = username;
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 256).Value = password;
                conn.Open();
                return (int)cmd.ExecuteScalar() == 1;
            }
        }

        // ── Registration CRUD ──────────────────────────────────────────────────

        /// <summary>Returns all regNo values ordered ascending.</summary>
        internal static List<string> GetAllRegNos()
        {
            var list = new List<string>();
            const string sql = "SELECT [regNo] FROM [Registration] ORDER BY [regNo]";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                        list.Add(rdr.GetValue(0).ToString());
            }

            return list;
        }

        /// <summary>Returns the student with the given regNo, or null if not found.</summary>
        internal static StudentRecord GetByRegNo(int regNo)
        {
            const string sql =
                "SELECT * FROM [Registration] WHERE [regNo] = @regNo";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@regNo", SqlDbType.Int).Value = regNo;
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                    return rdr.Read() ? StudentRecord.FromReader(rdr) : null;
            }
        }

        /// <summary>Inserts a new student record. Returns true on success.</summary>
        internal static bool InsertStudent(StudentRecord s)
        {
            const string sql = @"
                INSERT INTO [Registration]
                    ([firstName],[lastName],[dateOfBirth],[gender],[address],
                     [mobilePhone],[email],[homePhone],[parentName],[nic],[contactNo])
                VALUES
                    (@fn,@ln,@dob,@gender,@addr,@mob,@email,@hp,@parent,@nic,@cn)";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                AddStudentParams(cmd, s);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>Updates an existing student record. Returns true on success.</summary>
        internal static bool UpdateStudent(StudentRecord s)
        {
            const string sql = @"
                UPDATE [Registration] SET
                    [firstName]   = @fn,
                    [lastName]    = @ln,
                    [dateOfBirth] = @dob,
                    [gender]      = @gender,
                    [address]     = @addr,
                    [mobilePhone] = @mob,
                    [email]       = @email,
                    [homePhone]   = @hp,
                    [parentName]  = @parent,
                    [nic]         = @nic,
                    [contactNo]   = @cn
                WHERE [regNo] = @regNo";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                AddStudentParams(cmd, s);
                cmd.Parameters.Add("@regNo", SqlDbType.Int).Value = s.RegNo;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>Deletes the student with the given regNo. Returns true on success.</summary>
        internal static bool DeleteStudent(int regNo)
        {
            const string sql =
                "DELETE FROM [Registration] WHERE [regNo] = @regNo";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd  = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@regNo", SqlDbType.Int).Value = regNo;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private static void AddStudentParams(SqlCommand cmd, StudentRecord s)
        {
            cmd.Parameters.Add("@fn",     SqlDbType.NVarChar, 100).Value = Nv(s.FirstName);
            cmd.Parameters.Add("@ln",     SqlDbType.NVarChar, 100).Value = Nv(s.LastName);
            cmd.Parameters.Add("@dob",    SqlDbType.Date      ).Value    = s.DateOfBirth.HasValue
                                                                               ? (object)s.DateOfBirth.Value
                                                                               : DBNull.Value;
            cmd.Parameters.Add("@gender", SqlDbType.NVarChar,  10).Value = Nv(s.Gender);
            cmd.Parameters.Add("@addr",   SqlDbType.NVarChar, 255).Value = Nv(s.Address);
            cmd.Parameters.Add("@mob",    SqlDbType.NVarChar,  20).Value = Nv(s.MobilePhone);
            cmd.Parameters.Add("@email",  SqlDbType.NVarChar, 100).Value = Nv(s.Email);
            cmd.Parameters.Add("@hp",     SqlDbType.NVarChar,  20).Value = Nv(s.HomePhone);
            cmd.Parameters.Add("@parent", SqlDbType.NVarChar, 100).Value = Nv(s.ParentName);
            cmd.Parameters.Add("@nic",    SqlDbType.NVarChar,  20).Value = Nv(s.Nic);
            cmd.Parameters.Add("@cn",     SqlDbType.NVarChar,  20).Value = Nv(s.ContactNo);
        }

        private static object Nv(string s) =>
            string.IsNullOrEmpty(s) ? (object)DBNull.Value : s;
    }
}
