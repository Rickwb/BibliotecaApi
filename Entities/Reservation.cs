using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Reservation : BaseEntity
    {
        public Reservation(Customer client, DateTime startDate, DateTime endDate, List<Book> books)
        {
            Id = Guid.NewGuid();

        }

        public Customer Client { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private bool Completed { get; set; }
        public List<Book> Books { get; set; }

        public void CancelarReserva()
        {
            if (DateTime.Today < StartDate.Date)
            {
                Completed = true;

            }

            Completed= false;
        }


    }
}
