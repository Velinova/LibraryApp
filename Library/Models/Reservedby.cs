using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Reservedby
    {
        public int Reservationid { get; set; }
        public int Bookid { get; set; }

        public virtual Books Book { get; set; }
        public virtual Reservations Reservation { get; set; }
    }
}
