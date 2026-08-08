using Store.CoreLayer.Entirty.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order(string buyerEmail, ShippingAddress address, DeliveryMethod deliveryMethod, ICollection<ItemOrder> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset DateOfCreate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.pending;
        public ShippingAddress Address { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<ItemOrder> Items { get; set; } = new List<ItemOrder>();
        public decimal SubTotal { get; set; }
        public string? PaymentIntentId { get; set; }

        public decimal Total()
        => SubTotal + DeliveryMethod.Cost;
    }
}
