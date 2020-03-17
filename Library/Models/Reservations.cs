using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Reservations
    {
        public Reservations()
        {
            Reservedby = new HashSet<Reservedby>();
        }

        public int Id { get; set; }
        public bool? Reservationstatus { get; set; }
        public DateTime Reservationdate { get; set; }
        public int? Profileid { get; set; }

        public virtual Members Profile { get; set; }
        public virtual ICollection<Reservedby> Reservedby { get; set; }
    }
}
