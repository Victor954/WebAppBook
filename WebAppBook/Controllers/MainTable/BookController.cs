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
    public class BookController : TablesBaseDataController<Book>
    {
        [HttpPost]
        public void AddTable(Book book)
        {
            BookOperation addBook = new BookOperation(
                tableAndFileControll,
                CommandType.AddCommand
            );

            addBook.SetCommand(book);
        }
        [HttpPost]
        public void EditTable(Book book)
        {
            BookOperation EditBook = new BookOperation(
                tableAndFileControll,
                CommandType.EditCommand);

            EditBook.SetCommand(book);
        }

        protected override ControllerInfo<Book> SetContainerValue()
        {
            return new ControllerInfo<Book>(
                defaultRequestValue.Book,
                SetIncludeValues,
                SizeSendViewBag.isFull,
                "Book"
            );
        }
        private void SetIncludeValues(Book book)
        {
            ListCollection.SetAllValue(book.Genres, book.Languages, book.Translators);
        }
    }
}