using System;

namespace BibliotecaApi.Entities
{
    public class Reservation : BaseEntity
    {
        public Reservation()
        {
            Id = Guid.NewGuid();
        }
    }
}
