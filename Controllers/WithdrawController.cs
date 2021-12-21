using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class WithdrawController : BaseControl<CreateWithdrawDTO, Withdraw>
    {
        private readonly WithdrawService _withdrawService;
        private readonly ReservationService _reservationService;
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly CustomerService _customerService;
        public WithdrawController(WithdrawService withdrawService, CustomerService customerService, ReservationService reservationService, BookService bookAuthorService,
            AuthorService authorService)
        {
            _withdrawService = withdrawService;
            _customerService = customerService;
            _reservationService = reservationService;
            _bookService = bookAuthorService;
            _authorService = authorService;
        }

        [HttpPost]
        public override IActionResult Add(CreateWithdrawDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();

            var customer = _customerService.GetUserById(dto.IdCustomer);
            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer")
                if (ValidarCustomer(customer.UserId)) return BadRequest(new WithdrawResultDTO(new InvalidDataExeception("O cliente não pode alterar a reserva de outro cliente")));

            Reservation reservation;
            List<Book> books = new List<Book>();
            Withdraw withdraw;

            if (dto.IdReservation == Guid.Empty)
            {
                dto.IdBooksNoReservation.ForEach(b => books.Add(_bookService.GetBookById(b)));
                withdraw = new Withdraw(
                customer: customer,
                books: books);
            }
            else
            {
                    reservation = _reservationService.GetReservationById(dto.IdReservation);
                _reservationService.FinalizeReserva(dto.IdReservation, out withdraw);
                
            }
            var result = _withdrawService.AddWithdraw(withdraw);
            if (result.Error == false)
                return Created("", new WithdrawResultDTO(result.CreatedObj));

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
            var customer = _withdrawService.GetWidtdrawById(id).Customer;
            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer")
                if (ValidarCustomer(customer.UserId)) return BadRequest(new WithdrawResultDTO(new InvalidDataExeception("O cliente não pode alterar a reserva de outro cliente")));
            var resultwithdraw = _withdrawService.FinalizarWithdraw(id);
            if (resultwithdraw.Error == true)
            {
                return BadRequest(new WithdrawResultDTO(resultwithdraw.Exception).GetErrors());
            }

            return Ok(new WithdrawResultDTO(resultwithdraw.CreatedObj));
        }

        [HttpGet, ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetWithdrawByParams([FromQuery] bool? isOpen, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Guid idAuthor, [FromQuery] string bookName, [FromQuery] int page = 1, [FromQuery] int items = 5)
        {
            var author = _authorService.GetAuthorById(idAuthor);
            var withdraws = _withdrawService.GetWithdrawByParams(isOpen, startDate, endDate, author, bookName, page, items);
            List<WithdrawResultDTO> results = new List<WithdrawResultDTO>();
            withdraws.ToList().ForEach(withdraw => results.Add(new WithdrawResultDTO(withdraw)));
            return Ok(results);
        }

        [HttpPut, Route("/{id}")]
        public override IActionResult Update(Guid id, CreateWithdrawDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
