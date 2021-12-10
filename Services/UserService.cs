using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;

namespace BibliotecaApi.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly TokenService _tokenService;
        public UserService(UserRepository userRepository,TokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public LoginResultDTO Login(string username, string password)
        {
            var loginResult = _userRepository.Login(username, password);

            if (loginResult.Error)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Ocorreu um erro ao autenticar: {loginResult.Exception?.Message}" }
                };
            }

            var token = _tokenService.GenerateToken(loginResult.User);

            return new LoginResultDTO
            {
                Success = true,
                User = new UserLoginResultDTO
                {
                    Token = token,
                    Id = loginResult.User.Id,
                    Role = loginResult.User.Role,
                    Username = loginResult.User.Username
                }
            };
        }

        public bool ResetPassword(string username, string oldPassword, string newPassword)
        {
            return _userRepository.ResetPassword(username, oldPassword, newPassword);
        }
    }
}
