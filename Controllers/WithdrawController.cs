using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController,Authorize,Route("[controller]")]
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
            Withdraw withdraw;
            if (dto.IdReservation== Guid.Empty)
            {
                    withdraw = new Withdraw(
                    customer: customer,
                    books: dto.BooksNoReservation);
            }
            else
            {
                var reservation = _reservationService.GetReservationById(dto.IdReservation);
                    withdraw = new Withdraw(
                    customer: customer,
                    reservation: reservation);
            }

            return Created("",_withdrawService.AddWithdraw(withdraw));

        }
        [HttpGet,Route("{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_withdrawService.GetWidtdrawById(id));
        }
        [HttpPost,Route("finalize/{id}")]
        public IActionResult FinalizeWithdraw(Guid id)
        {
            return Ok(_withdrawService.FinalizarWithdraw(id));
        }


        public IActionResult GetWithdrawByParams([FromQuery]bool? isOpen, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Authors author, [FromQuery] string bookName, [FromQuery] int page, [FromQuery] int items)
        {
            return Ok(_withdrawService.GetWithdrawByParams(isOpen, startDate, endDate, author, bookName, page, items));
        }
        public override IActionResult Update(Guid id, CreateWithdrawDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
