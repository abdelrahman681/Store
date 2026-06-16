using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;

namespace Store.Helpers
{
    public class ResolverPicture:IValueResolver<Product,ProductDTO,string>
    {
        #region Field
        private readonly IConfiguration configuration;

        #endregion

        #region Ctor
        public ResolverPicture(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        #endregion
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl))
                return string.Empty;

          return $"{configuration["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
