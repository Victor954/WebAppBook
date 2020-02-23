using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Base_Class;
using WebAppBook.Models;
using System.Data.Entity;

namespace WebAppBook.Controllers
{
    public class GenreController : TablesSecondDataController<Genre>
    {
        public GenreController()
        {
            listSecondTables = ListCollection.genres;
            searchMethod = (db,search) => db.Genres.Where(d => d.GenreName.Contains(search));
        }
    }
}