namespace Store.DTO
{
    public class UpdateOrderDTO
    {
        //public int orderId { get; set; }
        public int DeliveryMethodId { get; set; }
        public string basketId { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }
        public List<ItemOrderDTO> Items { get; set; }
    }
}
