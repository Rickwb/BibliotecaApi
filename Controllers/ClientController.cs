using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class ClientController : BaseControl<CreateCustomerDTO, User>
    {
        private readonly CustomerService _clientService;
        public ClientController(CustomerService clientService)
        {
            _clientService = clientService;
        }
        [HttpPost, AllowAnonymous, Route("createClient")]
        public override IActionResult Add([FromBody] CreateCustomerDTO createClientDto)
        {
            createClientDto.Valid();
            if (!createClientDto.IsValid)
                return BadRequest("Não foi possivel adicionar o seu cadastro");

            var customerAdd = new User(
                username: createClientDto.Username,
                password: createClientDto.Password,
                role: createClientDto.Role
                )
            {
                Role = "costumer",
            };

            var clientAdd = new Customer(
                name: createClientDto.Name,
                document: createClientDto.Document,
                cep: createClientDto.Cep,
                userId: customerAdd.Id
                );

            return Ok(_clientService.AddClient(clientAdd, customerAdd));
        }
        [HttpGet, Authorize, Route("clients/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_clientService.GetUserById(id));
        }

    

        [HttpGet, Authorize, Route("clients")]
        public IActionResult GetAllByParams([FromQuery] string Name, [FromQuery] string document, [FromQuery] DateTime Birthdate, [FromQuery] int page, [FromQuery] int items)
        {
            return Ok(_clientService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }
      

      
        [HttpPut, Authorize, Route("clientUpdate/{id}")]
        public  override IActionResult Update(Guid id, [FromBody] CreateCustomerDTO clientDTO)
        {
            clientDTO.Valid();
            if (!clientDTO.IsValid)
                return BadRequest("Não foi possivel atualizar o seu cadastro");


            var customerAdd = new Customer(
                name: clientDTO.Name,
                document: clientDTO.Document,
                cep: clientDTO.Cep
                );


            return Ok(_clientService.UpdateClient(id, customerAdd));
        }

        [HttpPut, Authorize, Route("clientsUserUpdate/{id}")]
        public IActionResult UpdateUserFromClient(Guid idClient, [FromBody] CreateUserDTO userDTO)
        {
            userDTO.Valid();
            if (!userDTO.IsValid)
                return BadRequest("Não foi possivel atualizar o seu cadastro");

            var user = new User(
                username: userDTO.Username,
                password: userDTO.Password,
                role: "customer"
                );

            return Ok(_clientService.UpdateUserFromClient(idClient, user));
        }

       
      

    }
}

