using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class WithdrawService
    {
        private readonly WithdrawRepository _withdrawRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly BookRepository _bookRepository;
        public WithdrawService(WithdrawRepository withdrawRepository, ReservationRepository reservationRepository, BookRepository bookRepository)
        {
            _withdrawRepository = withdrawRepository;
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
        }

        public Withdraw GetWidtdrawById(Guid id) => _withdrawRepository.GetById(id);

        public IEnumerable<Withdraw> GetWithdrawByParams(bool? isOpen, DateTime? startDate, DateTime? endDate, Authors? author, string? bookName, int page, int items)
        {
            return _withdrawRepository.GetAllBooksWithParams(isOpen, startDate, endDate, author, bookName, page, items);
        }
        public CreateResult<Withdraw> AddWithdraw(Withdraw withdraw)
        {
            if (ValidWithdraw(withdraw))
            {
                _withdrawRepository.Add(withdraw);
                return CreateResult<Withdraw>.Sucess(withdraw);
            }

            return CreateResult<Withdraw>.Errors(new CreationException("Aluguel Inválido"));
        }

        public bool FinalizarWithdraw(Guid id)
        {
            List<Book> books;
            if (!_withdrawRepository.FinalizaWidthdraw(id, out books)) return false;

            books.ForEach(b => _bookRepository.Update(b.Id, b));

            return true;

        }

        public bool ValidWithdraw(Withdraw withdraw)
        {
            var books = withdraw.Books;
            List<Reservation> reservations = new List<Reservation>();
            List<Withdraw> withdraws = new List<Withdraw>();
            foreach (var b in books)
            {
                reservations = _reservationRepository.GetAll().Where(r => (r.StartDate.Date >= withdraw.WithdrawDate.Date && r.StartDate.Date <= withdraw.ReturnDate.Date) || (r.EndDate <= withdraw.ReturnDate.Date && r.EndDate >= withdraw.WithdrawDate.Date) && r.Books.Contains(b)).ToList();
                withdraws = _withdrawRepository.GetAll().Where(w => (w.WithdrawDate.Date >= withdraw.WithdrawDate.Date && w.WithdrawDate.Date <= withdraw.ReturnDate.Date) || (w.ReturnDate <= withdraw.ReturnDate && w.ReturnDate.Date >= withdraw.WithdrawDate.Date) && w.Books.Contains(b)).ToList();


                if (reservations.Count() + withdraws.Count() + 1 > b.NumCopies)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Withdraw> GetAll() => _withdrawRepository.GetAll();
    }
}
