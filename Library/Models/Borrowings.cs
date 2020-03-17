using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Borrowings
    {
        public Borrowings()
        {
            Fees = new HashSet<Fees>();
        }

        public int Id { get; set; }
        public DateTime Borrowdate { get; set; }
        public DateTime Expirationdate { get; set; }
        public DateTime? Returndate { get; set; }
        public int Profileid { get; set; }
        public int Sampleid { get; set; }

        public virtual Members Profile { get; set; }
        public virtual Samples Sample { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
    }
}
