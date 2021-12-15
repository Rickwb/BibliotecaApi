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
        public List<Guid> IdBooks { get; set; }

        public override void Valid()
        {
            if (idCustumer==Guid.Empty)IsValid= false;
            if(StartDate==DateTime.MinValue)IsValid= false;
            if (IdBooks.Count == 0) IsValid = false;

            IsValid= true;
        }
    }
}
