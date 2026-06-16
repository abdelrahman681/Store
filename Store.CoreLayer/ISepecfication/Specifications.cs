using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Store.Repository.Specification
{
    public class Specification<T> : ISpecification<T> where T : class
    {
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, bool>> Filter { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnable { get; set; }
        public int CountOfAllItem { get; set; }

        public Specification()
        {
            
        }
        public Specification(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
        }

        public void ApplyOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy=orderBy;
        }
        public void ApplyOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        public void ApplyPagination(int take,int skip)
        {
            IsPaginationEnable = true;
            Take= take;
            Skip = skip;
        }
    }
}
