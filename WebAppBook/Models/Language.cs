using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Language
    {
        public int? Id { get; set; }
        public string LanguageName { get; set; }

        public ICollection<Book> Books { get; set; }

        public Language()
        {
            Books = new List<Book>();
        }
    }
}