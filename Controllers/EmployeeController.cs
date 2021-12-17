using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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
                return BadRequest("Dados do funcionario estão inválidos");

            var userAdd = new User(
                username: createEmplDto.Username,
                password: createEmplDto.Password,
                role: createEmplDto.Role
                );

            var employee = new Employee
                (
                id: Guid.NewGuid(),
                name: createEmplDto.Name,
                document: createEmplDto.Document,
                age: createEmplDto.Age,
                cep: createEmplDto.Cep,
                role: createEmplDto.Role,
                userId: userAdd.Id
                );

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
        public string Autenticated() => $"autenticado {User.Identity.Name}";

        [HttpGet, Route("GetEmployeesByParams"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetAllByParams([FromQuery] string? Name, [FromQuery] string? document, [FromQuery] DateTime? Birthdate, [FromQuery] int page=1, [FromQuery] int items=5)
        {
            return Ok(_employeeService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }

        [HttpPut, Route("{id}")]
        public override IActionResult Update(Guid id, CreateEmployeeDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest();

            var oldEmployee = _employeeService.GetUserById(id);
            var employee = new Employee(
                id: id,
                name: dto.Name,
                document: dto.Document,
                age: dto.Age,
                cep: dto.Cep,
                role: dto.Role,
                userId:oldEmployee.UserId
                );

            return Ok(_employeeService.UpdateEmployee(id, employee));
        }

        [HttpPut, Route("UpdateUserfromEmplyee")]
        public IActionResult UpdateUserFromEmployee(Guid id, CreateUserDTO userDTO)
        {
            userDTO.Valid();
            if (!userDTO.IsValid)
                return BadRequest("Não foi possivel atualizar o seu cadastro");

            var user = new User(
                username: userDTO.Username,
                password: userDTO.Password,
                role: "customer"
                );

            return Ok(_employeeService.UpdateUserFromClient(id, user));
        }
    }
}
