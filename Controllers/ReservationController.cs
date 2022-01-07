using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Services;
using Domain.Enities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
            if (!dto.IsValid) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("Dados Invalidos")));

            var LogeedUserId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var customer = _customerService.GetAllCustomers().Where(x=>x.UserId.ToString()==LogeedUserId).SingleOrDefault();

            ////if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer
            // if (ValidarCustomer(customer.Id)) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("O cliente não pode cadastrar reservas para outras pessoas")).GetErrors());
            //if (customer is null) return BadRequest(new ReservationResultDTO(new CreationException("Cliente invalido")));
                    

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
            var reservation = _reservationService.GetReservationById(id);
            if (reservation.GetCompletedValue())
                return BadRequest(new ReservationResultDTO(new InvalidDataExeception("esta reserva já foi finalizada")).GetErrors());

            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer")
                if (ValidarCustomer(reservation.Client.UserId)) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("O clietne atual não pode acessar ")).GetErrors());

            var result = _reservationService.FinalizeReserva(id, out withdraw);

            if (result.Error == false)
                return Ok(result.CreatedObj);
            return BadRequest(new ReservationResultDTO(result.Exception).GetErrors());
        }

        [HttpPost, Route("cancel/{id}")]
        public IActionResult CancelReservation(Guid id)
        {
            var reserva = _reservationService.GetReservationById(id);
            if (reserva.GetCanceledValue())
                return BadRequest(new ReservationResultDTO(new InvalidDataExeception("esta reserva já foi finalizada")).GetErrors());
            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer")
                if (ValidarCustomer(reserva.Client.UserId)) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("O clietne atual não pode acessar ")).GetErrors());
            var result = _reservationService.CancelReservation(id);
            if (result.Error == false)
                return Ok(new ReservationResultDTO(result.CreatedObj));
            return BadRequest(new ReservationResultDTO(result.Exception).GetErrors());
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
            if (idAuthor != Guid.Empty)
                author = _authorService.GetAuthorById(idAuthor);
            else
                author = null;
            List<ReservationResultDTO> results = new List<ReservationResultDTO>();
            var reservations = _reservationService.GetReservationsByParams(startDate, endDate, author, bookName, page, items);
            reservations.ToList().ForEach(reservation => results.Add(new ReservationResultDTO(reservation)));
            return Ok(results);
        }

        [HttpPut, Route("{id}")]
        public override IActionResult Update(Guid id, CreateReservationDTO dto)
        {
            dto.ValidUpdate();
            if (!dto.UpdateValid) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("Dados Invalidos")));

            var customer = _customerService.GetUserById(dto.idCustumer);

            if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value.ToLower() == "customer")
                if (ValidarCustomer(customer.UserId)) return BadRequest(new ReservationResultDTO(new InvalidDataExeception("O cliente não pode alterar a reserva de outro cliente")).GetErrors());


            List<Book> Books = new List<Book>();
            dto.IdBooks.ForEach(x => Books.Add(_bookService.GetBookById(x)));



            var reservation = new Reservation(
                id: id,
                client: customer,
                startDate: dto.StartDate,
                endDate: dto.EndDate,
                books: Books
                );
            var result = _reservationService.UpdateReservation(id, reservation);
            if (result.Error==false)
                return Ok(new ReservationResultDTO(result.CreatedObj));
            return BadRequest(new ReservationResultDTO(result.Exception).GetErrors());
        }
        [HttpGet, Route("LoggedUser")]
        public IActionResult GetReservationsByLoggedUser()
        {
            Guid idLogged = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
            var reservations = _reservationService.GetAllReservations().Where(x => x.Client.UserId == idLogged);
            if (reservations is not null)
            {
                List<ReservationResultDTO> reservationResultDTOs = new List<ReservationResultDTO>();
                foreach (var reserv in reservations)
                {
                    var newResult = new ReservationResultDTO(reserv);
                }
                return Ok(reservationResultDTOs);

            }
            return NotFound();
        }
    }
}
