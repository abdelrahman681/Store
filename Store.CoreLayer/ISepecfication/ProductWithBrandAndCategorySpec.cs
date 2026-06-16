using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class ProductWithBrandAndCategorySpec : Specification<Product>
    {
        public ProductWithBrandAndCategorySpec(ProductParams product):base
            (p=>(!product.ProductBrandId.HasValue)||(p.ProductBrandId==product.ProductBrandId)&&
            (!product.ProductCategoryId.HasValue)||(p.ProductCategoryId==product.ProductCategoryId))
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
            if (product.sort is not null)
            {
                switch (product.sort)
                {
                    case Sorting.PriceAsc:
                        ApplyOrderBy(o => o.Price);
                        break;
                    case Sorting.PriceDesc: 
                        ApplyOrderByDesc(o => o.Price); 
                        break;
                        case Sorting.NameAsc:
                        ApplyOrderBy(o => o.Name);
                        break;
                        default :
                        ApplyOrderBy(o => o.Name); 
                        break;
                }
            }
            
            ApplyPagination(product.PageSize, product.PageSize * (product.PageIndex - 1));
        }
        public ProductWithBrandAndCategorySpec(int Id):base(o=>o.Id==Id)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
