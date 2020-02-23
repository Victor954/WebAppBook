using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Base_Class;
using WebAppBook.Models;

namespace WebAppBook.Controllers.Filter
{
    public class BookSearchController : FilterController<Book>
    {
        private ConcreteFilter<Book> filter = new ConcreteFilter<Book>();

        [HttpPost]
        public ActionResult SearchBookSearh(string search, string searchAuthor)
        {
            filter.SetSearchByName(s => s.Name, search);
            filter.SetSearchByName(GetFioAuthor, searchAuthor);

            filterContext.SetFilterResult(filter);

            return RedirectToAction("TableInvoke", "Book");
        }

        [HttpPost]
        public ActionResult SearchBook(List<Genre> listMany, List<Language> listManyLanguage)
        {
            filter.SetMany(s=>s.Genres, listMany);
            filter.SetMany(s => s.Languages, listManyLanguage);

            filterContext.SetFilterResult(filter);

            return RedirectToAction("TableInvoke", "Book");
        }
        private string GetFioAuthor(Book b)
        {
            return string.Format(
            "{0} {1} {2}",
            b.Author.Name,
            b.Author.LastName,
            b.Author.PatronymicName);
        }
    }
}