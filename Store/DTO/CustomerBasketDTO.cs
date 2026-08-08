
namespace Store.DTO
{
    public class CustomerBasketDTO
    {
        public CustomerBasketDTO(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public int DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public List<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();
    }
}
