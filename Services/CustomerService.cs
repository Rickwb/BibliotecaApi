using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class CustomerService
    {
        private readonly UserRepository _userRepository;
        private readonly CustomerRepository _clientRepository;
        private readonly CepService _cepService;
        public CustomerService(UserRepository userRepository, CustomerRepository clientRepository, CepService cepService)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _cepService = cepService;

        }

        public Customer AddClient(Customer customer, User user)
        {
            customer.Adress = _cepService.BuscarEnderecosAsync(customer.Cep).Result;
            _userRepository.Add(user);
            return _clientRepository.Add(customer);
        }

        public List<Customer> GetAllUsersWithParams(string Name, string document, DateTime? Birthdate, int page, int items)
        {
            return _clientRepository.GetAllCustomersWithParams(Name, document, Birthdate, page, items);
        }

        //public User GetLoggedUser()
        //{

        //}

        public Customer GetUserById(Guid id)
        {
            return _clientRepository.GetById(id);
        }

        public Customer UpdateClient(Guid id, Customer client)
        {
            var customer=_clientRepository.GetById(id);
            customer.Name= client.Name;
            customer.Document= client.Document;
            customer.Cep= client.Cep;
            return _clientRepository.Update(id, customer);
        }

        public User UpdateUserFromClient(Guid id, User user)
        {
            Guid userId = _clientRepository.GetById(id).UserId;
            var oldUser = _userRepository.GetById(userId);

            return _userRepository.Update(userId, user);
        }



    }
}
