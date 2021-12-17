using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorService(AuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }


        public Authors GetAuthorById(Guid idAuthor)
        {
            return _authorRepository.GetById(idAuthor);
        }

        public Authors UpdateAuthors(Guid idAuthor, Authors author)
        {
            return _authorRepository.Update(idAuthor, author);
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
            return _authorRepository.RemoveById(idAuthor);
        }

        public IEnumerable<Authors> GetAuthorsByParams(string? name, string? nacionality, int? age, int page=1, int items=5)
        {
            return _authorRepository.GetAllAuthorsWithParams(name, nacionality, age, page, items);
        }
    }
}
