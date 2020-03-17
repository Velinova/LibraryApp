using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Samples
    {
        public Samples()
        {
            Addedby = new HashSet<Addedby>();
            Borrowings = new HashSet<Borrowings>();
        }

        public int Id { get; set; }
        public int? Price { get; set; }
        public bool? Samplestatus { get; set; }
        public int Libraryid { get; set; }
        public int Bookid { get; set; }

        public virtual Books Book { get; set; }
        public virtual Libraries Library { get; set; }
        public virtual ICollection<Addedby> Addedby { get; set; }
        public virtual ICollection<Borrowings> Borrowings { get; set; }
    }
}
