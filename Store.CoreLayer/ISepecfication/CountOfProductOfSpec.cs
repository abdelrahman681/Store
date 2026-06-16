using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class CountOfProductOfSpec : Specification<Product>
    {
        public CountOfProductOfSpec(ProductParams productParamter) : base
            (p => 
            (!productParamter.ProductBrandId.HasValue || p.ProductBrandId == productParamter.ProductBrandId)
            &&
            (!productParamter.ProductCategoryId.HasValue || p.ProductCategoryId == productParamter.ProductCategoryId))
        {
            ApplyPagination(productParamter.PageSize, productParamter.PageSize * (productParamter.PageIndex - 1));
        }
    }
}
