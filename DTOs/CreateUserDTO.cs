namespace BibliotecaApi.DTOs
{
    public class CreateUserDTO:BaseDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public override void Valid()
        {
            IsValid = true;

            if (string.IsNullOrEmpty(Username))IsValid = false;
        }
    }
}
