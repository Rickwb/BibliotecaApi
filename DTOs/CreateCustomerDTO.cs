namespace BibliotecaApi.DTOs
{
    public class CreateCustomerDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Cep { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public override void Valid()
        {
            IsValid = true;
        }
    }
}
