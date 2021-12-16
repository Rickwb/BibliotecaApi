using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Authors : Person
    {
        public Authors(Guid id, string name, string nacionality, int age)
        {
            Id = id;
            Name = name;
            Nacionality = nacionality;
            Age = age;
            AuthorBooks ??= new List<Book>();
        }
        public Authors(Guid id, string name, string nacionality, int age, List<Book> books)
        {
            Id = id;
            Name = name;
            Nacionality = nacionality;
            Age = age;
            AuthorBooks = books;
        }

        public string Name { get; private set; }
        public string Nacionality { get; private set; }
        public List<Book> AuthorBooks { get; private set; }




    }
}
