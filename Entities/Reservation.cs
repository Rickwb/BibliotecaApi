using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Reservation : BaseEntity
    {
        public Reservation(Customer client)
        {
            Id = Guid.NewGuid();
        }
        public Customer Client { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Completed { get; set; }
        public List<Book> Books { get; set; }

        
    }
}
