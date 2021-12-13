using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class EmployeeRepository:BaseRepository<Employee>
    {
        public List<Employee> GetAllEmployeesWithParams(string? name, string? document, DateTime? Birthdate, int page, int items)
        {
            var employee = (IEnumerable<Employee>)_repository;
            if (name is not null)
                employee = employee.WhereIfNotNull(name, x => x.Name == name);
            if (document is not null)
                employee = employee.WhereIfNotNull(document,x => x.Document == document);
            if (document is not null)
                employee = employee.WhereIfNotNull(Birthdate,x => x.BirthDate == Birthdate);
            if (page != 0 && items != 0)
                employee = employee.Skip((page - 1) * items).Take(items);

            return employee.ToList();
        }

    }
   
}
