using BibliotecaApi.DTOs.Results;
using Domain.Enities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs
{
    public class CreateResultDTO<T> where T : BaseEntity<Guid>
    {
        protected bool Success { get; set; }
        protected List<string> Errors { get; set; }
    }
}
