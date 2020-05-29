using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CyberTigerLibrary.RepositoryPattern
{
    public class BaseSpec<T> where T : class
    {
        public BaseSpec(IList<Expression<Func<T, bool>>> filters = null,
            IList<Expression<Func<T, object>>> includes = null,
            IList<string> stringIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> order = null)
        {
            if (includes != null)
                Includes = includes;
            else
                Includes = new List<Expression<Func<T, object>>>();

            if (stringIncludes != null)
                StringIncludes = stringIncludes;
            else
                StringIncludes = new List<string>();

            if (filters != null)
                Filters = filters;
            else
                Filters = new List<Expression<Func<T, bool>>>();

            if (order != null)
                Order = order;
        }
        public IList<Expression<Func<T, bool>>> Filters { get; set; }
        public IList<Expression<Func<T, object>>> Includes { get; set; }
        public IList<string> StringIncludes { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> Order { get; set; }

        public void Sort(Expression<Func<T, object>> key, string dir = "asc")
        {
            if (dir == "desc")
                Order = q => q.OrderByDescending(key);
            else
                Order = q => q.OrderBy(key);
        }
    }
}
