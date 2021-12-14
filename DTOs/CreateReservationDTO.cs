using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.DTOs
{
    public class CreateReservationDTO:BaseDTO
    {
        public Guid idCustumer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Book> Books { get; set; }

        public override void Valid()
        {
            foreach (var item in Books)
            {
                if (item.NumCopies)
                {

                }
            }
        }
    }
}
