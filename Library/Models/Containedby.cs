using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Containedby
    {
        public int Bookid { get; set; }
        public int Catalogid { get; set; }

        public virtual Books Book { get; set; }
        public virtual Catalogs Catalog { get; set; }
    }
}
