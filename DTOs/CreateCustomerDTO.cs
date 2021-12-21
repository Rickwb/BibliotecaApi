using System;

namespace BibliotecaApi.DTOs
{
    public class CreateCustomerDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Cep { get; set; }
        public DateTime Birtdate { get; set; }
        public CreateAdressDTO Adress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public override void Valid()
        {
            IsValid = true;

            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Document)) IsValid = false;

            if ((String.IsNullOrEmpty(Cep) && String.IsNullOrEmpty(Adress.Cep)) || (Cep.Length != 8 && Adress.Cep.Length != 8)) IsValid = false;

            if (Role.ToLower() == "admin" || Role.ToLower() == "employee") IsValid = false;

            if (Birtdate.Year > 2010) IsValid = false;


        }
    }
}
