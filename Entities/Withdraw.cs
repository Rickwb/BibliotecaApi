using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Withdraw : BaseEntity
    {
        public Withdraw(Customer customer,Reservation reservation)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            Books = reservation.Books;
            WithdrawDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(5);
            IsOpen = true;

        }

        public Withdraw(Customer customer,List<Book> books)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            Books = books;
            WithdrawDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(5);
            IsOpen = true;
        }
        public Customer Customer { get; set; }
        public Reservation Reservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsOpen { get; set; }
        public List<Book> Books { get; set; }
    } 
}
