using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    public delegate IQueryable<T> SearchTableFromContext<T>(BookContext db);
    public delegate ICollection<T> IncludeTables<T,A>(A elem);

    //Интерфейс управления файлами
    public interface IFileCommand<T>
    {
        void SetCommand(T obj, byte[] file, string path, BookContext db);
    }
    public interface ITableInclude<T>
    {
        void SetCommand(T author, BookContext db);
    }
    //Интерфейс команды для Вставки / Удаления / Изменения GOF
    public interface ICommandVoid
    {
        void SetCommand(BookContext db);
    }
    //Интерфейс команды для выборки GOF
    public interface ICommandResult<T>
    {
        IEnumerable<T> SetCommand(BookContext db);
    }
    //Async интерфейс команды для выборки GOF
    public interface ICommandResultAsync<T>
    {
        Task<List<T>> SetCommandAsync(BookContext db);
    }
    //Интерфейс команды для выборки одного элемента GOF
    public interface ICommandSingle<T>
    {
        T SetCommand(BookContext db);
    }


    //Главные классы команды GOF
    sealed public class CommandContext
    {
        //Singleton GOF
        private static CommandContext commandContext;
        private CommandContext() { }
        public static CommandContext getContext()
        {
            if (commandContext == null)
                commandContext = new CommandContext();

            return commandContext;
        }
        //Command GOF
        public IEnumerable<T> SetResultCommand<T>(ICommandResult<T> command)
        {
            using (BookContext db = new BookContext())
            {
                return command.SetCommand(db);
            }
        }
        public List<T> SetResultCommandAsync<T>(ICommandResultAsync<T> command)
        {
            using (BookContext db = new BookContext())
            {
                return command.SetCommandAsync(db).Result;
            }
        }
        //Методы для самых частых запросов
        public IEnumerable<F> DoSelectCommand<F>() where F : class
        {
            SelectCommand<F> select = new SelectCommand<F>();
            return SetResultCommand(select);
        }
        public IEnumerable<F> DoSelectCommandAsync<F>() where F : class
        {
            SelectCommandAsycn<F> select = new SelectCommandAsycn<F>();

            return SetResultCommandAsync(select);
        }
        public IEnumerable<F> DoSelectCommand<F>(SearchTableFromContext<F> selector,bool pagination = false) where F : class
        {
            if (selector == null)
                throw new NullReferenceException();

            SelectCommand<F> select = new SelectCommand<F>(selector, pagination);

            return SetResultCommand(select);
        }
        public IEnumerable<F> DoSelectCommandAsync<F>(SearchTableFromContext<F> selector, bool pagination = false) where F : class
        {
            if (selector == null)
                throw new NullReferenceException();

            SelectCommandAsycn<F> select = new SelectCommandAsycn<F>(selector, pagination);

            return SetResultCommandAsync(select);
        }
        public IQueryable<F> DoSelectCommandQuery<F>(SearchTableFromContext<F> selector) where F : class
        {
            SelectCommand<F> select =  new SelectCommand<F>(selector);

            return SetResultCommand(select).AsQueryable();
        }
    }
    sealed public class CommandVoidContext
    {
        private static CommandVoidContext commandContext;
        private CommandVoidContext() { }
        public static CommandVoidContext getContext()
        {
            if (commandContext == null)
                commandContext = new CommandVoidContext();

            return commandContext;
        }
        public void SetVoidCommand(ICommandVoid command)
        {
            using (BookContext db = new BookContext())
            {
                command.SetCommand(db);
                db.SaveChanges();
            }
        }
        public void DoAddCommand<F>(F obj, EntityState state) where F : class
        {
            AddCommand<F> commamnd = new AddCommand<F>(obj) { EntityState = state };
            SetVoidCommand(commamnd);
        }
    }
    sealed public class CommandSingleContext
    {
        private static CommandSingleContext commandContext;
        private CommandSingleContext() { }
        public static CommandSingleContext getContext()
        {
            if (commandContext == null)
                commandContext = new CommandSingleContext();

            return commandContext;
        }
        public T SetResultCommand<T>(ICommandSingle<T> command)
        {
            using (BookContext db = new BookContext())
            {
                return command.SetCommand(db);
            }
        }
        public F DoSingleCommand<F>(SearchTableFromContext<F> selector, int id) where F : class
        {
            SingleController<F> singleCommand = new SingleController<F>(selector, id);
            return SetResultCommand(singleCommand);
        }
    }
}
