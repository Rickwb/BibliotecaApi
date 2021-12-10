using System;

namespace BibliotecaApi.Entities
{
    public class Employee:Person
    {
        public Employee(string name, string document, string cep,string role,Guid userId)
        {
            Name = name;
            Document = document;
            Cep = cep;
            Role = role;
            UserId = userId;
        }

        public Employee(string name, string document, string cep, string role)
        {
            Name = name;
            Document = document;
            Cep = cep;
            Role = role;
        }
        public string Role { get; set; }
    }
}
