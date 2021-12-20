using BibliotecaApi.DTOs;
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
    public class ReservationController : BaseControl<CreateReservationDTO, Reservation>
    {
        private readonly ReservationService _reservationService;
        private readonly CustomerService _customerService;
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;

        public ReservationController(ReservationService reservationService, CustomerService customerService, BookService bookAuthorService, AuthorService authorService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _bookService = bookAuthorService;
            _authorService = authorService;
        }

        [HttpPost]
        public override IActionResult Add(CreateReservationDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();

            var customer = _customerService.GetUserById(dto.idCustumer);

            List<Book> Books = new List<Book>();
            dto.IdBooks.ForEach(x => Books.Add(_bookService.GetBookById(x)));

            var reservation = new Reservation(
                id: Guid.NewGuid(),
                client: customer,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                books: Books
                );

            var result = _reservationService.AddReservation(reservation);
            if (result.Error == false)
                return Created("", new ReservationResultDTO(reservation));

            return BadRequest(new ReservationResultDTO(result.Exception).GetErrors());
        }

        [HttpPost, Route("finalize/{id}")]
        public IActionResult FinalzeReservation(Guid id)
        {
            Withdraw withdraw;
            return Ok(_reservationService.FinalizeReserva(id,out withdraw));
        }

        [HttpPost, Route("cancel/{id}")]
        public IActionResult CancelReservation(Guid id)
        {
            var reserva = _reservationService.CancelReservation(id);
            if (reserva != null)
                return Ok(reserva);
            return BadRequest();
        }

        [HttpGet, Route("{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_reservationService.GetReservationById(id));
        }

        [HttpGet, Route("params"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetReservationByParams([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid idAuthor,
            [FromQuery] string? bookName, [FromQuery] int page = 1, [FromQuery] int items = 5)
        {
            Authors author;
            if(idAuthor!=Guid.Empty)
                author = _authorService.GetAuthorById(idAuthor);
            else
                author = null;

            return Ok(_reservationService.GetReservationsByParams(startDate, endDate, author, bookName, page, items));
        }

        [HttpPut, Route("{id}")]
        public override IActionResult Update(Guid id, CreateReservationDTO dto)
        {
            dto.ValidUpdate();
            if (!dto.UpdateValid) return BadRequest();

            var customer = _customerService.GetUserById(dto.idCustumer);

            List<Book> Books = new List<Book>();
            dto.IdBooks.ForEach(x => Books.Add(_bookService.GetBookById(x)));

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
