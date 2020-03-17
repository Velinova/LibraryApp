using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Books
    {
        public Books()
        {
            Containedby = new HashSet<Containedby>();
            Reservedby = new HashSet<Reservedby>();
            Samples = new HashSet<Samples>();
            Writtenby = new HashSet<Writtenby>();
        }

        public int Id { get; set; }
        public int? Numberofpages { get; set; }
        public int? Numberofsamples { get; set; }
        public string Genre { get; set; }
        public string Booklanguage { get; set; }
        public string Bookname { get; set; }
        public string Plot { get; set; }

        public virtual ICollection<Containedby> Containedby { get; set; }
        public virtual ICollection<Reservedby> Reservedby { get; set; }
        public virtual ICollection<Samples> Samples { get; set; }
        public virtual ICollection<Writtenby> Writtenby { get; set; }
    }
}
