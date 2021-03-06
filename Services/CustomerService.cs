using BibliotecaApi.Repositories;
using Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class CustomerService
    {
        private readonly UserRepository _userRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly CepService _cepService;
        public CustomerService(UserRepository userRepository, CustomerRepository customerRepository, CepService cepService)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _cepService = cepService;

        }

        public CreateResult<Customer> AddClient(Customer customer, User user)
        {
            if (customer.Cep is not null)
            {
                customer.Adress = _cepService.BuscarEnderecosAsync(customer.Cep).Result;
                if (customer.Adress == null)
                {
                    return CreateResult<Customer>.Errors(new CreationException("Não foi possivel validar o cep, adicione o seu endereço manualmente"));
                }
            }
            _userRepository.Add(user);
            if (_customerRepository.GetAll().SingleOrDefault(d => d.Document == customer.Document) != null)
            {
                return CreateResult<Customer>.Errors(new SameObjectExeception("Já existe um cliente cadastrado com esse documento"));
            }
            _customerRepository.Add(customer);
            return CreateResult<Customer>.Sucess(customer);
        }

        public List<Customer> GetAllUsersWithParams(string Name, string document, DateTime? Birthdate, int page = 1, int items = 5)
        {
            return _customerRepository.GetAllCustomersWithParams(Name, document, Birthdate, page, items);
        }


        public Customer GetUserById(Guid id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer UpdateClient(Guid id, Customer custom)
        {
            var customer = _customerRepository.GetById(id);
            var Adress = _cepService.BuscarEnderecosAsync(customer.Cep).Result;

            customer.PropretiesUpdate(custom.Name, custom.Document, custom.Cep, Adress);


            return _customerRepository.Update(id, customer);
        }
        public User UpdateUserFromClient(Guid id, User user)
        {
            Guid userId = _customerRepository.GetById(id).UserId;
            var oldUser = _userRepository.GetById(userId);

            return _userRepository.Update(userId, user);
        }
        public IEnumerable<Customer> GetAllCustomers() => _customerRepository.GetAll();
    }
}
