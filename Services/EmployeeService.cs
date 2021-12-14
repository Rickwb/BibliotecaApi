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

        public Employee AddEmployee(Employee employee, User user)
        {
            employee.Adress = _cepService.BuscarEnderecosAsync(employee.Cep).Result;
            _userRepository.Add(user);
            return _employeeRepository.Add(employee);
        }


        public IEnumerable<Employee> GetAllUsersWithParams(string? Name, string? document, DateTime? Birthdate, int page, int items)
        {
            return _employeeRepository.GetAllEmployeesWithParams(Name, document, Birthdate, page, items);
        }

        public Employee GetUserById(Guid id)
        {
            return _employeeRepository.GetById(id);
        }

        public Employee UpdateEmployee(Guid idEmployee, Employee employee)
        {
            return _employeeRepository.Update(idEmployee, employee);
        }

        public User UpdateUserFromClient(Guid idEmployee, User user)
        {
            Guid userId = _employeeRepository.GetById(idEmployee).UserId;
            var oldUser = _userRepository.GetById(userId);

            return _userRepository.Update(userId, user);
        }
    }
}
