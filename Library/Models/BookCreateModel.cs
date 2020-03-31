using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class BookCreateModel
    {
        public int Id { get; set; }
        public int? Numberofpages { get; set; }
        public int? Numberofsamples { get; set; }
        public string Genre { get; set; }
        public string Booklanguage { get; set; }
        public string Bookname { get; set; }
        public string Plot { get; set; }
        public string AuthorName { get; set; }
    }
}
