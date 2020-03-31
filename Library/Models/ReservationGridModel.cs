using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public partial class ReservationGridModel
    {

        public int Id { get; set; }
        public int RId { get; set; }
        public DateTime Reservationdate { get; set; }
        public bool? Reservationstatus { get; set; }
        public string Bookname { get; set; }
        public int? Profileid { get; set; }

    }
}
