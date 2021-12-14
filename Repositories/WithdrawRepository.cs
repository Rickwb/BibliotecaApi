using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class WithdrawRepository : BaseRepository<Withdraw>
    {
        public List<Withdraw> GetAllBooksWithParams(bool? isOpen, DateTime? startDate, DateTime? endDate, Authors author, string? bookName, int page=1, int items=5)
        {
            var withdraws = (IEnumerable<Withdraw>)_repository;
            
            if (startDate is not null)
                withdraws = withdraws.WhereIfNotNull(startDate, x => x.WithdrawDate.ToString("dd/MM/yyyy") == startDate?.ToString("dd/MM/yyyy"));
            if (endDate is not null)
                withdraws = withdraws.WhereIfNotNull(endDate, x => x.ReturnDate.ToString("dd/MM/yyyy") == endDate?.ToString("dd/MM/yyyy"));
            if (author is not null)
                withdraws = withdraws.Where(x => !x.Reservation.Books.Any(y => y.Author == author));
            if (bookName is not null)
                withdraws = withdraws.WhereIfNotNull(bookName, x => !x.Reservation.Books.Any(y=>y.Title==bookName));
            if (isOpen is not null)
                withdraws = withdraws.WhereIfNotNull(isOpen, x => x.IsOpen);
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
                withdraw.ReturnDate = DateTime.Now;
                Update(idWith, withdraw);
                return true;
            }
            return false;
        }
    }
}
