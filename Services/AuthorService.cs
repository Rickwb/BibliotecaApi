using BibliotecaApi.Repositories;
using Domain.Enities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;
        private readonly BookRepository _bookRepository;

        public AuthorService(AuthorRepository authorRepository, BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }


        public Authors GetAuthorById(Guid idAuthor)
        {
            return _authorRepository.GetById(idAuthor);
        }

        public CreateResult<Authors> UpdateAuthors(Guid idAuthor, Authors author)
        {
            var authornew=_authorRepository.Update(idAuthor, author);
            if (authornew == null)
                return CreateResult<Authors>.Errors(new CreationException("O autor não pode ser atualizado"));

            return CreateResult<Authors>.Sucess(authornew);

        }
        public CreateResult<Authors> AddAuthors(Authors author)
        {
            try
            {
                if (author.Age == 0)
                    return CreateResult<Authors>.Errors(new InvalidDataExeception("O autor não pode ser cadastrado com a idade 0"));
                _authorRepository.Add(author);

                return CreateResult<Authors>.Sucess(author);

            }
            catch (Exception ex)
            {
                return CreateResult<Authors>.Errors(new CreationException("Nao foi possivel cadastrar"));
            }

        }

        public bool DeleteAuthor(Guid idAuthor)
        {
            var author = _authorRepository.GetById(idAuthor);
            if (author is not null)
            {
                if (author.AuthorBooks != null)
                {
                    author.AuthorBooks.ForEach(book => _bookRepository.RemoveById(book.Id)
                    );
                }
                return _authorRepository.RemoveById(idAuthor);
            }
            return false;
        }

        public IEnumerable<Authors> GetAuthorsByParams(string? name, string? nacionality, int? age, int page = 1, int items = 5)
        {
            return _authorRepository.GetAllAuthorsWithParams(name, nacionality, age, page, items);
        }
    }
}
