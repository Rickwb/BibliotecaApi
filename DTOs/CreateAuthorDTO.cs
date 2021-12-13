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
        public List<Book> AuthorBooks { get; set; }
        public override void Valid()
        {
            IsValid = true;
        }
    }
}
