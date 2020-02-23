using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBook.Models
{
    public class Book
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateWrite { get; set; }
        public DateTime DatePublication { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string ImgSrc { get; set; }
        public int Number { get; set; }
        //
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public int? AuthorId { get; set; }
        public Author Author { get; set; }
        //
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Translator> Translators { get; set; }
        public ICollection<Language> Languages { get; set; }

        public Book()
        {
            Genres = new List<Genre>();
            Translators = new List<Translator>();
            Languages = new List<Language>();
        }
    }
}