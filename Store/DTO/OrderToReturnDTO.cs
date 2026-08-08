using Store.CoreLayer.Entirty;

namespace Store.DTO
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public string DateOfCreate { get; set; } 
        public string Status { get; set; } 
        public ShippingAddress Address { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryMethodCost { get; set; }
        public ICollection<ItemOrderDTO> Items { get; set; } = new List<ItemOrderDTO>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }

    }
}
