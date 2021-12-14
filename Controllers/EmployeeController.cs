using BibliotecaApi.DTOs;
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

        [HttpPost,Route("AddEmployee")]
        public override IActionResult Add([FromBody]CreateEmployeeDTO createEmplDto)
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
                id:Guid.NewGuid(),
                name: createEmplDto.Name,
                document: createEmplDto.Document,
                cep: createEmplDto.Cep,
                role: createEmplDto.Role,
                userId: userAdd.Id
                );

            return Created("",_employeeService.AddEmployee(employee,userAdd));
        }

    
        [HttpGet,Route("employee/{id}")]
        public override IActionResult Get(Guid id)
        {
            return Ok(_employeeService.GetUserById(id));
        }

     
        [HttpGet,  Route("GetEmployeesByParams")]
        public IActionResult GetAllByParams([FromQuery] string? Name, [FromQuery] string? document, [FromQuery] DateTime? Birthdate, [FromQuery] int page, [FromQuery] int items)
        {
            return Ok(_employeeService.GetAllUsersWithParams(Name, document, Birthdate, page, items));
        }

      

        [HttpPut,Route("{id}")]
        public override IActionResult Update(Guid id, CreateEmployeeDTO dto)
        {
            dto.Valid();
            if (dto.IsValid) return BadRequest();

            var employee = new Employee(
                id:id,
                name: dto.Name,
                document: dto.Document,
                cep: dto.Cep,
                role: dto.Role
                );
            

            return Ok(_employeeService.UpdateEmployee(id,employee)) ;
        }
        [HttpPut,Route("UpdateUserfromEmplyee")]
        public  IActionResult UpdateUserFromEmployee(Guid id, CreateEmployeeDTO dto)
        {
            throw new NotImplementedException();
        }
    }

}
