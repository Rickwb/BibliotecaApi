using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    public class WithdrawController : BaseControl<CreateWithdrawDTO, Withdraw>
    {
        private readonly WithdrawService _withdrawService;
        private readonly ReservationService _reservationService;
        private readonly CustomerService _customerService;
        public WithdrawController(WithdrawService withdrawService,CustomerService customerService,ReservationService reservationService)
        {
            _withdrawService = withdrawService;
            _customerService = customerService;
            _reservationService = reservationService;
        }
        public override IActionResult Add(CreateWithdrawDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();

            var customer = _customerService.GetUserById(dto.IdCustomer);

            if (dto.IdReservation== Guid.Empty)
            {
                var withdraw = new Withdraw(
                    customer: customer,
                    books: dto.BooksNoReservation);
            }
            else
            {
                var reservation = _reservationService.
            }

        }

        public override IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Update(Guid id, CreateWithdrawDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
