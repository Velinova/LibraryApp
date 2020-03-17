using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Writtenby
    {
        public int Bookid { get; set; }
        public int Authorid { get; set; }

        public virtual Authors Author { get; set; }
        public virtual Books Book { get; set; }
    }
}
