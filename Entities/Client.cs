using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Client : Person
    {
        private List<Reservation> _reservations;
        private List<Withdraw> _withdraws;
        public Client(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }
        
       
        public IReadOnlyList<Reservation> Reservations => _reservations;
        public IReadOnlyList<Withdraw> Withdraws => _withdraws;
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }

        public decimal Multa { get; set; }
        public Adress Adress { get; set; }
    }
}
