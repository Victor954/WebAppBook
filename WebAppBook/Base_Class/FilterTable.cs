using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    public interface IFilterPart<T> where T : class
    {
        List<CollectionFilterCommad<T>> GetFilter();
    }
    public sealed class ConcreteFilter<T> : IFilterPart<T> where T : class
    {
        List<CollectionFilterCommad<T>> listFilters = new List<CollectionFilterCommad<T>>();

        public void SetMany<F>(IncludeTables<F, T> include, List<F> listMany) where F : class
        {
            FilterMayny<T, F> filterByMany = new FilterMayny<T, F>(include);
            filterByMany.SetListSecondTables(listMany);

            listFilters.Add(filterByMany);
        }
        public void SetSearchByName(StringFormater<T> searchString, string search)
        {
            FilterName<T> filterByName = new FilterName<T>(searchString);
            filterByName.SetSearchText(search);

            listFilters.Add(filterByName);
        }

        public List<CollectionFilterCommad<T>> GetFilter()
        {
            return listFilters;
        }
    }
    public sealed class FilterContext<T> where T : class
    {
        FilterSelectController<T> controllerFilter;
        List<CollectionFilterCommad<T>> listFilters;
        static IQueryable<T> requestList;

        public static void SetForFiltering(IQueryable<T> requestList)
        {
            FilterContext<T>.requestList = requestList;
        }
        public void SetFilterResult(IFilterPart<T> filter)
        {
            if (requestList == null)
                throw new NullReferenceException();

            listFilters = filter.GetFilter();

            DoAllFilter();

            listFilters.Clear();
        }
        private void DoAllFilter()
        {
            controllerFilter = new FilterSelectController<T>(requestList);
            listFilters.ForEach(connection => controllerFilter.AddFilter(connection));

            controllerFilter.DoFilterResult();
        }
    }
}