﻿using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.Services
{
    public class BookAuthorService
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;

        public BookAuthorService(AuthorRepository authorRepository,BookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }


        public Book AddBook(Book book)
        {
            return _bookRepository.Add(book);
        }

        public IEnumerable<Book> GetBookByParams(Book? book,string? name,int? realeaseYear,int page,int items )
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

        //Author 

        public Authors GetAuthorById(Guid idAuthor)
        {
            return _authorRepository.GetById(idAuthor);
        }
        public Authors UpdateAuthors(Guid idAuthor,Authors author)
        {
            return _authorRepository.Update(idAuthor, author);
        }
        public Authors AddAuthors(Authors author)
        {
            return _authorRepository.Add(author);
        }

        public bool DeleteAuthor(Guid idAuthor)
        {
            return _authorRepository.RemoveById(idAuthor);
        }
        public IEnumerable<Authors> GetAuthorsByParams(string name,string nacionality,int age,int page,int items)
        {
           return _authorRepository.GetAllAuthorsWithParams(name, nacionality, age, page, items);
        }

    }
}