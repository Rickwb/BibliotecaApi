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
            Books= reservation.Books;
        }
        public ReservationResultDTO(CreationException exception)
        {
            Errors= new List<string>() { exception.Message };
        }

        public Guid ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Book> Books { get; set; }

        public List<string> GetErrors() => Errors;

    }
}
