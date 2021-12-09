using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Services
{
    public class UserService 
    {
        private readonly UserRepository _userRepository;
        private readonly ClientRepository _clientRepository;
        public UserService(BaseRepository<User> repository) 
        {
            _userRepository = (UserRepository)repository;
            
        }

        public User AddUser(User user)
        {
            return _userRepository.Add(user);
        }
        public IEnumerable<User> GetAllUsersWithParams(string? Name ,string? document, DateTime? Birthdate, int page,int items)
        {
            var clients = _clientRepository.GetAll();
            if (Name is not null)
                clients = clients.Where(x => x.Name == Name).ToList();
            if (document is not null)
                clients = clients.Where(x => x.Document == document).ToList();
            if (document is not null)
                clients = clients.Where(x => x.BirthDate == Birthdate).ToList();
            if (page !=0 && items !=0)
                clients = clients.Skip((page - 1) * items).Take(items).ToList();

            var users = clients.Select(x => x.User);

            return users;
        }
        //public User GetLoggedUser()
        //{

        //}

        

        public User GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User UpdateUser(Guid id,User user)
        {
            return _userRepository.Update(id, user);
        }



    }
}
