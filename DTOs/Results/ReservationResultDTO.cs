using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs.Results
{
    public class ReservationResultDTO:CreateResultDTO<Reservation>
    {
        public ReservationResultDTO(Reservation reservation)
        {
            ReservationId = reservation.Id;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            Books = new List<BookResultDTO>();
            reservation.Books.ForEach(book => Books.Add(new BookResultDTO(book)));
        }
        public ReservationResultDTO(CreationException exception)
        {
            Errors= new List<string>() { exception.Message };
        }

        public Guid ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BookResultDTO> Books { get; set; }

        public List<string> GetErrors() => Errors;

    }
}
