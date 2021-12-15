using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class EmployeeResultDTO:CreateResultDTO<Employee>
    {
        public EmployeeResultDTO(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            Document = employee.Document;
            Role = employee.Role;
        }
        public EmployeeResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Role { get; set; }

        public List<string> GetErrors()=> Errors;
    }
}
