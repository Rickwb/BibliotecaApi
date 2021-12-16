using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class WithdrawResultDTO:CreateResultDTO<Withdraw>
    {

        public WithdrawResultDTO(Withdraw withdraw)
        {
            CustomerResult = new CustomerResultDTO(withdraw.Customer);
            IdReservation = withdraw.Reservation.Id;
            WithdrawDate = withdraw.WithdrawDate;
            ReturnDate = withdraw.WithdrawDate;
        }

        public WithdrawResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }



        public CustomerResultDTO CustomerResult { get; set; }
        public Guid IdReservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
