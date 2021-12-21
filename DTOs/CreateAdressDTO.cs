using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.DTOs
{
    public class CreateAdressDTO : BaseDTO
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Ibge { get; set; }
        public string Gia { get; set; }
        public string Ddd { get; set; }
        public string Siafi { get; set; }

        public override void Valid()
        {
            if (Cep.Length != 8) IsValid = false;
            if (Uf.Length != 2 || String.IsNullOrWhiteSpace(Uf)) IsValid = false;
            if (Logradouro.Length > 100 || String.IsNullOrWhiteSpace(Logradouro)) IsValid = false;
            if (String.IsNullOrWhiteSpace(Bairro)) IsValid = false;

        }
    }
}
