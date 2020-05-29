using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CyberTigerLibrary.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> qry, IList<Expression<Func<T, object>>> includes)
            where T : class
        {

            if (includes != null && includes.Any())
                foreach (var include in includes)
                    qry = qry.Include(include);

            return qry;
        }

        public static IQueryable<T> IncludeString<T>(this IQueryable<T> qry, IList<string> stringIncludes)
            where T : class
        {

            if (stringIncludes != null && stringIncludes.Any())
                foreach (var include in stringIncludes)
                    qry = qry.Include(include);

            return qry;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> qry, IList<Expression<Func<T, bool>>> filters)
        {
            foreach (var filter in filters)
                qry = qry.Where(filter);

            return qry;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> qry, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (orderBy != null)
                qry = orderBy(qry);

            return qry;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> qry, int page, int take)
        {
            if (take > 0)
                return qry.Skip(page * (int)take).Take((int)take);

            return qry;
        }
    }
}
