using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public partial class BorrowingGridModel
    {
       
        public int Id { get; set; }
        public int Bookid { get; set; }
        public DateTime Borrowdate { get; set; }
        public DateTime Expirationdate { get; set; }
        public DateTime? Returndate { get; set; }
        public int Profileid { get; set; }
        public int Sampleid { get; set; }
        public string Bookname { get; set; }
    }
}
