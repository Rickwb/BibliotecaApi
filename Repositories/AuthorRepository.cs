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
            var author = (IEnumerable<Authors>)_repository
                .WhereIfNotNull(name, x => x.Name == name)
                .WhereIfNotNull(nacionality, x => x.Nacionality == nacionality)
                .WhereIfNotNull(age, x => x.Age == age)
                .Skip((page - 1) * items).Take(items);

            return author.ToList();
        }
    }
}
