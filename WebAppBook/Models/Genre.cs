using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Genre
    {
        public int? Id { get; set; }
        public string GenreName { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<Author> Authors { get; set; }

        public Genre()
        {
            Books = new List<Book>();
            Authors = new List<Author>();
        }
    }
}