using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class ItemOrder:BaseEntity
    {
        public ItemOrder()
        {

        }
        public ItemOrder(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
