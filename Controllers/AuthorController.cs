using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
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

        [HttpPost, Route("addAuthor"), Authorize(Roles = "admin,employee")]
        public override IActionResult Add([FromBody] CreateAuthorDTO dtoAuthor)
        {
            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest();

            var author = new Authors(
                id: Guid.NewGuid(),
                name: dtoAuthor.Name,
                nacionality: dtoAuthor.Nacionality,
                age: dtoAuthor.Age);
            var result = _bookAuthorService.AddAuthors(author);
            if (result.Error == false)
            {
                var authorResult= new AuthorResultDTO(author);
                return Ok(authorResult);

            }
            else
            {
                var authorResult = new AuthorResultDTO(result.Exception);
                return BadRequest(authorResult.GetErrors());
            }
        }

        [HttpGet, Route("getAuthor/{id}")]
        public override IActionResult Get(Guid id) => Ok(_bookAuthorService.GetAuthorById(id));

        [HttpPut, Route("update/{id}"), Authorize(Roles = "admin,employee")]
        public override IActionResult Update(Guid id, [FromBody] CreateAuthorDTO dtoAuthor)
        {
            List<Book> books=new List<Book>();
             dtoAuthor.AuthorBooks.ForEach(b => books.Add(_bookAuthorService.GetBookById(b)));
            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest();

            var author = new Authors(
                id: id,
                name: dtoAuthor.Name,
                nacionality: dtoAuthor.Nacionality,
                age: dtoAuthor.Age,
                books: books);

            return Ok(_bookAuthorService.UpdateAuthors(id, author));
        }
        [HttpDelete, Route("deleteAuthor/{id}"), Authorize(Roles = "admin,employee")]
        public IActionResult DeleteAuthor(Guid id)
        {
            bool deletado = _bookAuthorService.DeleteAuthor(id);
            if (!deletado) return BadRequest();

            return NoContent();
        }
        [HttpGet, Route("authors")]
        public IActionResult GetAuthorsByParams([FromQuery] string? name, [FromQuery] string? nacionality, [FromQuery] int? age, [FromQuery] int page, [FromQuery] int items)
        {
            return Ok(_bookAuthorService.GetAuthorsByParams(name, nacionality, age, page, items).ToList());
        }
    }
}
