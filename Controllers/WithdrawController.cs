﻿using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class WithdrawController : BaseControl<CreateWithdrawDTO, Withdraw>
    {
        private readonly WithdrawService _withdrawService;
        private readonly ReservationService _reservationService;
        private readonly BookAuthorService _bookAuthorService;
        private readonly CustomerService _customerService;
        public WithdrawController(WithdrawService withdrawService, CustomerService customerService, ReservationService reservationService, BookAuthorService bookAuthorService)
        {
            _withdrawService = withdrawService;
            _customerService = customerService;
            _reservationService = reservationService;
            _bookAuthorService = bookAuthorService;
        }

        [HttpPost]
        public override IActionResult Add(CreateWithdrawDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();

            var customer = _customerService.GetUserById(dto.IdCustomer);
            List<Book> books = new List<Book>();
            dto.IdBooksNoReservation.ForEach(b => books.Add(_bookAuthorService.GetBookById(b)));
            Withdraw withdraw;
            if (dto.IdReservation == Guid.Empty)
            {
                withdraw = new Withdraw(
                customer: customer,
                books: books);
            }
            else
            {
                var reservation = _reservationService.GetReservationById(dto.IdReservation);
                withdraw = new Withdraw(
                customer: customer,
                reservation: reservation);
            }
            var result = _withdrawService.AddWithdraw(withdraw);
            if (result.Error==false)
                return Created("", new WithdrawResultDTO(withdraw));

            return BadRequest(new WithdrawResultDTO(result.Exception));
        }

        [HttpGet, Route("{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_withdrawService.GetWidtdrawById(id));
        }

        [HttpPost, Route("finalize/{id}")]
        public IActionResult FinalizeWithdraw(Guid id)
        {
            return Ok(_withdrawService.FinalizarWithdraw(id));
        }

        [HttpGet]
        public IActionResult GetWithdrawByParams([FromQuery] bool? isOpen, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid idAuthor, [FromQuery] string bookName, [FromQuery] int page, [FromQuery] int items)
        {
            var author = _bookAuthorService.GetAuthorById(idAuthor);
            return Ok(_withdrawService.GetWithdrawByParams(isOpen, startDate, endDate, author, bookName, page, items));
        }

        [HttpPut, Route("/{id}")]
        public override IActionResult Update(Guid id, CreateWithdrawDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
