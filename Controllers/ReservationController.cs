using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class ReservationController : BaseControl<CreateReservationDTO, Reservation>
    {
        private readonly ReservationService _reservationService;
        private readonly CustomerService _customerService;
        private readonly BookAuthorService _bookauthorService;

        public ReservationController(ReservationService reservationService, CustomerService customerService, BookAuthorService bookAuthorService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _bookauthorService = bookAuthorService;

        }

        [HttpPost]
        public override IActionResult Add(CreateReservationDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();
            var customer = _customerService.GetUserById(dto.idCustumer);

            List<Book> Books = new List<Book>();
            dto.IdBooks.ForEach(x => Books.Add(_bookauthorService.GetBookById(x)));

            var reservation = new Reservation(
                id: Guid.NewGuid(),
                client: customer,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                books: Books
                );
            var reserv = _reservationService.AddReservation(reservation);
            if (reserv is not null)
                return Created("", reserv);

            return BadRequest();
        }
        [HttpPost, Route("finalize/{id}")]
        public IActionResult FinalzeReservation(Guid id)
        {
            return Ok(_reservationService.FinalizeReserva(id));
        }
        [HttpPost, Route("cancel/{id}")]
        public IActionResult CancelReservation(Guid id)
        {
            return Ok(_reservationService.CancelReservation(id));
        }


        [HttpGet, Route("{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_reservationService.GetReservationById(id));
        }
        [HttpGet, Route("params")]
        public IActionResult GetReservationByParams([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid idAuthor,
            [FromQuery] string? bookName, [FromQuery] int page, [FromQuery] int items)
        {
            var author = _bookauthorService.GetAuthorById(idAuthor);
            return Ok(_reservationService.GetReservationsByParams(startDate, endDate, author, bookName, page, items));
        }
        [HttpPut, Route("{id}")]
        public override IActionResult Update(Guid id, CreateReservationDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();
            var customer = _customerService.GetUserById(dto.idCustumer);

            List<Book> Books = new List<Book>();
            dto.IdBooks.ForEach(x => Books.Add(_bookauthorService.GetBookById(x)));

            var reservation = new Reservation(
                id: id,
                client: customer,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                books: Books
                );


            return Ok(_reservationService.UpdateReservation(id, reservation));
        }
    }
}
