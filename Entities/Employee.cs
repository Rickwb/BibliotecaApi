using System;

namespace BibliotecaApi.Entities
{
    public class Employee:Person
    {
        public Employee(Guid id,string name, string document,int age, string cep,string role,Guid userId)
        {
            Id = id;
            Name = name;
            Document = document;
            Age = age;
            Cep = cep;
            Role = role;
            UserId = userId;
        }
        public Employee(Guid id,string name, string document,int age, string cep, string role)
        {
            Id=id;
            Name = name;
            Document = document;
            Age = age;
            Cep = cep;
            Role = role;
        }
        public string Role { get; private set; }
    }
}
