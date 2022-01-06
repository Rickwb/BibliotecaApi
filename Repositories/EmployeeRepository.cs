using Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class EmployeeRepository:BaseRepository<Employee>
    {
        public List<Employee> GetAllEmployeesWithParams(string? name, string? document, DateTime? Birthdate, int page, int items)
        {
            var employee = (IEnumerable<Employee>)_repository
               .WhereIfNotNull(name, x => x.Name == name)
               .WhereIfNotNull(document,x => x.Document == document)
               .WhereIfNotNull(Birthdate,x => x.BirthDate.ToString("dd/MM/yyyy") == Birthdate?.ToString("dd/MM/yyyy"))
               .Skip((page - 1) * items).Take(items);

            return employee.ToList();
        }
    }
}
