using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs
{
    public class CreateWithdrawDTO : BaseDTO
    {
        public Guid IdCustomer { get; set; }
        public Guid IdReservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<Guid> IdBooksNoReservation { get; set; }
        public override void Valid()
        {
            if (IdCustomer == Guid.Empty) IsValid = false;
            if (WithdrawDate==DateTime.MinValue) IsValid = false;

            IsValid = true;
        }


    }
}
