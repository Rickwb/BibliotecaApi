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
        public WithdrawService(WithdrawRepository withdrawRepository,ReservationRepository reservationRepository)
        {
            _withdrawRepository = withdrawRepository;
            _reservationRepository = reservationRepository;

        }

        public Withdraw GetWidtdrawById(Guid id) => _withdrawRepository.GetById(id);

        public IEnumerable<Withdraw> GetWithdrawByParams(bool? isOpen, DateTime? startDate, DateTime? endDate, Authors? author, string? bookName, int page, int items)
        {
            return _withdrawRepository.GetAllBooksWithParams(isOpen, startDate, endDate, author, bookName, page, items);
        }
        public Withdraw AddWithdraw(Withdraw withdraw)
        {


            if (ValidWithdraw(withdraw))
            {
                return _withdrawRepository.Add(withdraw);

            }

            throw new Exception("Invalid Withdraw");

        }

        public bool FinalizarWithdraw(Guid id)
        {
            return _withdrawRepository.FinalizaWidthdraw(id);
        }

        public bool ValidWithdraw(Withdraw withdraw)
        {
            var books = withdraw.Books;
            IEnumerable<Reservation> reservations;
            IEnumerable<Withdraw> withdraws;
            foreach (var b in books)
            {
                reservations = _reservationRepository.GetReservationsWithParams(startDate: withdraw.WithdrawDate, endDate: withdraw.ExpireDate, null, null, 0, 100).Where(x => x.Books.All(y => y.Id == b.Id));
                withdraws = GetWithdrawByParams(isOpen: true,
                   startDate: withdraw.WithdrawDate,
                   endDate: withdraw.ExpireDate,
                   null,
                   null,
                   1,
                   100
                   ).Where(x => x.Books.All(y => y.Id == b.Id));

                if (reservations.Count() + withdraws.Count()> b.NumCopies)
                {
                    return false;
                } 
            }
            return true;




        }
    }
}
