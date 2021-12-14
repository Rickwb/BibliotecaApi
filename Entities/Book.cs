using System;

namespace BibliotecaApi.Entities
{
    public class Book : BaseEntity
    {
        public Book(Guid id,Authors author, string title, int numCopies, int realeaseYear)
        {
            Id = id;
            Author = author;
            Title = title;
            NumCopies = numCopies;
            NumCopiesAvailable = numCopies;
            RealeaseYear = realeaseYear;

        }

        public Authors Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RealeaseYear { get; set; }
        public int NumCopies { get; set; }
        public int NumCopiesAvailable { get; set; }


        public void ControNumberOfAvailableCopies(bool retirada, int qtdCopies)
        {
            if (qtdCopies <= NumCopiesAvailable)
            {
                if (retirada)
                    NumCopiesAvailable -= qtdCopies;
                else
                    NumCopiesAvailable += qtdCopies;
            }
            else
            {
                throw new Exception("Não foi possivel pois foi execido o numero de copies");
            }

        }




    }
}
