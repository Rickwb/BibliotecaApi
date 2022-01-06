using Domain.Enities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class WithdrawResultDTO : CreateResultDTO<Withdraw>
    {
        public WithdrawResultDTO(Withdraw withdraw)
        {
            WithdrawId = withdraw.Id;
            CustomerResult = new CustomerResultDTO(withdraw.Customer);
            if (withdraw.Reservation != null)
            {
                IdReservation = withdraw.Reservation.Id;
            }
            WithdrawDate = withdraw.WithdrawDate;
            ReturnDate = withdraw.WithdrawDate;
            booksRented = new List<BookResultDTO>();
            withdraw.Books.ForEach(book => booksRented.Add(new BookResultDTO(book)));
        }

        public WithdrawResultDTO(CreationException exception)
        {
            Errors = new List<string>() { exception.Message };
        }
        public Guid WithdrawId { get; set;}
        public CustomerResultDTO CustomerResult { get; set; }
        public Guid IdReservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<BookResultDTO> booksRented { get; set; }

        public List<string> GetErrors() => Errors;
    }
}
