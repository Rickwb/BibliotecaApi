﻿using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>
    {
        public List<Reservation> GetReservationsWithParams(DateTime? startDate, DateTime? endDate, Authors author, string? bookName, int page=1, int items=5)
        {
            var reservations = (IEnumerable<Reservation>)_repository
                .WhereIfNotNull(startDate, x => x.StartDate == startDate)
                .WhereIfNotNull(endDate, x => x.EndDate == endDate)
                .WhereIfNotNull(author, x => !x.Books.Any(y => y.Author == author))
                .WhereIfNotNull(bookName, x => x.Books.Any(y => y.Title == bookName))
                .Skip((page - 1) * items).Take(items);

            return reservations.ToList();
        }

        public bool CancelarReserva(Guid id)
        {
            var reservation = GetById(id);
            if (reservation == null) return false;

            reservation.CancelarReserva();

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
