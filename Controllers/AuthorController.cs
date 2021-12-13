using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class AuthorController : BaseControl<CreateAuthorDTO, Authors>
    {
        private readonly BookAuthorService _bookAuthorService;

        public AuthorController(BookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }

        [HttpPost, Route("addAuthor")]
        public override IActionResult Add([FromBody] CreateAuthorDTO dtoAuthor)
        {
            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest();

            var author = new Authors(
                name: dtoAuthor.Name,
                nacionality: dtoAuthor.Nacionality,
                age: dtoAuthor.Age);

            return Ok(_bookAuthorService.AddAuthors(author));
        }
        [HttpGet,Route("getAuthor/{id}")]
        public override IActionResult Get(Guid id) => Ok(_bookAuthorService.GetAuthorById(id));

        [HttpPut,Route("update/{id}")]
        public override IActionResult Update(Guid id,[FromBody]CreateAuthorDTO dtoAuthor)
        {
            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest();

            var author = new Authors(
                name: dtoAuthor.Name,
                nacionality: dtoAuthor.Nacionality,
                age: dtoAuthor.Age,
                books: dtoAuthor.AuthorBooks);

            return Ok(_bookAuthorService.UpdateAuthors(id, author));
        }
        [HttpDelete,Route("deleteAuthor/{id}")]
        public IActionResult DeleteAuthor(Guid idAuthor)
        {
            bool deletado = _bookAuthorService.DeleteAuthor(idAuthor);
            if (!deletado) return BadRequest();

            return NoContent();
        }
        [HttpGet,Route("authors")]
        public IActionResult GetAuthorsByParams([FromQuery] string name, [FromQuery] string nacionality, [FromQuery]int age, [FromQuery]int page, [FromQuery]int items)
        {
            return Ok(_bookAuthorService.GetAuthorsByParams(name, nacionality, age, page, items).ToList());
        }
    }
}
