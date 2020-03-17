using System;
using System.Collections.Generic;

namespace Library
{
    public partial class Profiles
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Userpassword { get; set; }
        public string Profilename { get; set; }
        public string Surname { get; set; }

        public virtual Librarians Librarians { get; set; }
        public virtual Members Members { get; set; }
    }
}
