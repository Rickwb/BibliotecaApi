namespace BibliotecaApi.DTOs
{
    public class CreateClientDTO : BaseDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public override void Valid()
        {
            throw new System.NotImplementedException();
        }
    }
}
