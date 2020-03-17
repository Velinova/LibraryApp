using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Members
    {
        public Members()
        {
            Borrowings = new HashSet<Borrowings>();
            Reservations = new HashSet<Reservations>();
        }

        public int Profileid { get; set; }
        public string Profession { get; set; }
        public DateTime? Registrationdate { get; set; }

        public virtual Profiles Profile { get; set; }
        public virtual ICollection<Borrowings> Borrowings { get; set; }
        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}
