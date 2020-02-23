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
    public class AuthorController : TablesBaseDataController<Author>
    {
        [HttpPost]
        public void AddTable(Author author)
        {
            AuthorOperation addAuthor = new AuthorOperation(
                tableAndFileControll,
                CommandType.AddCommand
            );

            addAuthor.SetCommand(author);
        }
        [HttpPost]
        public void EditTable(Author author)
        {
            AuthorOperation EditAuthor = new AuthorOperation(
                tableAndFileControll,
                CommandType.EditCommand);

            EditAuthor.SetCommand(author);
        }

        protected override ControllerInfo<Author> SetContainerValue()
        {
            return new ControllerInfo<Author>(
                defaultRequestValue.Author,
                SetIncludeValues,
                SizeSendViewBag.isMin,
                "Author"
            );
        }
        private void SetIncludeValues(Author author)
        {
            ListCollection.SetAllValue(author.Genres);
        }
    }
}