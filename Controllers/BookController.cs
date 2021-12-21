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
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;

        public BookController(BookService bookAuthorService, AuthorService authorService)
        {
            _bookService = bookAuthorService;
            _authorService = authorService;
        }

        [HttpGet, Route("getBook/{id}")]
        public override IActionResult Get(Guid id) => Ok(new BookResultDTO(_bookService.GetBookById(id)));

        [HttpPut, Route("updateBook/{id}"), Authorize(Roles = "admin,employee")]
        public override IActionResult Update(Guid id, [FromBody] CreateBookDTO createBookDTO)
        {
            createBookDTO.Valid();
            if (!createBookDTO.IsValid)
                return BadRequest(new BookResultDTO(new InvalidDataExeception("Dados Invalidos")));

            var author = _authorService.GetAuthorById(createBookDTO.IdAuthor);

            var book = new Book(
                id: id,
                author: author,
                description: createBookDTO.Description,
                title: createBookDTO.Title,
                numCopies: createBookDTO.NumCopies,
                realeaseYear: createBookDTO.RealeaseYear
                );
            var booknew = _bookService.UpdateBook(id, book);
            return Ok(new BookResultDTO(booknew));
        }

        [HttpPost, Route("addBook"), Authorize(Roles = "admin,employee")]
        public override IActionResult Add([FromBody] CreateBookDTO dto)
        {
            dto.Valid();
            if (!dto.IsValid) return BadRequest(new BookResultDTO(new InvalidDataExeception("Dados Inválidos")).GetErrors());

            var author = _authorService.GetAuthorById(dto.IdAuthor);

            var book = new Book(
                id: Guid.NewGuid(),
                author: author,
                description: dto.Description,
                title: dto.Title,
                numCopies: dto.NumCopies,
                realeaseYear: dto.RealeaseYear);

            var result = _bookService.AddBook(book);

            if (result.Error == false) return Created("", new BookResultDTO(book));

            return BadRequest(new BookResultDTO(result.Exception).GetErrors());
        }

        [HttpDelete, Route("deleteBook/{id}"), Authorize(Roles = "admin,employee")]
        public IActionResult DeleteBook(Guid id)
        {
            bool deletado = _bookService.DeleteBook(id);
            if (!deletado) return BadRequest(new BookResultDTO(new InvalidDataExeception("Dados Invalidos")).GetErrors());

            return NotFound();
        }

        [HttpGet, Route("GetBooksFiltered"), ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public IActionResult GetBooksByParams([FromQuery] Guid idBook, [FromQuery] string? name, [FromQuery] int? realeaseYear, [FromQuery] int page=1, [FromQuery] int items=5)
        {
            var book = _bookService.GetBookById(idBook);

            List<BookResultDTO> booksresults = new List<BookResultDTO>();
            var booksParams=_bookService.GetBookByParams(book, name, realeaseYear, page, items);
            booksParams.ToList().ForEach(book => booksresults.Add(new BookResultDTO(book)));

            return Ok(booksresults);
        }
    }
}
