using System;

namespace BibliotecaApi.Entities
{
    public class Withdraw : BaseEntity
    {
        public Withdraw()
        {
            Id = Guid.NewGuid();
        }
        public Customer Customer { get; set; }
        public Reservation Reservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsOpen { get; set; }
    } 
}
