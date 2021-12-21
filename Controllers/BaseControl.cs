using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    public abstract class BaseControl<_DTO,_Entity> : ControllerBase where _DTO : BaseDTO where _Entity : BaseEntity  
    {
        private UserService _userService;

        protected virtual bool ValidarCustomer(Guid userId)
        {
            if (userId.ToString()==User.Claims.First(c=>c.Type==ClaimTypes.Sid).Value)
                return true;

            return false;
        }

        public abstract IActionResult Add(_DTO dto);
     
        public abstract IActionResult Get(Guid id);

        public abstract IActionResult Update(Guid id, _DTO dto);
       

    }
}
