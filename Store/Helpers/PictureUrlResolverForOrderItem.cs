using AutoMapper;
using Store.CoreLayer.Entirty;
using Store.DTO;

namespace Store.Helpers
{
    public class PictureUrlResolverForOrderItem : IValueResolver<ItemOrder, ItemOrderDTO, string>
    {
        private readonly IConfiguration configuration;

        public PictureUrlResolverForOrderItem(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(ItemOrder source, ItemOrderDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;

            return $"{configuration["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
