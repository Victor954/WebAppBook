using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Author
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public DateTime DateBorn { get; set; }
        public string CountryBorn { get; set; }
        public string Info { get; set; }
        public string ImgSrc { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public ICollection<Book> Books { get; set; }

        public Author()
        {
            Genres = new List<Genre>();
            Books = new List<Book>();
        }
    }
}