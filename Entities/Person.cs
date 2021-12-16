using System;

namespace BibliotecaApi.Entities
{
    public abstract class Person : BaseEntity
    {
        public string Name { get; protected set; }
        public string Document { get; protected set; }
        public int Age { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Cep { get; protected set; }
        public Adress Adress { get; set; }
    }
}
