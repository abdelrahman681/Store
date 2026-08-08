using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface IPayment
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentStatus(string paymentIntent, bool flag);
    }
}
