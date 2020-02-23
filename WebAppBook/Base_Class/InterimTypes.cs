using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    //Отражение Id Таблицы Ef
    static class ReflectionEfId<T>
    {
        public static int GetId(T obj)
        {
            Type type = typeof(T);

            return (int)type.GetProperty("Id").GetValue(obj);
        }
    }
    //Пагинация для таблиц
    public static class Pagination
    {
        const ushort takeCount = 6;
        public static ushort startPoint { get; set; } = 0;
        public static int count { get; private set; }

        static public List<T> SetPagination<T>(IQueryable<T> unPagination) where T : class
        {
            SetPageSize(unPagination.Count());

            return unPagination.OrderBy(s=> s.Equals(s))
                .Skip(startPoint * takeCount)
                .Take(takeCount)
                .ToList();
        }
        static async public Task<List<T>> SetPaginationAsync<T>(IQueryable<T> unPagination) where T : class
        {
            SetPageSize(unPagination.Count());

            return await unPagination.OrderBy(s => s.Equals(s))
                .Skip(startPoint * takeCount)
                .Take(takeCount)
                .ToListAsync();
        }

        static void SetPageSize(double requestCount)
        {
            double pageCount = requestCount / (double)takeCount;

            if (pageCount < 1)
                count = (int)Math.Floor(pageCount);
            else
                count = (int)Math.Ceiling(pageCount);
        }
    }
    public static class PanigationFilter<T> where T : class
    {
        static IQueryable<T> unPagination;
        static public List<T> SetPagination(IQueryable<T> unPagination)
        {
            PanigationFilter<T>.unPagination = unPagination;

            return Pagination.SetPagination(unPagination);
        }

        static public List<T> GetPagination()
        {
            if (unPagination == null)
                return null;

            return Pagination.SetPagination(unPagination);
        }
    }
    //Изменить на Enum
    sealed public class SelectDefault
    {
        public readonly SearchTableFromContext<Book> Book;
        public readonly SearchTableFromContext<Author> Author;
        public readonly SearchTableFromContext<Publisher> Publisher;

        public SelectDefault()
        {
            Book = s =>
                s.Books.Include("Translators")
                .Include("Genres")
                .Include("Languages")
                .Include("Author");

            Author = s => s.Authors.Include("Books").Include("Genres");

            Publisher = s => s.Publishers.Include("Books");
        }
    }  
}