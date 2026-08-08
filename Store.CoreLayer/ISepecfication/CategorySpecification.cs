using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class CategorySpecification :Specification<ProductCategory>
    {
        public CategorySpecification(BrandAndCategoryParams @params) : base()
        {
            if (@params.sort is not null)
            {
                switch (@params.sort)
                {
                    case Sorting.NameAsc:
                        ApplyOrderBy(o => o.Name);
                        break;
                    case Sorting.NameDesc:
                        ApplyOrderByDesc(o => o.Name);
                        break;
                }
            }
            ApplyPagination(@params.PageSize, @params.PageSize * (@params.PageIndex - 1));
        }
        public CategorySpecification(int categoryId) : base(b => b.Id == categoryId)
        {

        }
    }
}
