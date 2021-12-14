using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class ReservationController : BaseControl<CreateReservationDTO, Reservation>
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public override IActionResult Add(CreateReservationDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();
            

            _reservationService.AddReservation();
        }
        [HttpPost, Route("finalize/{id}")]
        public IActionResult FinalzeReservation(Guid id)
        {
            
        }
        [HttpPost, Route("cancel/{id}")]
        public IActionResult CancelReservation(Guid id)
        {
            
        }


        [HttpGet,Route("{id}")]
        public override IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }
        [HttpGet,Route("params")]
        public IActionResult GetReservationByParams([FromQuery]DateTime? startDate, [FromQuery] DateTime? endDate,[FromQuery] Guid? idAuthor,
            [FromQuery]string? bookName, [FromQuery] int? page, [FromQuery] int? items)
        {
            return 
        }
        public override IActionResult Update(Guid id, CreateReservationDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
