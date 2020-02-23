using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    //Установка кастомной комманды над БД
    sealed class AddCommand<T> : ICommandVoid where T : class
    {
        public EntityState EntityState { get; set; }
        private T obj;

        public AddCommand(T obj)
        {
            this.obj = obj;
        }

        public void SetCommand(BookContext db)
        {
            db.Entry(obj).State = EntityState;
        }
    }
    //Комманда выборки из БД
    partial class BaseSelectCommad<T> where T : class
    {
        protected SearchTableFromContext<T> request;
        protected bool isPagination = false;
        public BaseSelectCommad()
        {
            request = db => db.Set<T>();
        }
        public BaseSelectCommad(SearchTableFromContext<T> request)
        {
            this.request = request;
        }
        public BaseSelectCommad(Search<T> request, string search)
        {
            this.request = db => request(db, search);
        }
        public BaseSelectCommad(SearchTableFromContext<T> request, bool isPagination)
        {
            this.request = request;
            this.isPagination = isPagination;
        }
    }
    //Комманда выборки из БД
    sealed class SelectCommand<T> : BaseSelectCommad<T> , ICommandResult<T> where T : class
    {
        public SelectCommand() : base() { }
        public SelectCommand(SearchTableFromContext<T> request) : base(request) { }
        public SelectCommand(Search<T> request, string search) : base(request, search) { }
        public SelectCommand(SearchTableFromContext<T> request, bool isPagination) : base(request, isPagination) { }

        public IEnumerable<T> SetCommand(BookContext db)
        {
            if (isPagination)
                return Pagination.SetPagination(request(db));

            return request(db).ToList();
        }
    }
    //Async комманда выборки из БД
    sealed class SelectCommandAsycn<T> : BaseSelectCommad<T> , ICommandResultAsync<T> where T : class
    {
        public SelectCommandAsycn() : base() { }
        public SelectCommandAsycn(SearchTableFromContext<T> request) : base(request) { }
        public SelectCommandAsycn(SearchTableFromContext<T> request, bool isPagination) : base(request, isPagination) { }

        public Task<List<T>> SetCommandAsync(BookContext db)
        {
            if(isPagination)
                return Pagination.SetPaginationAsync(request(db));

            return request(db).ToListAsync();
        }
    }

    //Комманда выборки одного элемента по Id
    sealed class SingleController<T> : ICommandSingle<T> where T : class
    {
        int id { get; set; }
        SearchTableFromContext<T> request { get; set; }
        public SingleController(SearchTableFromContext<T> request, int id)
        {
            this.id = id;
            this.request = request;
        }
        public T SetCommand(BookContext db)
        {
            return request(db).AsEnumerable().FirstOrDefault(k => ReflectionEfId<T>.GetId(k) == id);
        }
    }
    //Классы настроек над связанными таблицами
    sealed class EditManyToManyCommand<T> : ICommandVoid where T : class
    {
        int id;
        SearchTableFromContext<T> request;
        AddManyToManyCommand<T> addCommand;
        public EditManyToManyCommand(int id)
        {
            this.id = id;
        }
        public EditManyToManyCommand(SearchTableFromContext<T> request, List<ITableInclude<T>> include, int id)
        {
            this.id = id;
            this.request = request;

            addCommand = new AddManyToManyCommand<T>(request, include);
        }
        public void SetCommand(BookContext db)
        {
            T firstIdEqual = request(db).AsEnumerable().FirstOrDefault(j => ReflectionEfId<T>.GetId(j) == id);

            addCommand.SetEdit(firstIdEqual, db);
        }
    }
    sealed class AddManyToManyCommand<T> : ICommandVoid where T : class
    {
        SearchTableFromContext<T> request;
        List<ITableInclude<T>> listIncludeCommand;

        public AddManyToManyCommand(SearchTableFromContext<T> request, List<ITableInclude<T>> listIncludeCommand)
        {
            this.listIncludeCommand = listIncludeCommand;
            this.request = request;
        }
        public void SetEdit(T obj, BookContext db)
        {
            foreach (ITableInclude<T> include in listIncludeCommand)
            {
                include.SetCommand(obj, db);
            }
            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void SetCommand(BookContext db)
        {
            T lastElem = request(db).AsEnumerable().LastOrDefault();
            SetEdit(lastElem, db);
        }
    }
    //Управаление связанными таблицами
    sealed class RelatedTableCommand<A, T> : ITableInclude<A>
    {
        private IncludeTables<T, A> relMethod;
        private SearchTableFromContext<T> request;
        private List<T> SecondTablesList;

        public RelatedTableCommand(IncludeTables<T, A> relMethod, SearchTableFromContext<T> request, List<T> SecondTablesList)
        {
            this.relMethod = relMethod;
            this.SecondTablesList = SecondTablesList;
            this.request = request;
        }

        public void SetCommand(A mainTable, BookContext db)
        {
            relMethod(mainTable).Clear();

            foreach (T secondTable in SecondTablesList)
            {
                T equalsId = request(db).AsEnumerable().FirstOrDefault(
                        k => ReflectionEfId<T>.GetId(k) == ReflectionEfId<T>.GetId(secondTable)
                );

                relMethod(mainTable).Add(equalsId);
            }
        }
    }

    //Управление с файлми
    sealed public class AddWithFileCommand<T> : ICommandVoid where T : class
    {
        static byte[] mainFileByteArray;
        static string path;
        private IFileCommand<T> controlFileCommand;
        public T Obj { get; set; }

        public AddWithFileCommand(IFileCommand<T> fileCommand)
        {
            controlFileCommand = fileCommand;
        }

        public string SetFile(HttpPostedFileBase file, Controller controller)
        {
            string insertPath = "/Content/Images/" + Path.GetFileName(file.FileName);

            if (file != null)
            {
                path = controller.Server.MapPath(string.Format("~{0}", insertPath));
                SetFileByteArray(file);
            }

            return insertPath;
        }
        private void SetFileByteArray(HttpPostedFileBase file)
        {
            mainFileByteArray = new byte[file.ContentLength];
            file.InputStream.Read(mainFileByteArray, 0, file.ContentLength);
        }
        public void SetCommand(BookContext db)
        {
            controlFileCommand.SetCommand(Obj, mainFileByteArray, path, db);
        }
    }
    //Устанавливает Добавить или Изменить таблицу с файлами
    sealed class DefaultEditFile<T> : IFileCommand<T> where T : class
    {
        public void SetCommand(T obj, byte[] file, string path, BookContext db)
        {
            try
            {
                SetAddOrEdit(EntityState.Modified, obj, file, path, db);
            }
            catch
            {
                SetAddOrEdit(EntityState.Added, obj, file, path, db);
            }
        }

        private void SetAddOrEdit(EntityState state , T obj, byte[] file, string path, BookContext db)
        {
            if (file == null)
            {
                db.Entry(obj).State = state;
            }
            else
            {
                DefaultControllFile<T> controllFile = new DefaultControllFile<T>(state);
                controllFile.SetCommand(obj, file, path, db);
            }
        }
    }
    //Реализовывает управление таблицей с файлами
    sealed class DefaultControllFile<T> : IFileCommand<T>
    {
        private EntityState state;

        public DefaultControllFile(EntityState state)
        {
            this.state = state;
        }

        public void SetCommand(T obj, byte[] file, string path, BookContext db)
        {
            if (obj == null)
                throw new NullReferenceException();

            using (MemoryStream memoryStream = new MemoryStream(file))
            {
                using (Image image = Image.FromStream(memoryStream))
                {
                    image.Save(path);
                }
            }

            db.Entry(obj).State = state;
        }
    }
}