using SQLite;
using System;
using System.IO;

namespace LoginApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id { get; set; }
        [MaxLength(25)]
        public string username { get; set; }
        [MaxLength(12)]
        public string password { get; set; }
        [MaxLength(128)]
        public string info { get; set; }
    }
}