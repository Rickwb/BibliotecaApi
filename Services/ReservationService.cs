using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class ReservationService
    {

        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public Reservation AddReservation(Reservation reservation)
        {
            return _reservationRepository.Add(reservation);
        }

        public Reservation UpdateReservation(Guid idReservation,Reservation reservation)
        {
            return _reservationRepository.Update(idReservation, reservation);
        }

        public bool ValidarReserva(Reservation reservation)
        {
            var reservas = _reservationRepository.GetAll();
            
            foreach (var reserv in reservas)
            {
                reserv.Books.ForEach(book => reservation.Books.Contains(book));
            }

        }
    }
}
