using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class WishList 
    {
        public string CustomerId { get; set; }

        public AppUser Customer { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
