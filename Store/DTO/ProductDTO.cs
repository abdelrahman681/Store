using Store.CoreLayer.Entirty;
using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public record ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public decimal Price { get; set; }
        //public int? ProductBrandId { get; set; }
        //public int? ProductCategoryId { get; set; }
        public int StockQuantity { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}
