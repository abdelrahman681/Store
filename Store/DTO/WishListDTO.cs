using Store.CoreLayer.Entirty;

namespace Store.DTO
{
    public class WishListDTO
    {
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public ProductDTO Product { get; set; }

    }
}
