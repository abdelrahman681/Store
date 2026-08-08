using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class BrandSpecification:Specification<ProductBrand>
    {
        public BrandSpecification(BrandAndCategoryParams brand):base()
        {
            if(brand.sort is not null)
            {
                switch (brand.sort)
                {
                    case Sorting.NameAsc:
                        ApplyOrderBy(o => o.Name);
                        break;
                        case Sorting.NameDesc:
                        ApplyOrderByDesc(o => o.Name);
                        break;
                }
            }
            ApplyPagination(brand.PageSize,brand.PageSize*(brand.PageIndex-1));
        }
        public BrandSpecification(int brandId) : base(b=>b.Id==brandId)
        {

        }
    }
}
