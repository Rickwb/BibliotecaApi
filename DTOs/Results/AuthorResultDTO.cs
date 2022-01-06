using Domain.Enities;
using Newtonsoft.Json;
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
                AuthorBooks ??= new List<BookResultDTO>();
            if (author.AuthorBooks != null)
                author.AuthorBooks.ForEach(x=>AuthorBooks.Add(new BookResultDTO(x)));

        }
        public AuthorResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }
        public List<BookResultDTO> AuthorBooks { get; private set; }

        public List<string> GetErrors() => Errors;
    }
}
