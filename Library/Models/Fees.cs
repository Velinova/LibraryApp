using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Fees
    {
        public int Id { get; set; }
        public int? Feetotal { get; set; }
        public int Borrowid { get; set; }

        public virtual Borrowings Borrow { get; set; }
    }
}
