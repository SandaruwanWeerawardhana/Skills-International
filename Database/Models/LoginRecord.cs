using System;

namespace Skills_International_School_Management_System.Database.Models
{
    /// <summary>Maps a row from the [Logins] table.</summary>
    public class LoginRecord
    {
        public int    Id        { get; set; }
        public string Username  { get; set; }
        public string Password  { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
