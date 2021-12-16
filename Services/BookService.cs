using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;

        public BookService(AuthorRepository authorRepository, BookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        public CreateResult<Book> AddBook(Book book)
        {
            if (book.Author == null)
                return CreateResult<Book>.Errors(new InvalidDataExeception("O livro não pode ser cadastrado sem autor"));
            if (_bookRepository.GetAll().Contains(book))
                return CreateResult<Book>.Errors(new SameObjectExeception("O Livro já existe no cadastro, adicione uma copia "));

            _bookRepository.Add(book);
            return CreateResult<Book>.Sucess(book);
        }

        public IEnumerable<Book> GetBookByParams(Book? book, string? name, int? realeaseYear, int page, int items)
        {
            return _bookRepository.GetAllBooksWithParams(book, name, realeaseYear, page, items);
        }

        public Book GetBookById(Guid idBook) => _bookRepository.GetById(idBook);

        public Book UpdateBook(Guid idBook, Book book)
        {
            return _bookRepository.Update(idBook, book);
        }

        public bool DeleteBook(Guid idBook)
        {
            return _bookRepository.RemoveById(idBook);
        } 

    }
}
