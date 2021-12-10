using System;

namespace BibliotecaApi.Entities
{
    public class User : BaseEntity
    {
        public User(string username, string password,string role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
            Role = role;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLockout { get; set; }
        public DateTime? LockoutDate { get; set; }
    }
}
