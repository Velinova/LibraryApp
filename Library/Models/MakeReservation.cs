using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public partial class MakeReservation
    {
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ProfileId { get; set; }
        public bool ReservationStatus { get; set; }

        public MakeReservation()
        {

        }

    }
}
