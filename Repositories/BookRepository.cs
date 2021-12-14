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
            var books = (IEnumerable<Book>)_repository
              .WhereIfNotNull(name, x => x.Title == name)
              .WhereIfNotNull(releaseYear, x => x.RealeaseYear == releaseYear)
              .WhereIfNotNull(author, x => x.Author.Equals(author))
              .Skip((page - 1) * items).Take(items);

            return books.ToList();
        }
    }
}
