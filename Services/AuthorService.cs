using BibliotecaApi.Repositories;
using DapperContext.Repositories;
using Domain.Enities;
using EFContext.Repositories;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepositoryEF _authorRepositoryEF;
        private readonly BookRepositoryEF _bookRepositoryEF;
        private readonly AuthorRepositoryDP _authorRepositoryDP;
        public AuthorService(AuthorRepository authorRepository, BookRepository bookRepository,AuthorRepositoryEF authorRepositoryEF, AuthorRepositoryDP authorRepositoryDP,BookRepositoryEF bookRepositoryEF)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _authorRepositoryEF = authorRepositoryEF;
            _authorRepositoryEF = authorRepositoryEF;
            _authorRepositoryDP = authorRepositoryDP;
            _bookRepositoryEF = bookRepositoryEF;
        }


        public Authors GetAuthorById(Guid idAuthor)
        {
            return _authorRepository.GetById(idAuthor);
        }

        public CreateResult<Authors> UpdateAuthors(Guid idAuthor, Authors author)
        {

            var authorResult = _authorRepositoryDP.GetById(idAuthor.ToByteArray(), "\"BaseEntity<Guid>\"");
            if (authorResult == null)
                return CreateResult<Authors>.Errors(new CreationException("Not found for update"));

            var authornew = _authorRepositoryEF.Update(author);
            if (!authornew)
                return CreateResult<Authors>.Errors(new CreationException("O autor não pode ser atualizado"));

            return CreateResult<Authors>.Sucess(author);

        }
        public CreateResult<Authors> AddAuthors(Authors author)
        {
            try
            {
                if (author.Age == 0)
                    return CreateResult<Authors>.Errors(new InvalidDataExeception("O autor não pode ser cadastrado com a idade 0"));
                _authorRepositoryEF.Insert(author);

                return CreateResult<Authors>.Sucess(author);

            }
            catch (Exception ex)
            {
                return CreateResult<Authors>.Errors(new CreationException("Nao foi possivel cadastrar"));
            }

        }

        public bool DeleteAuthor(Guid idAuthor)
        {
            var author = _authorRepositoryDP.GetById(idAuthor,"BaseEnitt");
            if (author is not null)
            {
                if (author != null)
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
