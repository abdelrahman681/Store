using AutoMapper;
using Store.CoreLayer.Entirty;
using Store.DTO;

namespace Store.Helpers
{
    public class MapingProfiles :Profile
    {
        public MapingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ResolverPicture>())
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));

            CreateMap<ProductBrand, ProductDTO>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
            CreateMap<OrderToReturnDTO, Order>().ReverseMap()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<ItemOrder, ItemOrderDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolverForOrderItem>());
            CreateMap<WishList, WishListDTO>().ReverseMap();
            CreateMap<ReviewToReturnDTO, Review>().ReverseMap()
                .ForMember(r=>r.CustomerName,r=>r.MapFrom(s=>s.Customer.DisplayName))
                .ForMember(r=>r.ProductName,r=>r.MapFrom(s=>s.Product.Name));
            CreateMap<NotificationToReturnDTO, Notification>().ReverseMap()
                .ForMember(n=>n.UserName,n=>n.MapFrom(s=>s.User.DisplayName));
        }
    }
}
