using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecification<T> spec)
        {
            var query = inputQuery;
            //spec.CountOfAllItem = query.Count();
            if (spec.Filter is not null)
                query = query.Where(spec.Filter);
            if(spec.OrderBy is not null)
                query=query.OrderBy(spec.OrderBy);
            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            if(spec.IsPaginationEnable)
                query=query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (currentInclud, nextInclude) => currentInclud.Include(nextInclude));
            return query;
        }
    }
}
