using BibliotecaApi.DTOs.Results;
using BibliotecaApi.Entities;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs
{
    public class CreateResultDTO<T>  where T :BaseEntity
    {
        protected bool Success { get; set; }
        protected List<string> Errors { get; set; }
    }
}
