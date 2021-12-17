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

        public ReservationService(ReservationRepository reservationRepository, WithdrawService withdrawService, BookRepository bookRepository)
        {
            _reservationRepository = reservationRepository;
            _withdrawService = withdrawService;
            _bookRepository = bookRepository;
        }

        public CreateResult<Reservation> AddReservation(Reservation reservation)
        {
            if (ValidarReserva(reservation))
            {
                if (reservation.Client.Multa != 0)
                    return CreateResult<Reservation>.Errors(new InvalidDataExeception("O cliente não pode reservar tendo multas"));

                _reservationRepository.Add(reservation);
                return CreateResult<Reservation>.Sucess(reservation);
            };

            
            return null;
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
            if (_reservationRepository.CancelarReserva(idReservation, out books)) return false;

            return true;
        }
        public bool FinalizeReserva(Guid idReservation)
        {
            var reserv = _reservationRepository.GetById(idReservation);

            var withdraw = new Withdraw(reserv.Client
                , reservation: reserv);
            if (!_withdrawService.ValidWithdraw(withdraw)) return false;

            _withdrawService.AddWithdraw(withdraw);

            List<Book> books = new List<Book>();
            bool finalized = _reservationRepository.FinalizarReserva(idReservation, out books);

            books.ForEach(book => _bookRepository.Update(book.Id, book));

            return finalized;
        }

        public bool ValidarReserva(Reservation reservation)
        {
            var books = reservation.Books;
            List<Reservation> reservations = new List<Reservation>();
            List<Withdraw> withdraws = new List<Withdraw>();
            foreach (var b in books)
            {
                reservations = _reservationRepository.GetAll().Where(r => (r.StartDate.Date >= reservation.StartDate.Date && r.StartDate.Date<=reservation.EndDate.Date) 
                || (r.EndDate.Date<=reservation.EndDate.Date && r.EndDate.Date>=reservation.StartDate.Date) && r.Books.Contains(b)).ToList();

                withdraws = _withdrawService.GetAll().Where(w => (w.WithdrawDate.Date >= reservation.StartDate.Date && w.WithdrawDate.Date<=reservation.EndDate.Date)
                || (w.ReturnDate<=reservation.EndDate && w.ReturnDate.Date>=reservation.StartDate.Date) && w.Books.Contains(b)).ToList(); ;

                if (reservations.Count() +1 + withdraws.Count() > b.NumCopies)
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
