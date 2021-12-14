using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class ReservationRepository:BaseRepository<Reservation>
    {
        public List<Reservation> GetAllBooksWithParams(DateTime? startDate,DateTime? endDate, Authors author, string? bookName,int page, int items)
        {
            var reservations = (IEnumerable<Reservation>)_repository;
            if (startDate is not null)
                reservations = reservations.WhereIfNotNull(startDate, x => x.StartDate == startDate);
            if (endDate is not null)
                reservations = reservations.WhereIfNotNull(endDate, x => x.EndDate == endDate);
            if (author is not null)
            {
                var authorinBooks=reservations.Where(x=>x.Books.ForEach(i=>i.Author)==author)
                reservations = reservations.WhereIfNotNull(author, x => x.Books == author);
            }
            if (bookName is not null)
                reservations = reservations.WhereIfNotNull(bookName, x => x.Books);

            if (page != 0 && items != 0)
                reservations = reservations.Skip((page - 1) * items).Take(items);

            return reservations.ToList();
        }

        public bool CancelarReserva(Guid id)
        {
            var reservation = GetById(id);
            if (reservation == null) return false;

            reservation.CancelarReserva();

            Update(id, reservation);

            return true;

        }
    }
}
