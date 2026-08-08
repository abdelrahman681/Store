using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public class BasketItemDTO
    {
        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="The Id must be Greater than one")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="The Price Must be greater then 0")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity Must one at least")]
        public int Quantity { get; set; }
    }
}
