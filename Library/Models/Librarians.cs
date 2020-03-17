using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Librarians
    {
        public Librarians()
        {
            Addedby = new HashSet<Addedby>();
        }

        public int Profileid { get; set; }

        public virtual Profiles Profile { get; set; }
        public virtual ICollection<Addedby> Addedby { get; set; }
    }
}
