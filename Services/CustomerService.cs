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
        public CustomerService(UserRepository userRepository,CustomerRepository clientRepository,CepService cepService)
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

        public IEnumerable<Customer> GetAllUsersWithParams(string? Name, string? document, DateTime? Birthdate, int page, int items)
        {
            var clients = _clientRepository.GetAll();

            if (Name is not null)
                clients = clients.Where(x => x.Name == Name).ToList();
            if (document is not null)
                clients = clients.Where(x => x.Document == document).ToList();
            if (document is not null)
                clients = clients.Where(x => x.BirthDate == Birthdate).ToList();
            if (page != 0 && items != 0)
                clients = clients.Skip((page - 1) * items).Take(items).ToList();

            return clients;
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
            return _clientRepository.Update(id, client);
        }

        public User UpdateUserFromClient(Guid id, User user)
        {
            Guid userId = _clientRepository.GetById(id).UserId;
            var oldUser = _userRepository.GetById(userId);

            return _userRepository.Update(userId, user);
        }



    }
}
