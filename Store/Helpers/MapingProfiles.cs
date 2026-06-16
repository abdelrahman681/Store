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
        }
    }
}
