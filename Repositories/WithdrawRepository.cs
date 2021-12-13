using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Repositories
{
    public class WithdrawRepository : BaseRepository<Withdraw>
    {
        public List<Reservation> GetAllBooksWithParams(bool isOpen, DateTime? startDate, DateTime? endDate, Authors author, string? bookName, int page, int items)
        {
            var withdraws = (IEnumerable<Reservation>)_repository;
            if (startDate is not null)
                withdraws = withdraws.WhereIfNotNull(startDate, x => x.StartDate == startDate);
            if (endDate is not null)
                withdraws = withdraws.WhereIfNotNull(endDate, x => x.EndDate == endDate);
            if (author is not null)
                withdraws = withdraws.WhereIfNotNull(author, x => x.Books.);
            if (bookName is not null)
                withdraws = withdraws.WhereIfNotNull(bookName, x => x.Books);

            if (page != 0 && items != 0)
                withdraws = withdraws.Skip((page - 1) * items).Take(items);

            return withdraws.ToList();
        }


        public bool FinalizaWidthdraw(Guid idWith)
        {
            var withdraw = GetById(idWith);
            if (withdraw is not null)
            {
                withdraw.IsOpen= false;
                Update(idWith, withdraw);
                return true;
            }
            return false;
        }
    }
}
