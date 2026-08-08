using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class OrderWithSpecification :Specification<Order>
    {
        public OrderWithSpecification(string Email,BaseParams order) :base(o=>(!string.IsNullOrEmpty(Email) ||(o.BuyerEmail== Email)))
        {
            Includes.Add(o => o.Address);
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            ApplyPagination(order.PageSize, order.PageSize * (order.PageIndex - 1));
            ApplyOrderByDesc(o => o.DateOfCreate);
        }
        public OrderWithSpecification(string email,int orderId):base(o=>o.Id==orderId&&o.BuyerEmail==email)
        {
            Includes.Add(o => o.Address);
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
