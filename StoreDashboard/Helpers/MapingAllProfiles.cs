using AutoMapper;
using DashBoard.Models;
using Store.CoreLayer.Entirty;

namespace StoreDashboard.Helpers
{
    public class MapingAllProfiles:Profile
    {
        public MapingAllProfiles()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap().ForMember(d=>d.Brand,o=>o.MapFrom(s=>s.Brand));
        }
    }
}
