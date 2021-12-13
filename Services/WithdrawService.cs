using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public class WithdrawService
    {
        private readonly WithdrawRepository _withdrawRepository;

        public WithdrawService(WithdrawRepository withdrawRepository)
        {
            _withdrawRepository = withdrawRepository;
        }

        public Withdraw GetWidtdrawById(Guid id) => _withdrawRepository.GetById(id);
        
        public IEnumerable<Withdraw> GetWithdrawByParams(bool isOpen,DateTime startDate, DateTime endDate, Authors author, string bookName, int page, int items)
        {
            return 
        }
        public Withdraw AddWithdraw(Withdraw withdraw)
        {
            return _withdrawRepository.Add(withdraw);
        }

        public bool FinalizarWithdraw(Guid id)
        {
            return _withdrawRepository.FinalizaWidthdraw(id);
        }
    }
}
