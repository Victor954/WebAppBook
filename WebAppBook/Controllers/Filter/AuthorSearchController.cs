using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Models;
using WebAppBook.Base_Class;
using System.Data.Entity;

namespace WebAppBook.Controllers.FilterController
{
    public class AuthorSearchController : FilterController<Author>
    {
        private ConcreteFilter<Author> filter = new ConcreteFilter<Author>();

        [HttpPost]
        public ActionResult Search(string search,List<Genre> listMany)
        {
            filter.SetSearchByName(GetFioAuthor,search);
            filter.SetMany(s=>s.Genres, listMany);

            filterContext.SetFilterResult(filter);

            return RedirectToAction("TableInvoke", "Author");
        }

        private string GetFioAuthor(Author a)
        {
            return string.Format(
            "{0} {1} {2}",
            a.Name,
            a.LastName,
            a.PatronymicName);
        }
    }
}