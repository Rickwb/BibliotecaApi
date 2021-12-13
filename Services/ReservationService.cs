using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
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
            ValidarReserva(reservation);
            return _reservationRepository.Add(reservation);
        }

        public Reservation UpdateReservation(Guid idReservation, Reservation reservation)
        {
            ValidarReserva(reservation);
            return _reservationRepository.Update(idReservation, reservation);
        }

        public bool ValidarReserva(Reservation reservation)
        {
            var reservas = _reservationRepository.GetAll();
            foreach (var reserv in reservas)
            {
                if (reserv.EndDate > reservation.StartDate || reservation.EndDate > reserv.StartDate)
                {

                    foreach (var r in reserv.Books)
                    {
                        int contagem = reservation.Books.Where(x => x.Id == r.Id).Count();
                        if (contagem>r.NumCopies)
                            return false;
                    }
                }

            }
            return true;


        }
        public IEnumerable<Reservation> GetReservationsLoggedUser()
        {
            return _reservationRepository.GetAll()
        }
        public Reservation GetReservationById(Guid id)
        {
            return _reservationRepository.GetById(id);
        }
    }
}
