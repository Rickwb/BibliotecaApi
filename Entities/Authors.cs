﻿using System;
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

        public string Name { get; set; }
        public string Nacionality { get; set; }
        public List<Book> AuthorBooks { get; set; }




    }
}
