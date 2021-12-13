using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Repositories
{
    public class AuthorRepository : BaseRepository<Authors>
    {

        public List<Authors> GetAllAuthorsWithParams(string? name, string? nacionality, int? age, int page, int items)
        {
            var author = (IEnumerable<Authors>)_repository;
            if (name is not null)
                author = author.WhereIfNotNull(name, x => x.Name == name);
            if (nacionality is not null)
                author = author.WhereIfNotNull(nacionality, x => x.Nacionality == nacionality);
            if (age != 0)
                author = author.WhereIfNotNull(age, x => x.Age == age);
            if (page != 0 && items != 0)
                author = author.Skip((page - 1) * items).Take(items);

            return author.ToList();
        }

    }
}
