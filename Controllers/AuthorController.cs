using BibliotecaApi.DTOs;
using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Services;
using Domain.Enities;
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
        private readonly AuthorService _authorService;
        private readonly BookService _bookService;

        public AuthorController(AuthorService authorService, BookService bookAuthorService)
        {
            _authorService = authorService;
            _bookService = bookAuthorService;
        }

        [HttpPost, Route("addAuthor"), Authorize(Roles = "admin,employee")]
        public override IActionResult Add([FromBody] CreateAuthorDTO dtoAuthor)
        {

            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest();

            Authors author;
            if (dtoAuthor.AuthorBooks == null)
            {
                author = new Authors(
                   id: Guid.NewGuid(),
                   name: dtoAuthor.Name,
                   nacionality: dtoAuthor.Nacionality,
                   age: dtoAuthor.Age);
            }
            else
            {
                List<Book> books = new List<Book>();
                dtoAuthor.AuthorBooks.ForEach(i => books.Add(_bookService.GetBookById(i)));
                author = new Authors(
                   id: Guid.NewGuid(),
                   name: dtoAuthor.Name,
                   nacionality: dtoAuthor.Nacionality,
                   age: dtoAuthor.Age,
                   books: books);
            }

            var result = _authorService.AddAuthors(author);

            if (result.Error == false)
            {
                var authorResult = new AuthorResultDTO(author);
                return Ok(authorResult);
            }
            else
            {
                var authorResult = new AuthorResultDTO(result.Exception);
                return BadRequest(authorResult.GetErrors());
            }
        }

        [HttpGet, Route("getAuthor/{id}")]
        public override IActionResult Get(Guid id) => Ok(new AuthorResultDTO(_authorService.GetAuthorById(id)));

        [HttpPut, Route("update/{id}"), Authorize(Roles = "admin,employee")]
        public override IActionResult Update(Guid id, [FromBody] CreateAuthorDTO dtoAuthor)
        {
            List<Book> books = new List<Book>();
            if (dtoAuthor.AuthorBooks != null)
            {
                dtoAuthor.AuthorBooks.ForEach(b => books.Add(_bookService.GetBookById(b)));
            }

            dtoAuthor.Valid();
            if (!dtoAuthor.IsValid) return BadRequest(new AuthorResultDTO(new InvalidDataExeception("Dados invalidos")).GetErrors());

            var author = new Authors(
                id: id,
                name: dtoAuthor.Name,
                nacionality: dtoAuthor.Nacionality,
                age: dtoAuthor.Age,
                books: books);
            var result=_authorService.UpdateAuthors(id, author);
            if (result.Error == true)
                return BadRequest(result.Exception);
            return Ok(new AuthorResultDTO(result.CreatedObj));
        }

        [HttpDelete, Route("deleteAuthor/{id}"), Authorize(Roles = "admin,employee")]
        public IActionResult DeleteAuthor(Guid id)
        {
            bool deletado = _authorService.DeleteAuthor(id);
            if (!deletado) return BadRequest(new AuthorResultDTO(new CreationException("Autor não foi deletado")).GetErrors());

            return NoContent();
        }
        [HttpGet, Route("authors"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetAuthorsByParams([FromQuery] string? name, [FromQuery] string? nacionality, [FromQuery] int? age, [FromQuery] int page = 1, [FromQuery] int items = 5)
        {
            List<AuthorResultDTO> authorResultDTOs = new List<AuthorResultDTO>();
            var authors = _authorService.GetAuthorsByParams(name, nacionality, age, page, items).ToList();
            authors.ForEach(x => authorResultDTOs.Add(new AuthorResultDTO(x)));

            return Ok(authorResultDTOs);
        }
    }
}
