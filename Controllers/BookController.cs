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
    public class BookController : BaseControl<CreateBookDTO, Book>
    {
        private readonly BookAuthorService _bookAuthorService;
        public BookController(BookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }
        [HttpGet, Route("getBook/{id}")]
        public override IActionResult Get(Guid idBook) => Ok(_bookAuthorService.GetBookById(idBook));

        [HttpPut, Route("updateBook/{id}")]
        public override IActionResult Update(Guid idBook, [FromBody] CreateBookDTO createBookDTO)
        {
            createBookDTO.Valid();
            if (!createBookDTO.IsValid)
                return BadRequest();
            var author = _bookAuthorService.GetAuthorById(createBookDTO.IdAuthor);

            var book = new Book(
                author: author,
                title: createBookDTO.Title,
                numCopies: createBookDTO.NumCopies,
                realeaseYear: createBookDTO.RealeaseYear
                );
            return Ok(_bookAuthorService.UpdateBook(idBook, book));
        }
        [HttpPost, Route("addBook")]
        public override IActionResult Add([FromBody] CreateBookDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid)
                return BadRequest();

            var author = _bookAuthorService.GetAuthorById(dto.IdAuthor);

            var book = new Book(
                author: author,
                title: dto.Title,
                numCopies: dto.NumCopies,
                realeaseYear: dto.RealeaseYear);

            return Ok(_bookAuthorService.AddBook(book));

        }
        [HttpDelete, Route("deleteBook/{id}")]
        public IActionResult DeleteBook(Guid idBook)
        {
            bool deletado = _bookAuthorService.DeleteBook(idBook);
            if (!deletado) return BadRequest();

            return NotFound(deletado);

        }
        [HttpGet, Route("GetBooksFiltered")]
        public IActionResult GetBooksByParams([FromQuery] Guid idBook, [FromQuery] string name, [FromQuery] int realeaseYear, [FromQuery] int page, [FromQuery] int items)
        {
            var book = _bookAuthorService.GetBookById(idBook);
            return Ok(_bookAuthorService.GetBookByParams(book, name, realeaseYear, page, items));
        }
    }
}
