using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Catalogs
    {
        public Catalogs()
        {
            Containedby = new HashSet<Containedby>();
        }

        public int Id { get; set; }
        public DateTime? Creationdate { get; set; }

        public virtual ICollection<Containedby> Containedby { get; set; }
    }
}
