using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.DTOs
{
    public class CreateBookDTO : BaseDTO
    {
        public Guid IdAuthor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RealeaseYear { get; set; }
        public int NumCopies { get; set; }

        public override void Valid()
        {
            IsValid = true;

            if (IdAuthor == Guid.Empty) IsValid = false;

            if (String.IsNullOrEmpty(Title) || NumCopies <= 0) IsValid = false;

            if (RealeaseYear > DateTime.Now.Year|| RealeaseYear<=0) IsValid = false;


        }
    }
}
