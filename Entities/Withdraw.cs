using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Withdraw : BaseEntity
    {
        public Withdraw(Customer customer, Reservation reservation)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            Books = reservation.Books;
            Reservation = reservation;
            WithdrawDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(5);
            IsOpen = true;

        }

        public Withdraw(Customer customer, List<Book> books)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            Books = books;
            WithdrawDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(5);
            IsOpen = true;
        }

        public Customer Customer { get; private set; }
        public Reservation Reservation { get; private set; }
        public DateTime WithdrawDate { get; private set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ExpireDate { get; private set; }
        public bool IsOpen { get; private set; }
        public List<Book> Books { get; private set; }

        public void SetIsOpen(bool state)=>IsOpen= state;
    }
}
