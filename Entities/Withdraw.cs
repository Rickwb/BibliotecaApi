using System;

namespace BibliotecaApi.Entities
{
    public class Withdraw : BaseEntity
    {
        public Withdraw()
        {
            Id = Guid.NewGuid();
        }
    }
}
