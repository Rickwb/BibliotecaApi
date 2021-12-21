using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class EmployeeService
    {
        private readonly UserRepository _userRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly CepService _cepService;

        public EmployeeService(UserRepository userRepository, EmployeeRepository employeeRepository, CepService cepService)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _cepService = cepService;
        }

        public CreateResult<Employee> AddEmployee(Employee employee, User user)
        {
            employee.Adress = _cepService.BuscarEnderecosAsync(employee.Cep).Result;
            _userRepository.Add(user);
            if (string.IsNullOrEmpty(user.Role))
                return CreateResult<Employee>.Errors(new InvalidDataExeception("O campo role está nulo"));
            if (user.Role.ToLower() != "employee" && user.Role.ToLower() != "admin")
                return CreateResult<Employee>.Errors(new InvalidDataExeception("Este cargo não existe"));
            if (_employeeRepository.GetAll().Where(x => x.Document == employee.Document).SingleOrDefault() != null)
                return CreateResult<Employee>.Errors(new SameObjectExeception("Este funcionário já está cadastrado"));

            _employeeRepository.Add(employee);
            return CreateResult<Employee>.Sucess(employee);
        }

        public IEnumerable<Employee> GetAllUsersWithParams(string? Name, string? document, DateTime? Birthdate, int page, int items)
        {
            return _employeeRepository.GetAllEmployeesWithParams(Name, document, Birthdate, page, items);
        }

        public Employee GetUserById(Guid id)
        {
            return _employeeRepository.GetById(id);
        }

        public CreateResult<Employee> UpdateEmployee(Guid idEmployee, Employee employee)
        {

            employee.Adress = _cepService.BuscarEnderecosAsync(employee.Cep).Result;
            var updatedEmployee = _employeeRepository.Update(idEmployee, employee);
            if (updatedEmployee is not null)
                return CreateResult<Employee>.Sucess(updatedEmployee);

            return CreateResult<Employee>.Errors(new CreationException("Não foi possivel atualizar"));
        }

        public CreateResult<User> UpdateUserFromClient(Guid idEmployee, User user)
        {
            Guid userId = _employeeRepository.GetById(idEmployee).UserId;
            var oldUser = _userRepository.GetById(userId);
            var updatedUser= _userRepository.Update(userId, user);
            if (updatedUser is not null)
                return CreateResult<User>.Sucess(updatedUser);
            return CreateResult<User>.Errors(new CreationException("O usuario não pode ser alterado"));
        }
    }
}
