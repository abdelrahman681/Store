using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class CreateOrder
    {
        public string BuyerEmail { get; set; }
        public string CartId { get; set; }
        public int DeliveryMethodId { get; set; }
        public ShippingAddress Address { get; set; }
    }
}
