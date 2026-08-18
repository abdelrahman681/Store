using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public class ItemOrderDTO
    {

        //public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public string PictureUrl { get; set; }
        //public decimal Price { get; set; }
        //public int Quantity { get; set; }
        public int ProductId { get; set; }  
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity Must one at least")]
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
