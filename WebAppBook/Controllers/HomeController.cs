using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Models;
using WebAppBook.Base_Class;
using System.Data.Entity;

namespace WebAppBook.Controllers
{
    public class HomeController : TablesData
    {
        public ActionResult Index()
        {
            Pagination.startPoint = 0;
            ListCollection.SetDefault();

            return View(SizeSendViewBag.isMin);
        }
    }
}