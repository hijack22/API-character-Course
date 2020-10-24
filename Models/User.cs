using System.Collections.Generic;

namespace API_Course.Models
{
    public class User
    {
        public int id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public List<Character> Characters { get; set; }
    }
}