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
            var Allbooks = _reservationRepository.GetAll().ToList();
            bool hasAlready = Allbooks.Find(x => x.Client.Id == reservation.Client.Id && x.Books.Any(b => reservation.Books.Contains(b))) == null ? false : true;
            if (hasAlready)
                return CreateResult<Reservation>.Errors(new SameObjectExeception("Este cliente já tem uma reserva para com este(es) livro(os)"));

            bool valid;
            reservation = ValidarReserva(reservation, out valid);
            if (valid)
            {
                //if (reservation.Client.Multa != 0)
                //    return CreateResult<Reservation>.Errors(new InvalidDataExeception("O cliente não pode reservar tendo multas"));

                _reservationRepository.Add(reservation);
                return CreateResult<Reservation>.Sucess(reservation);
            };


            return CreateResult<Reservation>.Errors(new CreationException("Não foi possivel cadastrar"));
        }

        public CreateResult<Reservation> UpdateReservation(Guid idReservation, Reservation reservation)
        {
            bool valid;
            reservation = ValidarReserva(reservation, out valid);

            var reservaUpdate=_reservationRepository.Update(idReservation, reservation);
            if (reservaUpdate!=null)
                return CreateResult<Reservation>.Sucess(reservation);
            return CreateResult<Reservation>.Errors(new CreationException("não foi possível atualizar"));
        }

        public CreateResult<Reservation> CancelReservation(Guid idReservation)
        {

            var reservation = _reservationRepository.GetById(idReservation);
            List<Book> books;
            if (_reservationRepository.CancelarReserva(idReservation, out books) == null) return CreateResult<Reservation>.Errors(new CreationException("Not canceled reservation"));

            return CreateResult<Reservation>.Sucess(reservation);
        }
        public CreateResult<Reservation> FinalizeReserva(Guid idReservation, out Withdraw withdraw)
        {
            var reserv = _reservationRepository.GetById(idReservation);

            withdraw = new Withdraw(reserv.Client
               , reservation: reserv);

            withdraw = _withdrawService.ValidWithdraw(withdraw, out bool validWithdraw);
            if (!validWithdraw) return null;

            _withdrawService.AddWithdraw(withdraw);

            List<Book> books = new List<Book>();
            bool finalized = _reservationRepository.FinalizarReserva(idReservation, out books);

            books.ForEach(book => _bookRepository.Update(book.Id, book));

            return CreateResult<Reservation>.Sucess(reserv);
        }

        public Reservation ValidarReserva(Reservation reservation, out bool valid)
        {
            var books = reservation.Books;
            List<Reservation> reservations = new List<Reservation>();
            List<Withdraw> withdraws = new List<Withdraw>();
            foreach (var b in books)
            {
                reservations = _reservationRepository.GetAll().Where(r => (r.StartDate.Date >= reservation.StartDate.Date && r.StartDate.Date <= reservation.EndDate.Date)
                || (r.EndDate.Date <= reservation.EndDate.Date && r.EndDate.Date >= reservation.StartDate.Date))
                    .Where(x => x.Books.Any(y => y.Id == b.Id))
                    .Where(r => r.GetCanceledValue() == false && r.GetCompletedValue() == false).ToList();

                withdraws = _withdrawService.GetAll().Where(w => (w.WithdrawDate.Date >= reservation.StartDate.Date && w.WithdrawDate.Date <= reservation.EndDate.Date)
                || (w.ReturnDate <= reservation.EndDate && w.ReturnDate.Date >= reservation.StartDate.Date))
                    .Where(x => x.Books.Any(y => y.Id == b.Id))
                    .Where(x => x.IsOpen).ToList();

                if (reservations.Count() + 1 + withdraws.Count() > b.NumCopies)
                {
                    valid = false;
                    return null;
                }
            }
            valid = true;
            return reservation;
        }

        public IEnumerable<Reservation> GetReservationsLoggedUser()
        {
            return _reservationRepository.GetAll();
        }
        public Reservation GetReservationById(Guid id)
        {
            return _reservationRepository.GetById(id);
        }

        public IEnumerable<Reservation> GetReservationsByParams(DateTime? startDate, DateTime? endDate, Authors? author, string? bookName, int page = 1, int items = 5)
        {
            return _reservationRepository.GetReservationsWithParams(startDate, endDate, author, bookName, page, items);
        }

        public IEnumerable<Reservation> GetAllReservations() => _reservationRepository.GetAll();

    }
}
