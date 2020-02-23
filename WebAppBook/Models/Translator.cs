using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Translator
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }

        public ICollection<Book> Books { get; set; }

        public Translator()
        {
            Books = new List<Book>();
        }
    }
}