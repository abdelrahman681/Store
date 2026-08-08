
using Store.CoreLayer.Entirty;
using System.ComponentModel.DataAnnotations;

namespace DashBoard.Models
{
    public class ProductViewModel
    {
        public ProductBrand? Brand { get; set; }
        [Required(ErrorMessage = "ProductBrandId is Required")]
        public int? ProductBrandId { get; set; }

        public ProductCategory? Category { get; set; }
        [Required(ErrorMessage = "ProductCategoryId is Required")]
        public int? ProductCategoryId { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(1, 100000)]
        public decimal Price { get; set; }
    }
}
