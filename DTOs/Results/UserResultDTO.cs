using Domain.Enities;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class UserResultDTO:CreateResultDTO<User>
    {
        public UserResultDTO(User user)
        {
            Username = user.Username;
            Password = user.Password;
            Role = user.Role;
        }
        public UserResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public List<string> GetErrors() => Errors;
    }
}
