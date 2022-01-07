using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaApi.DTOs
{
    public class CreateReservationDTO : BaseDTO
    {
        public Guid idCustumer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> IdBooks { get; set; }

        public bool UpdateValid { get; private set; }

        public override void Valid()
        {
            IsValid = true;

            if (idCustumer == Guid.Empty) IsValid = false;
            if (StartDate > EndDate) IsValid = false;
            if (StartDate == DateTime.MinValue) IsValid = false;
            if (StartDate.Date < DateTime.Today.Date) IsValid = false;
            if (EndDate == DateTime.MinValue || EndDate.Date < DateTime.Today.Date) IsValid = false;
            if (IdBooks.Count == 0) IsValid = false;
        }

        public void ValidUpdate()
        {
            UpdateValid = true;

            if (idCustumer == Guid.Empty) UpdateValid = false;
            if (StartDate < EndDate) UpdateValid = false;
            if (StartDate == DateTime.MinValue) UpdateValid = false;
            if (EndDate == DateTime.MinValue || EndDate.Date < DateTime.Today.Date) UpdateValid = false;
            if (IdBooks.Count == 0) UpdateValid = false;
        }
    }
}
