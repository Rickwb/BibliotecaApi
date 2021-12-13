using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Controllers
{
    public class AuthorController : BaseControl<CreateAuthorDTO, Authors>
    {
        private readonly BookAuthorService _bookAuthorService;

        public AuthorController(BookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }


        public override IActionResult Add(CreateAuthorDTO dto)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Update(Guid id, CreateAuthorDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
