using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class AuthorResultDTO : CreateResultDTO<Authors>
    {
        public AuthorResultDTO(Authors author)
        {
            Id = author.Id;
            Name = author.Name;
            Nacionality = author.Nacionality;
            Age = author.Age;
        }
        public AuthorResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }

        public List<string> GetErrors() => Errors;
    }
}
