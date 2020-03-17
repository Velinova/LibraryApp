using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Libraries
    {
        public Libraries()
        {
            Samples = new HashSet<Samples>();
        }

        public int Id { get; set; }
        public string Libraryname { get; set; }
        public string Librarylocation { get; set; }
        public int? Numberofbooks { get; set; }

        public virtual ICollection<Samples> Samples { get; set; }
    }
}
