using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class WithdrawRepository : BaseRepository<Withdraw>
    {
        public List<Withdraw> GetAllBooksWithParams(bool? isOpen, DateTime? startDate, DateTime? endDate, Authors author, string? bookName, int page = 1, int items = 5)
        {
            var withdraws = (IEnumerable<Withdraw>)_repository
                .WhereIfNotNull(startDate, x => x.WithdrawDate.ToString("dd/MM/yyyy") == startDate?.ToString("dd/MM/yyyy"))
                .WhereIfNotNull(endDate, x => x.ReturnDate.ToString("dd/MM/yyyy") == endDate?.ToString("dd/MM/yyyy"))
                .WhereIfNotNull(bookName, x => !x.Books.Any(y => y.Title == bookName))
                .WhereIfNotNull(isOpen, x => x.IsOpen==isOpen)
                .Skip((page - 1) * items).Take(items);


            return withdraws.ToList();
        }

        public bool FinalizaWidthdraw(Guid idWith, out List<Book> booksCopies)
        {
            var withdraw = GetById(idWith);
            booksCopies = new List<Book>();
            if (withdraw is not null)
            {
                withdraw.SetIsOpen(false);
                withdraw.ReturnDate = DateTime.Now;
                foreach (var b in withdraw.Books)
                {
                    b.ControNumberOfAvailableCopies(false, 1);
                    booksCopies.Add(b);
                }
                Update(idWith, withdraw);
                return true;
            }
            return false;
        }
    }
}
