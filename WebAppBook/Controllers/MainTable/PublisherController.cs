using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Base_Class;
using WebAppBook.Models;

namespace WebAppBook.Controllers.MainTable
{
    public class PublisherController : TablesBaseDataController<Publisher>
    {
        CommandVoidContext commandVoid = CommandVoidContext.getContext();

        [HttpPost]
        public void AddTable(Publisher publisher)
        {
            commandVoid.DoAddCommand(publisher, EntityState.Added);
        }
        [HttpPost]
        public void EditTable(Publisher publisher)
        {
            commandVoid.DoAddCommand(publisher, EntityState.Modified);
        }

        protected override ControllerInfo<Publisher> SetContainerValue()
        {
            return new ControllerInfo<Publisher>(defaultRequestValue.Publisher, SetIncludeValues, SizeSendViewBag.isMin , "Publisher");
        }
        private void SetIncludeValues(Publisher publisher){ }
    }
}