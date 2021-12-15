using BibliotecaApi.DTOs;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class CreateResult<T> where T : BaseEntity
    {
        public  bool Error { get; set; }

        public  T CreatedObj { get; set; }
        public  CreationException Exception { get; set; }

        public static CreateResult<T> Sucess(T obj)
        {
            return new CreateResult<T>()
            {
                Error = false,

                CreatedObj = obj
            };
        }
        public static CreateResult<T> Errors(CreationException exeption)
        {
            return new CreateResult<T>()
            {
                Error = true,
                Exception = exeption,
                CreatedObj = null
            };
        }

    }

    public class CreationException : Exception
    {
        private const string Message = "Não foi posssivel cadastrar";

        public CreationException(string message):base(message)
        {

        }
        public CreationException(Exception inner) : base(Message, inner)
        {
        }

        public CreationException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidDataExeception : CreationException
    {
        private const string Message = "Dados para o cadastro foram inseridos de forma errada";

        public static InvalidDataExeception invalidDataExeception = new (Message);
        public InvalidDataExeception(string message) : base(message)
        {
        }

        public InvalidDataExeception(Exception inner) : base(inner)
        {
        }

        public InvalidDataExeception(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class SameObjectExeception : CreationException
    {
        private const string Message = "Já existe no banco de dados";

        public static InvalidDataExeception invalidDataExeception = new(Message);
        public SameObjectExeception(string message) : base(message)
        {
        }

        public SameObjectExeception(Exception inner) : base(inner)
        {
        }

        public SameObjectExeception(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
