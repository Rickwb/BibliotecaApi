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
                return BadRequest(new CustomerResultDTO(new CreationException("não foi possivel adicionar o seu cadastro")).GetErrors());

            var userAdd = new User(
                username: createClientDto.Username,
                password: createClientDto.Password,
                role: createClientDto.Role
                )
            {
                Role = "costumer",
            };
            Customer customerAdd;
            if (String.IsNullOrWhiteSpace(createClientDto.Cep) || createClientDto.Cep == "string")
            {
                var adressAdd = new Adress
                    (
                    cep: createClientDto.Adress.Cep,
                    logradouro: createClientDto.Adress.Logradouro,
                    complemento: createClientDto.Adress.Complemento,
                    bairro: createClientDto.Adress.Bairro,
                    localidade: createClientDto.Adress.Localidade,
                    uf: createClientDto.Adress.Uf,
                    ibge: createClientDto.Adress.Ibge,
                    gia: createClientDto.Adress.Gia,
                    ddd: createClientDto.Adress.Ddd,
                    siafi: createClientDto.Adress.Siafi
                    );

                customerAdd = new Customer(
                id: Guid.NewGuid(),
                name: createClientDto.Name,
                document: createClientDto.Document,
                age: createClientDto.Age,
                adress: adressAdd,
                userId: userAdd.Id,
                birthdate: createClientDto.Birtdate
                );
            }
            else
            {

                customerAdd = new Customer(
                id: Guid.NewGuid(),
                name: createClientDto.Name,
                age: createClientDto.Age,
                document: createClientDto.Document,
                cep: createClientDto.Cep,
                userId: userAdd.Id,
                birthdate: createClientDto.Birtdate
                );
            }

            var result = _customerService.AddClient(customerAdd, userAdd);

            if (result.Error == false)
            {
                var customerResult = new CustomerResultDTO(customerAdd);
                return Ok(customerResult);
            }

            var custmerResult = new CustomerResultDTO(result.Exception);
            return Ok(custmerResult.GetErrors());
        }

        [HttpGet, Authorize(Roles = "admin,employee"), Route("clients/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(new CustomerResultDTO(_customerService.GetUserById(id)));
        }

        [HttpGet, Authorize(Roles = "admin,employee"), Route("clients"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetAllByParams([FromQuery] string? Name, [FromQuery] string? document, [FromQuery] DateTime? Birthdate, [FromQuery] int page = 1, [FromQuery] int items = 5)
        {
            List<CustomerResultDTO> results = new List<CustomerResultDTO>();
            var customers = _customerService.GetAllUsersWithParams(Name, document, Birthdate, page, items);

            customers.ForEach(customer => results.Add(new CustomerResultDTO(customer)));
            return Ok(results);
        }

        [HttpPut, Authorize(Roles = "admin,employee"), Route("clientUpdate/{id}")]
        public override IActionResult Update(Guid id, [FromBody] CreateCustomerDTO customerDTO)
        {
            customerDTO.Valid();
            if (!customerDTO.IsValid)
                return BadRequest(new CustomerResultDTO(new InvalidDataExeception("Não foi possivel atualizar o seu cadastro")).GetErrors());

            var oldcustomer = _customerService.GetUserById(id);
            var customerAdd = new Customer(
                id: id,
                name: customerDTO.Name,
                document: customerDTO.Document,
                cep: customerDTO.Cep,
                age: customerDTO.Age,
                birthdate: oldcustomer.BirthDate,
                reservations: oldcustomer.Reservations.ToList(),
                withdraws: oldcustomer.Withdraws.ToList()
                );

            return Ok(_customerService.UpdateClient(id, customerAdd));
        }

        [HttpGet, Authorize, Route("userLogged")]
        public string Autorizado() => $"{User.Identity.Name} | {User.Claims.First(c => c.Type == ClaimTypes.Sid)} está autenticado.";

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

