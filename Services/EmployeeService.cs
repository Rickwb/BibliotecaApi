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

        public EmployeeService(UserRepository userRepository,EmployeeRepository employeeRepository,CepService cepService)
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
            var employee = _employeeRepository.GetAll();

            if (Name is not null)
                employee = employee.Where(x => x.Name == Name).ToList();
            if (document is not null)
                employee = employee.Where(x => x.Document == document).ToList();
            if (document is not null)
                employee = employee.Where(x => x.BirthDate == Birthdate).ToList();
            if (page != 0 && items != 0)
                employee = employee.Skip((page - 1) * items).Take(items).ToList();

            return employee;
        }

        //public User GetLoggedUser()
        //{

        //}

        public Employee GetUserById(Guid id)
        {
            return _employeeRepository.GetById(id);
        }

        public Employee UpdateClient(Guid idEmployee, Employee employee)
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
