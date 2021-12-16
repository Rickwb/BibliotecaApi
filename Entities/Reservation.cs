using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Reservation : BaseEntity
    {
        public Reservation(Guid id, Customer client, DateTime startDate, DateTime endDate, List<Book> books)
        {
            Id = id;
            Client = client;
            StartDate = startDate;
            EndDate = endDate;
            Books = books;
        }

        public Customer Client { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        private bool Completed { get; set; }
        private bool Cancelada { get; set; }
        public List<Book> Books { get; private set; }

        public void CancelarReserva()
        {
            if (DateTime.Today < StartDate.Date)
            {
                Cancelada = true;

            }
            Cancelada = false;
        }

        public void FinalizarReserva()
        {
            Completed = true;
        }
    }
}
