using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Base_Class;
using WebAppBook.Models;

namespace WebAppBook.Controllers
{
    public class TranslatorController : TablesSecondDataController<Translator>
    {
        public TranslatorController()
        {
            listSecondTables = ListCollection.translators;
        }

        [HttpPost]
        public override JsonResult Search(string search)
        {
            return Json(selectContext.DoSelectCommand(s => s.Translators.Where(
                d => d.Name.Contains(search) ||
                d.LastName.Contains(search) ||
                d.PatronymicName.Contains(search)
            )));
        }
    }
}