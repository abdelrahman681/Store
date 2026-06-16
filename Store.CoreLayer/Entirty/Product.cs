using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public ProductBrand Brand { get; set; }
        public int? ProductBrandId { get; set; }
        public ProductCategory Category { get; set; }
        public int? ProductCategoryId { get; set; }


    }
}
