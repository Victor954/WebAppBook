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
    public class LanguagesController : TablesSecondDataController<Language>
    {
        public LanguagesController()
        {
            listSecondTables = ListCollection.languages;
            searchMethod = (db, search) => db.Languages.Where(d => d.LanguageName.Contains(search));
        }
    }
}