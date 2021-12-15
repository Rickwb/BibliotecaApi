using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class BookResultDTO:CreateResultDTO<Book>
    {
        public BookResultDTO(Book book)
        {

        }
        public BookResultDTO(CreationException exception)
        {

        }
        public Guid IdAuthor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RealeaseYear { get; set; }
        public int NumCopies { get; set; }

        public List<string> GetErrors() => Errors;
    }
}
