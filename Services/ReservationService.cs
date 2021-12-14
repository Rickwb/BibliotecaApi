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
        private readonly WithdrawService _withdrawService;
        private readonly BookRepository _bookRepository;

        public ReservationService(ReservationRepository reservationRepository, WithdrawService withdrawService,BookRepository bookRepository)
        {
            _reservationRepository = reservationRepository;
            _withdrawService = withdrawService;
            _bookRepository = bookRepository;
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
        public bool CancelReservation(Guid idReservation)
        {
            var reservation = _reservationRepository.GetById(idReservation);
            List<Book> books;
            if (_reservationRepository.CancelarReserva(idReservation,out books)) return false;

            books.ForEach(book => _bookRepository.Update(book.Id, book));

            return true;

        }
        public bool FinalizeReserva(Guid idReservation)
        {
            var reserv = _reservationRepository.GetById(idReservation);

            var withdraw = new Withdraw(reserv.Client
                , reservation: reserv);
            if (!_withdrawService.ValidWithdraw(withdraw)) return false;

            _withdrawService.AddWithdraw(withdraw);


            return _reservationRepository.FinalizarReserva(idReservation);
        }

        public bool ValidarReserva(Reservation reservation)
        {
            var books = reservation.Books;
            IEnumerable<Reservation> reservations;
            IEnumerable<Withdraw> withdraws;
            foreach (var b in books)
            {
                reservations = GetReservationsByParams(startDate: reservation.StartDate, endDate: null, null, null, 1, 10).Where(x => x.Books.All(y => y.Id == b.Id));
                withdraws = _withdrawService.GetWithdrawByParams(isOpen: true,
                   startDate: reservation.StartDate,
                   endDate: null,
                   null,
                   null,
                   1,
                   10
                   ).Where(x => x.Books.All(y => y.Id == b.Id));

                if (reservations.Count() + withdraws.Count() > b.NumCopies)
                {
                    return false;
                }
            }

            return true;


        }
        public IEnumerable<Reservation> GetReservationsLoggedUser()
        {
            return _reservationRepository.GetAll();
        }
        public Reservation GetReservationById(Guid id)
        {
            return _reservationRepository.GetById(id);
        }

        public IEnumerable<Reservation> GetReservationsByParams(DateTime? startDate, DateTime? endDate, Authors? author, string? bookName, int page, int items)
        {
            return _reservationRepository.GetReservationsWithParams(startDate, endDate, author, bookName, page, items);
        }

    }
}
