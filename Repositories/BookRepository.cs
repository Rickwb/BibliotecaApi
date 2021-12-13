using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Repositories
{
    public class BookRepository:BaseRepository<Book>
    {

        public List<Book> GetAllBooksWithParams(Book author, string? name, int? releaseYear, int page, int items)
        {
            var books = (IEnumerable<Book>)_repository;
            if (name is not null)
                books = books.WhereIfNotNull(name, x => x.Title == name);
            if (releaseYear is not null)
                books = books.WhereIfNotNull(releaseYear, x => x.RealeaseYear == releaseYear);
            if (author is not null)
                books = books.WhereIfNotNull(author, x => x.Author.Equals(author));
            if (page != 0 && items != 0)
                books = books.Skip((page - 1) * items).Take(items);

            return books.ToList();
        }
    }
}
