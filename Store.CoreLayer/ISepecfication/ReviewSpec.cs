using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class ReviewSpec:Specification<Review>
    {
        public ReviewSpec(ReviewParameter parameter) : base(r => r.ProductId == parameter.ProductId)
        {
            Includes.Add(r => r.Customer);
            Includes.Add(r => r.Product);
            ApplyOrderByDesc(r => r.CreatedAt);
            ApplyPagination(parameter.PageSize, parameter.PageSize * (parameter.PageIndex - 1));
        }
        public ReviewSpec(string? customerId,int ProductId):base(r=>(string.IsNullOrEmpty(customerId)||r.CustomerId==customerId)&&r.ProductId==ProductId)
        {
            
        }
        public ReviewSpec(int Id, string customerId) : base(r => r.CustomerId == customerId && r.Id == Id)
        {

        }
    }
}
