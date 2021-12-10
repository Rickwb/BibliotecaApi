using System;

namespace BibliotecaApi.Entities
{
    public abstract class Person:BaseEntity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid UserId { get;  set; }
        public string Cep { get; set; }
        public Adress Adress { get; set; }
    }
}
