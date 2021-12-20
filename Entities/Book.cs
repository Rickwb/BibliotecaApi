using System;

namespace BibliotecaApi.Entities
{
    public class Book : BaseEntity
    {
        public Book(Guid id, Authors author, string description, string title, int numCopies, int realeaseYear)
        {
            Id = id;
            Author = author;
            Title = title;
            Description = description;
            NumCopies = numCopies;
            NumCopiesAvailable = numCopies;
            RealeaseYear = realeaseYear;

        }

        public Authors Author { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int RealeaseYear { get; private set; }
        public int NumCopies { get; private  set; }
        public int NumCopiesAvailable { get; private set; }

        public void ControNumberOfAvailableCopies(bool retirada, int qtdCopies)
        {
            if (qtdCopies <= NumCopies)
            {
                if (retirada)
                    NumCopies -= qtdCopies;
                else
                    NumCopies += qtdCopies;
            }
            else
            {
                throw new Exception("Não foi possivel pois foi execido o numero de copies");
            }


        }

    }
}
