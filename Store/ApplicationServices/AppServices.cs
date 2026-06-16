using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.IUnitOfWork;
using Store.Helpers;
using Store.Repository.GenericRepository;
using Store.Repository.UnitOfWork;
using System.Text.Json.Serialization;

namespace Store.ApplicationServices
{
    public static class AppServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(cfg =>
            {
            }, typeof(MapingProfiles).Assembly);
            Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter());
            });
            return Services;
        }
    }
}
