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
    public class BookController : BaseControl<CreateBookDTO, Book>
    {
        private readonly BookAuthorService _bookAuthorService;
        public BookController(BookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }
        [HttpGet, Route("getBook/{id}")]
        public override IActionResult Get(Guid id) => Ok(_bookAuthorService.GetBookById(id));

        [HttpPut, Route("updateBook/{id}"), Authorize(Roles = "admin,employee")]
        public override IActionResult Update(Guid id, [FromBody] CreateBookDTO createBookDTO)
        {
            createBookDTO.Valid();
            if (!createBookDTO.IsValid)
                return BadRequest();
            var author = _bookAuthorService.GetAuthorById(createBookDTO.IdAuthor);

            var book = new Book(
                id: id,
                author: author,
                description: createBookDTO.Description,
                title: createBookDTO.Title,
                numCopies: createBookDTO.NumCopies,
                realeaseYear: createBookDTO.RealeaseYear
                );

            return Ok(_bookAuthorService.UpdateBook(id, book));
        }
        [HttpPost, Route("addBook"), Authorize(Roles = "admin,employee")]
        public override IActionResult Add([FromBody] CreateBookDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid)
                return BadRequest();

            var author = _bookAuthorService.GetAuthorById(dto.IdAuthor);

            var book = new Book(
                id: Guid.NewGuid(),
                author: author,
                description:dto.Description,
                title: dto.Title,
                numCopies: dto.NumCopies,
                realeaseYear: dto.RealeaseYear);
            var result = _bookAuthorService.AddBook(book);

            if (result.Error==false)
            {

                return Created("", new BookResultDTO(book));
            }
            return BadRequest(new BookResultDTO(result.Exception));

        }
        [HttpDelete, Route("deleteBook/{id}"), Authorize(Roles = "admin,employee")]
        public IActionResult DeleteBook(Guid id)
        {
            bool deletado = _bookAuthorService.DeleteBook(id);
            if (!deletado) return BadRequest();

            return NotFound(deletado);

        }
        [HttpGet, Route("GetBooksFiltered")]
        public IActionResult GetBooksByParams([FromQuery] Guid idBook, [FromQuery] string? name, [FromQuery] int? realeaseYear, [FromQuery] int page, [FromQuery] int items)
        {
            var book = _bookAuthorService.GetBookById(idBook);
            return Ok(_bookAuthorService.GetBookByParams(book, name, realeaseYear, page, items));
        }
    }
}
