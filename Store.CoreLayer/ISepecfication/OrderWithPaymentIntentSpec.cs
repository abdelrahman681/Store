using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class OrderWithPaymentIntentSpec : Specification<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntetntId):base(o=>o.PaymentIntentId==paymentIntetntId)
        {
        }
    }
}
