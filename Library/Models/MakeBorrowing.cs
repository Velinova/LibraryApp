using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public partial class MakeBorrowing
    {
        public int BookId { get; set; }
        public string UserName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public MakeBorrowing()
        {

        }

    }
}
