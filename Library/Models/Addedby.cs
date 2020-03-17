using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Addedby
    {
        public int Sampleid { get; set; }
        public int Profileid { get; set; }

        public virtual Librarians Profile { get; set; }
        public virtual Samples Sample { get; set; }
    }
}
