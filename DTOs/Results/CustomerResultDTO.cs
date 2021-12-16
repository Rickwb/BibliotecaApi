using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class CustomerResultDTO : CreateResultDTO<Customer>
    {
        public CustomerResultDTO(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Document = customer.Document;
            Adress = customer.Adress;
        }
        public CustomerResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public Adress Adress { get; set; }

        public List<string> GetErros()=> Errors;
    }
}
