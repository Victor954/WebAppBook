using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBook.Base_Class
{
    public abstract class FilterController<T> : Controller where T : class
    {
        protected FilterContext<T> filterContext = new FilterContext<T>();
    }
}