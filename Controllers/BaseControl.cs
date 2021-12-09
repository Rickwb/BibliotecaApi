using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    public abstract class BaseControl<_DTO,_Entity> : ControllerBase where _DTO : BaseDTO where _Entity : BaseEntity  
    {
        public abstract IActionResult Add(_DTO dto);
        public abstract IActionResult Get(Guid id);
        public abstract IActionResult GetAll();
        public abstract IActionResult Remove(_DTO t);
        public abstract IActionResult Update(Guid id, _DTO dto);
        public abstract IActionResult RemoveById(Guid id);

    }
}
