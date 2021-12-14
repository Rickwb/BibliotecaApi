using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class ReservationController : BaseControl<CreateReservationDTO, Reservation>
    {
        [HttpPost]
        public override IActionResult Add(CreateReservationDTO dto)
        {
            throw new NotImplementedException();
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
        { }
        public override IActionResult Update(Guid id, CreateReservationDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
