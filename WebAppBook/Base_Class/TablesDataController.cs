using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Models;
using System.Reflection;

namespace WebAppBook.Base_Class
{
    public delegate IQueryable<T> Search<T>(BookContext db,string search);
    //Перечисление видов получения количества полей в таблице
    public enum SizeSendViewBag { isFull ,isMin }
    //Абстрактный контроллер для определения малых контроллеров
    public abstract class TablesSecondDataController<T> : Controller where T : class
    {
        protected static CommandContext selectContext = CommandContext.getContext();
        protected static CommandVoidContext voidContext = CommandVoidContext.getContext();

        static SearchTableFromContext<T> dbSetType = s => s.Set<T>();
        protected List<T> listSecondTables { get; set; }
        protected Search<T> searchMethod { get; set; }

        private JsonResult SetResulst(SearchTableFromContext<T> search, T obj, EntityState entityState)
        {
            voidContext.DoAddCommand(obj, entityState);
            return Json(selectContext.DoSelectCommand(search));
        }

        [HttpPost]
        public void AddList(T obj)
        {
            listSecondTables.Add(obj);
        }
        [HttpPost]
        public void RemoveList(T obj)
        {
            listSecondTables.Remove( listSecondTables.FirstOrDefault(s=> ReflectionEfId<T>.GetId(s) == ReflectionEfId<T>.GetId(obj)) );
        }
        [HttpPost]
        public JsonResult AddDb(T obj)
        {
            return SetResulst(dbSetType, obj, EntityState.Added);
        }
        [HttpPost]
        public JsonResult EditDb(T obj)
        {
            return SetResulst(dbSetType, obj, EntityState.Modified);
        }
        [HttpPost]
        public JsonResult DeleteDb(T obj)
        {
            return SetResulst(dbSetType, obj, EntityState.Deleted);
        }
        [HttpPost]
        public virtual JsonResult Search(string search)
        {
            return Json(selectContext.SetResultCommand(new SelectCommand<T>(searchMethod,search)));
        }
    }
    //Абстрактный контроллер для определения крупных контроллеров
    public partial class TablesData : Controller
    {
        static protected CommandContext selectContext = CommandContext.getContext();
        static protected SelectDefault defaultRequestValue = new SelectDefault();

        public ViewResult View(SizeSendViewBag state)
        {
            SetTablesViewBag(state);
            return View();
        }
        private void SetTablesViewBag(SizeSendViewBag size)
        {
            SetSecondTablesViewBag();
            switch (size)
            {
                case SizeSendViewBag.isFull:
                    SetBaseTablesViewBag();
                    break;
            }
        }
        private void SetSecondTablesViewBag()
        {
            ViewBag.Genre = selectContext.DoSelectCommandAsync<Genre>(); ;
            ViewBag.Language = selectContext.DoSelectCommandAsync<Language>();

            ViewBag.Translator = selectContext.DoSelectCommandAsync(
            s => s.Translators.Include("Books")
            );
        }
        private void SetBaseTablesViewBag()
        {
            ViewBag.Author = selectContext.DoSelectCommandAsync(defaultRequestValue.Author);
            ViewBag.Publisher = selectContext.DoSelectCommandAsync(defaultRequestValue.Publisher);
        }
    }
    public abstract class TablesBaseDataController<T> : TablesData where T : class
    {
        protected AddWithFileCommand<T> tableAndFileControll = new AddWithFileCommand<T>(new DefaultEditFile<T>());
        protected static ControllerInfo<T> container;
        protected abstract ControllerInfo<T> SetContainerValue();

        [HttpPost]
        public string Upload()
        {
            return tableAndFileControll.SetFile(Request.Files[0], this);
        }
        [HttpPost]
        public void RemoveTable(T table)
        {
            CommandVoidContext singleContext = CommandVoidContext.getContext();

            singleContext.DoAddCommand(table, EntityState.Deleted);
        }
        [HttpGet]
        public ActionResult TableInvoke()
        {
            container = SetContainerValue();

            ViewBag.Table = PanigationFilter<T>.GetPagination() ?? selectContext.DoSelectCommand(container.request, true);
            FilterContext<T>.SetForFiltering(selectContext.DoSelectCommandQuery(container.request));

            return PartialView(container.view);
        }
        public ActionResult AddForm()
        {
            return View(container.sizeDbFilelds);
        }
        public ActionResult EditForm(int id)
        {
            CommandSingleContext singleContext = CommandSingleContext.getContext();

            T editedRow = singleContext.DoSingleCommand(container.request, id);
            ViewBag.Table = editedRow;

            container.includeMethod(editedRow);

            return View(container.sizeDbFilelds);
        }

        [HttpPost]
        public ActionResult SetPagination(ushort page)
        {
            Pagination.startPoint = page;

            return RedirectToAction("TableInvoke");
        }
    }
    public struct ControllerInfo<T> where T : class
    {
        public readonly SearchTableFromContext<T> request;
        public readonly string view;
        public readonly SizeSendViewBag sizeDbFilelds;
        public readonly Action<T> includeMethod;

        public ControllerInfo( SearchTableFromContext<T> _request, Action<T> _includeValues, SizeSendViewBag _size, string _views)
        {
            request = _request;
            includeMethod = _includeValues;
            view = _views;
            sizeDbFilelds = _size;
        }
    }
}