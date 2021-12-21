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
    public class EmployeeController : BaseControl<CreateEmployeeDTO, Employee>
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost, Route("AddEmployee")]
        public override IActionResult Add([FromBody] CreateEmployeeDTO createEmplDto)
        {
            createEmplDto.Valid();
            if (!createEmplDto.IsValid)
                return BadRequest(new AuthorResultDTO(new InvalidDataExeception("Dados invalidos para o cadastro")));


            var userAdd = new User(
                username: createEmplDto.Username,
                password: createEmplDto.Password,
                role: createEmplDto.Role
                );
            Employee employee;
            if (String.IsNullOrEmpty(createEmplDto.Cep) || createEmplDto.Cep == "strig")
            {
                var adressAdd = new Adress
                   (
                   cep: createEmplDto.Adress.Cep,
                   logradouro: createEmplDto.Adress.Logradouro,
                   complemento: createEmplDto.Adress.Complemento,
                   bairro: createEmplDto.Adress.Bairro,
                   localidade: createEmplDto.Adress.Localidade,
                   uf: createEmplDto.Adress.Uf,
                   ibge: createEmplDto.Adress.Ibge,
                   gia: createEmplDto.Adress.Gia,
                   ddd: createEmplDto.Adress.Ddd,
                   siafi: createEmplDto.Adress.Siafi
                   );
                employee = new Employee
               (
               id: Guid.NewGuid(),
               name: createEmplDto.Name,
               document: createEmplDto.Document,
               age: createEmplDto.Age,
               adress: adressAdd,
               role: createEmplDto.Role,
               birthDate: createEmplDto.BirthDate,
               userId: userAdd.Id
               );
            }
            else
            {

                employee = new Employee
                (
                id: Guid.NewGuid(),
                name: createEmplDto.Name,
                document: createEmplDto.Document,
                age: createEmplDto.Age,
                cep: createEmplDto.Cep,
                role: createEmplDto.Role,
                birthDate: createEmplDto.BirthDate,
                userId: userAdd.Id
                );
            }

            var result = _employeeService.AddEmployee(employee, userAdd);

            if (result.Error == false)
            {
                var employeeResult = new EmployeeResultDTO(employee);
                return Created("", employeeResult);
            }

            return BadRequest(new EmployeeResultDTO(result.Exception).GetErrors());
        }

        [HttpGet, Route("employee/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_employeeService.GetUserById(id));
        }

        [HttpGet, Route("Autenticathed")]
        public string Autenticated() => $"{User.Identity.Name} | {User.Claims.First(c => c.Type == ClaimTypes.Sid)} está autenticado.";

        [HttpGet, Route("GetEmployeesByParams"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetAllByParams([FromQuery] string? Name, [FromQuery] string? document, [FromQuery] DateTime? Birthdate, [FromQuery] int page = 1, [FromQuery] int items = 5)
        {
            return Ok(_employeeService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }

        [HttpPut, Route("{id}")]
        public override IActionResult Update(Guid id, CreateEmployeeDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest(new AuthorResultDTO(new InvalidDataExeception("Dados invalidos para o cadastro")));

            var oldEmployee = _employeeService.GetUserById(id);
            var employee = new Employee(
                id: id,
                name: dto.Name,
                document: dto.Document,
                age: dto.Age,
                cep: dto.Cep,
                role: dto.Role,
                birthDate: dto.BirthDate,
                userId: oldEmployee.UserId
                );
            var employer = _employeeService.UpdateEmployee(id, employee);

            return employer.Error == false ? Ok(new EmployeeResultDTO(employer.CreatedObj)) : BadRequest(new EmployeeResultDTO(new InvalidDataExeception("Não foi possivel cadastrar")));
        }

        [HttpPut, Route("UpdateUserfromEmplyee")]
        public IActionResult UpdateUserFromEmployee(Guid id, CreateUserDTO userDTO)
        {
            userDTO.Valid();
            if (!userDTO.IsValid)
                return BadRequest(new UserResultDTO(new InvalidDataExeception("Dados invalidos para o cadastro")));

            var user = new User(
                username: userDTO.Username,
                password: userDTO.Password,
                role: userDTO.Role
                );
            var result = _employeeService.UpdateUserFromClient(id, user);

            return result.Error == false ? Ok(new UserResultDTO(result.CreatedObj)) : BadRequest(new UserResultDTO(new CreationException("Não foi possivel atualizar o usuario")));
        }
    }
}
