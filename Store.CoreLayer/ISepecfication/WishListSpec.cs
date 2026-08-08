using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Specification
{
    public class WishListSpec : Specification<WishList>
    {
        public WishListSpec(string customerId,WishListSpecParamter paramter) : base(p=>p.CustomerId==customerId)
        {
            ApplyOrderByDesc(x => x.CreatedAt);
            ApplyPagination(paramter.PageSize,paramter.PageSize*(paramter.PageIndex-1));
            Includes.Add(x => x.Product);
            Includes.Add(x => x.Product.Brand);
            Includes.Add(x => x.Product.Category);
        }
        public WishListSpec(string customerId, int productId) : base(x => x.CustomerId == customerId && x.ProductId == productId)
        {

        }
    }
}
