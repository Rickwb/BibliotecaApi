using Domain.Enities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class BookResultDTO:CreateResultDTO<Book>
    {
        public BookResultDTO(Book book)
        {
            Id = book.Id;
            IdAuthor = book.Author.Id;
            Title = book.Title;
            Description = book.Description;
            RealeaseYear = book.RealeaseYear.ToString().PadLeft(4,'0');
            NumCopies = book.NumCopies;
        }
        public BookResultDTO(CreationException exception)
        {
            Errors= new List<string> { exception.Message };
        }
        public Guid Id { get; set; }
        public Guid IdAuthor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RealeaseYear { get; set; }
        public int NumCopies { get; set; }

        public List<string> GetErrors() => Errors;
    }
}
