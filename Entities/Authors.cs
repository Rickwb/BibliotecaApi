using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Authors : Person
    {
        public Authors(string name)
        {
            Id = Guid.NewGuid();
        }

        public string Name { get; set; }
        public string Nacionality { get; set; }
        public List<Book> AuthorBooks { get; set; }




    }
}
