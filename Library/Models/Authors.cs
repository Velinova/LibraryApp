using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Authors
    {
        public Authors()
        {
            Writtenby = new HashSet<Writtenby>();
        }

        public int Id { get; set; }
        public string Authorname { get; set; }

        public virtual ICollection<Writtenby> Writtenby { get; set; }
    }
}
