using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    public delegate string StringFormater<T>(T obj);
    //Суперкласс для классов фильтрации
    public abstract class CollectionFilterCommad<T>
    {
        public IQueryable<T> IQueryForFilter { get; set; }
        public CollectionFilterCommad<T> Filter { get; set; }

        public IQueryable<T> SetCommand()
        {
            IQueryForFilter = DoFilter();

            if (Filter != null)
                Filter.IQueryForFilter = IQueryForFilter;

            return IQueryForFilter;
        }
        protected abstract IQueryable<T> DoFilter();
    }
    //Класс для выборки по строке
    public sealed class FilterName<T> : CollectionFilterCommad<T>
    {
        StringFormater<T> searchString;
        string searchText = "";

        public FilterName(StringFormater<T> searchString) : base()
        {
            this.searchString = searchString;
        }
        public void SetSearchText(string searchText)
        {
            this.searchText = searchText;
        }
        protected override sealed IQueryable<T> DoFilter()
        {
            return IQueryForFilter.Where(s => searchString(s).Contains(searchText)); 
        }
    }
    //Класс для выборки по вложенным таблицам A в таблице T
    public sealed class FilterMayny<T,A> : CollectionFilterCommad<T>
    {
        IncludeTables<A, T> bindingTables;
        List<A> ListSecondTables = new List<A>();

        public void SetListSecondTables(List<A> ListSecondTables)
        {
            this.ListSecondTables = ListSecondTables;
        }

        public FilterMayny(IncludeTables<A, T> bindingTables) : base()
        {
            this.bindingTables = bindingTables;
        }

        protected override sealed IQueryable<T> DoFilter()
        {
            return (ListSecondTables == null) ? IQueryForFilter : IQueryForFilter.Where(s => GetEqualsBaseSecondId(s));
        }
        private bool GetEqualsBaseSecondId(T baseTable)
        {
            ICollection<A> ICollectionSecond = bindingTables(baseTable);

            bool stopFlag = true;
            int index = 0;

            bool result = false;

            while (stopFlag){
                if (GetIsEqualsId(ICollectionSecond, index) && index < ListSecondTables.Count - 1)
                    index++;
                else
                {
                    if (GetIsEqualsId(ICollectionSecond, index))
                        result = true;

                    stopFlag = false;
                }
            }

            return result;
        }
        private bool GetIsEqualsId(ICollection<A> ICollectionSecond , int index)
        {
            //Ловим кота в мешке 
            if (ListSecondTables == null)
                return false;

            return ICollectionSecond.Any(
                    mainTable => HasEqualsId(mainTable, ListSecondTables[index])
                );
        }
        private bool HasEqualsId(A second1, A second2)
        {
            return ReflectionEfId<A>.GetId(second1) == ReflectionEfId<A>.GetId(second2);
        }
    }
    //Класс контроллера для объединения множества фильтров в один List
    public sealed class FilterSelectController<T> where T : class
    {
        List<CollectionFilterCommad<T>> listFilterCommand = new List<CollectionFilterCommad<T>>();
        IQueryable<T> IQueryForFilterMain { get; set; }

        public FilterSelectController(IQueryable<T> list)
        {
            IQueryForFilterMain = list;
        }

        public void AddFilter(CollectionFilterCommad<T> someFilter)
        {
            if (listFilterCommand.Count == 0)
                someFilter.IQueryForFilter = IQueryForFilterMain;

            else
                listFilterCommand.LastOrDefault().Filter = someFilter;

            listFilterCommand.Add(someFilter);
        }

        public List<T> DoFilterResult()
        {
            foreach(CollectionFilterCommad<T> filter in listFilterCommand)
            {
                IQueryForFilterMain = filter.SetCommand();
            }

            Pagination.startPoint = 0;

            return PanigationFilter<T>.SetPagination(IQueryForFilterMain);
        }
    }
}