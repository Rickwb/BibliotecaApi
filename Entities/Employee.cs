using System;

namespace BibliotecaApi.Entities
{
    public class Employee:Person
    {
        public Employee(Guid id,string name, string document, string cep,string role,Guid userId)
        {
            Id = id;
            Name = name;
            Document = document;
            Cep = cep;
            Role = role;
            UserId = userId;
        }
        public Employee(Guid id,string name, string document, string cep, string role)
        {
            Id=id;
            Name = name;
            Document = document;
            Cep = cep;
            Role = role;
        }
        public string Role { get; set; }
    }
}
