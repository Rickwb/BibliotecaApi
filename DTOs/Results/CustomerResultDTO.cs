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
            Reservations = new List<ReservationResultDTO>();
            if (customer.Reservations is not null || customer.Reservations.Count!=0)
            {
                customer.GetReservationsCustomer().ForEach(r => Reservations.Add(new ReservationResultDTO(r)));
            }
            Withdraws = new List<WithdrawResultDTO>();
            if (customer.Withdraws is not null || customer.Withdraws.Count != 0)
            {
                customer.GetWithdrawsCustomer().ForEach(w => Withdraws.Add(new WithdrawResultDTO(w)));
            }


        }
        public CustomerResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public Adress Adress { get; set; }
        public List<ReservationResultDTO> Reservations { get; set; }
        public List<WithdrawResultDTO> Withdraws { get; set; }

        public List<string> GetErros()=> Errors;
    }
}
