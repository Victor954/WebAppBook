using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Publisher
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ICollection<Book> Books { get; set; }

        public Publisher()
        {
            Books = new List<Book>();
        }
    }
}