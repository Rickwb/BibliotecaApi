using BibliotecaApi.DTOs;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BibliotecaApi.Controllers
{
    [ApiController,Route("controller")]
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
           _userService= userService;
           
        }

        [HttpPost,Route("user/Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            return Ok(_userService.Login(loginDTO.Username, loginDTO.Password));
        }

        [HttpPut,Route("/user/resetPassword")]
        public async Task<IActionResult> ChangePassword(UserLoginDTO loginDTO,[FromQuery] string newPassword)
        {
            if (_userService.ResetPassword(loginDTO.Username, loginDTO.Password, newPassword)) return Ok(true);

            return BadRequest();
        }
    }
}
