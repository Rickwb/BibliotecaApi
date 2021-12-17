namespace BibliotecaApi.DTOs
{
    public class UserLoginDTO : BaseDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override void Valid()
        {
            IsValid=true;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)) IsValid=false;
        }
    }
}
