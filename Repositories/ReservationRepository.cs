﻿using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>
    {
        public List<Reservation> GetReservationsWithParams(DateTime? startDate, DateTime? endDate, Authors? author, string? bookName, int page=1, int items=5)
        {
            var reservations = (IEnumerable<Reservation>)_repository
                .WhereIfNotNull(startDate, x => x.StartDate.ToString("dd/MM/yyyy") == startDate?.ToString("dd/MM/yyyy"))
                .WhereIfNotNull(endDate, x => x.EndDate.ToString("dd/MM/yyyy") == endDate?.ToString("dd/MM/yyyy"))
                .WhereIfNotNull(author, x => !x.Books.Any(y => y.Author == author))
                .WhereIfNotNull(bookName, x => x.Books.Any(y => y.Title == bookName))
                .Skip((page - 1) * items).Take(items);

            return reservations.ToList();
        }

        public bool CancelarReserva(Guid id,out List<Book> booksCopies)
        {
            var reservation = GetById(id);
            booksCopies = new List<Book>();
            if (reservation == null) return false;

            reservation.CancelarReserva();
            foreach (var b in reservation.Books)
            {
                b.ControNumberOfAvailableCopies(false,1);
                booksCopies.Add(b);
            }
            var reserv = Update(id, reservation);

            return reserv != null ? true : false;

        }
        public bool FinalizarReserva(Guid id)
        {
            var reservation = GetById(id);
            if (reservation == null) return false;

        

            reservation.FinalizarReserva();

            var reserv = Update(id, reservation);

            return reserv != null ? true : false;

        }
    }
}
