using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class CustomerController : BaseControl<CreateCustomerDTO, User>
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost, AllowAnonymous, Route("createClient")]
        public override IActionResult Add([FromBody] CreateCustomerDTO createClientDto)
        {
            createClientDto.Valid();
            if (!createClientDto.IsValid)
                return BadRequest("Não foi possivel adicionar o seu cadastro");

            var userAdd = new User(
                username: createClientDto.Username,
                password: createClientDto.Password,
                role: createClientDto.Role
                )
            {
                Role = "costumer",
            };

            var customerAdd = new Customer(
                id: Guid.NewGuid(),
                name: createClientDto.Name,
                document: createClientDto.Document,
                cep: createClientDto.Cep,
                userId: userAdd.Id,
                birthdate: createClientDto.Birtdate
                ) ;

            var result = _customerService.AddClient(customerAdd, userAdd);

            if (result.Error == false)
            {
                var customerResult = new CustomerResultDTO(customerAdd);
                return Ok(customerResult);
            }

            var custmerResult = new CustomerResultDTO(result.Exception);
            return Ok(custmerResult.GetErros());
        }

        [HttpGet, Authorize(Roles = "admin,employee"), Route("clients/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_customerService.GetUserById(id));
        }

        [HttpGet, Authorize(Roles = "admin,employee"), Route("clients"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetAllByParams([FromQuery] string? Name, [FromQuery] string? document, [FromQuery] DateTime? Birthdate, [FromQuery] int page=1, [FromQuery] int items=5)
        {
            return Ok(_customerService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }

        [HttpPut, Authorize(Roles = "admin,employee"), Route("clientUpdate/{id}")]
        public override IActionResult Update(Guid id, [FromBody] CreateCustomerDTO customerDTO)
        {
            customerDTO.Valid();
            if (!customerDTO.IsValid)
                return BadRequest("Não foi possivel atualizar o seu cadastro");

            var oldcustomer=_customerService.GetUserById(id);
            var customerAdd = new Customer(
                id: id,
                name: customerDTO.Name,
                document: customerDTO.Document,
                cep: customerDTO.Cep,
                birthdate: oldcustomer.BirthDate,
                reservations: oldcustomer.Reservations.ToList(),
                withdraws: oldcustomer.Withdraws.ToList()
                );

            return Ok(_customerService.UpdateClient(id, customerAdd));
        }

        [HttpGet, Authorize, Route("userLogged")]
        public string Autorizado() => $"autenticado {User.Identity.Name}";

        [HttpPut, Authorize(Roles = "admin,employee"), Route("clientsUserUpdate/{id}")]
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

            return Ok(_customerService.UpdateUserFromClient(idClient, user));
        }
    }
}

