using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        

        public List<Customer> GetAllCustomersWithParams(string? Name, string? document, DateTime? Birthdate, int page, int items = 50)
        {
            var clients = (IEnumerable<Customer>)_repository;
            if (String.IsNullOrEmpty(Name))
                clients = clients.WhereIfNotNull(Name, x => x.Name == Name);
            if (String.IsNullOrEmpty(document))
                clients = clients.WhereIfNotNull(document,x => x.Document == document);
            if (document is not null)
                clients = clients.WhereIfNotNull(Birthdate,x => x.BirthDate == Birthdate);
            if (page != 0 && items != 0)
                clients = clients.Skip((page - 1) * items).Take(items);

            return clients.ToList();
        }

    }


    public static class AAAA
    {
        public static IEnumerable<TSource> WhereIfNotNull<TSource>(this IEnumerable<TSource> source, object param, Func<TSource, bool> predicate)
        {
            if(param is null)
                return source;

            if(param.GetType() == typeof(string) && !string.IsNullOrEmpty(param as string))
                return source.Where(predicate);

            return source.Where(predicate);
        }
    }
}
