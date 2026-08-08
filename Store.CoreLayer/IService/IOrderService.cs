using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, ShippingAddress address);
        Task<Order?> UpdateOrderAsync(string BuyerEmail,int orderId, string BasketId, int DeliveryMethodId, ShippingAddress address);
        Task<IReadOnlyList<Order?>> GetOrdersForSpecificUser(string Email, BaseParams order);
        Task<Order?> GetOrderByIdForSpecificUser(string buyerEmail, int OrderId);
    }
}
