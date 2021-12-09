using System;

namespace BibliotecaApi.Entities
{
    public class Book:BaseEntity
    {
        public Book(Authors author,string title,int numCopies)
        {
            Id= Guid.NewGuid();
            Author = author;
            Title = title;
            NumCopies = numCopies;

        }

        public Authors Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RealeaseYear { get; set; }
        public int NumCopies { get; set; }




    }
}
