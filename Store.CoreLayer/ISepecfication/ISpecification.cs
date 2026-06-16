using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public interface ISpecification<T> where T : class
    {
        public List<Expression<Func<T,object>>> Includes { get; set; }
        public Expression<Func<T,bool>> Filter { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public int CountOfAllItem { get; set; }
        public bool IsPaginationEnable { get; set; }

    }
}
