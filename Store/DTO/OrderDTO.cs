namespace Store.DTO
{
    public class OrderDTO
    {
        public string basketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public ShippingAddressDTO Address { get; set; }
    }
}
