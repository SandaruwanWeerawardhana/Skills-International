using System;
using System.Data.SqlClient;

namespace Skills_International_School_Management_System.Database.Models
{
    /// <summary>Maps a row from the [Registration] table.</summary>
    public class StudentRecord
    {
        public int      RegNo       { get; set; }
        public string   FirstName   { get; set; }
        public string   LastName    { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string   Gender      { get; set; }
        public string   Address     { get; set; }
        public string   MobilePhone { get; set; }
        public string   Email       { get; set; }
        public string   HomePhone   { get; set; }
        public string   ParentName  { get; set; }
        public string   Nic         { get; set; }
        public string   ContactNo   { get; set; }


        internal static StudentRecord FromReader(SqlDataReader rdr)
        {
            string Get(string col)
            {
                int i = rdr.GetOrdinal(col);
                return rdr.IsDBNull(i) ? string.Empty : rdr.GetValue(i).ToString();
            }

            var r = new StudentRecord
            {
                RegNo       = (int)rdr["regNo"],
                FirstName   = Get("firstName"),
                LastName    = Get("lastName"),
                Gender      = Get("gender"),
                Address     = Get("address"),
                MobilePhone = Get("mobilePhone"),
                Email       = Get("email"),
                HomePhone   = Get("homePhone"),
                ParentName  = Get("parentName"),
                Nic         = Get("nic"),
                ContactNo   = Get("contactNo"),
            };

            int dobOrd = rdr.GetOrdinal("dateOfBirth");
            if (!rdr.IsDBNull(dobOrd))
                r.DateOfBirth = rdr.GetDateTime(dobOrd);
            return r;
        }
    }
}
