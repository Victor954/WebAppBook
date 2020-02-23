using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Base_Class;
using WebAppBook.Models;

namespace WebAppBook.Controllers.Filter
{
    public class PublisherSearchController : FilterController<Publisher>
    {
        private ConcreteFilter<Publisher> filter = new ConcreteFilter<Publisher>();

        [HttpPost]
        public ActionResult SearchPublisher(string search,string searchBook)
        {
            filter.SetSearchByName(s=>s.Name, search);
            filter.SetSearchByName(GetPublisherBook, searchBook);

            filterContext.SetFilterResult(filter);

            return RedirectToAction("TableInvoke", "Publisher");
        }

        private string GetPublisherBook(Publisher publisher)
        {
            StringBuilder builder = new StringBuilder();
            IEnumerable<string> bookName =  publisher.Books.Select(s => s.Name);

            foreach(string name in bookName)
            {
                builder.Append(name);
            }
            string test = builder.ToString();
            return builder.ToString();
        }
    }
}