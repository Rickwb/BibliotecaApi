using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController,Authorize]
    public class ClientController : BaseControl<CreateClientDTO, User>
    {
        private readonly UserService _userService;
        public ClientController(UserService userService) 
        {
            _userService = userService;
        }
        [HttpPost,AllowAnonymous,Route("/users")]
        public override IActionResult Add(CreateClientDTO userDto)
        {
            userDto.Valid();
            if (!userDto.IsValid)
                return BadRequest("Não foi possivel adicionar");

            var userAdd = new User(
                username: userDto.Username,
                password: userDto.Password,
                role: userDto.Role
                );

            return Ok(_userService.AddUser(userAdd));
        }
        [HttpGet,Authorize,Route("clients/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_userService.GetUserById(id)); 
        }

        public override IActionResult GetAll()
        {
            throw new NotImplementedException();
        }
        public IActionResult GetLoggedUser()
        {
            throw new NotImplementedException();
        }

        [HttpGet,Authorize,Route("clients")]
        public IActionResult GetAllByParams([FromQuery]string Name,[FromQuery]string  document,[FromQuery]DateTime Birthdate, [FromQuery]int page, [FromQuery]int items )
        {
            return Ok(_userService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }

        public override IActionResult Remove(CreateClientDTO t)
        {
            throw new NotImplementedException();
        }

        public override IActionResult RemoveById(Guid id)
        {
            throw new NotImplementedException();
        }
        [HttpPut,Authorize,Route("/users/{id}")]
        public override IActionResult Update(Guid id,[FromBody] CreateClientDTO userDto)
        {
            userDto.Valid();
            if (!userDto.IsValid)
                return BadRequest("Não foi possivel adicionar");

            var userUp = new User(
                username: userDto.Username,
                password: userDto.Password,
                role: userDto.Role
                );

            return Ok(_userService.UpdateUser(id,userUp));
        }
    }
}
