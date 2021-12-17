using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.DTOs
{
    public class CreateAuthorDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }
        public List<Guid> AuthorBooks { get; set; }
        public override void Valid()
        {
            IsValid = true;

            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty("Nacionality"))
                IsValid = false;
            if(Age <=0)
                IsValid = false;

        }
    }
}
