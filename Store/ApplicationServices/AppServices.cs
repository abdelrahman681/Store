using E_Commerce.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StackExchange.Redis;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Mail;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.IService;
using Store.CoreLayer.IServices;
using Store.CoreLayer.IUnitOfWork;
using Store.Helpers;
using Store.Repository.GenericRepository;
using Store.Repository.StoreContext;
using Store.Repository.UnitOfWork;
using Store.Service;
using System.Text.Json.Serialization;

namespace Store.ApplicationServices
{
    public static class AppServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            Services.AddSignalR();
            Services.Configure<MailSetting>(configuration.GetSection("MaillSetting"));
            Services.AddScoped<INotificationService, NotificationService>();
            Services.AddScoped<IResponseCacheService, ResponseCacheService>();
            Services.AddScoped<IReviewService, ReviewService>();
            Services.AddScoped<IPayment, Payment>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IWishListRepository, WishListRepository>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IEmailSetting, EmailSetting>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddAutoMapper(cfg =>
            {
            }, typeof(MapingProfiles).Assembly);
            Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter());
            });
            Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Any())
                                              .SelectMany(p => p.Value.Errors)
                                              .Select(e => e.ErrorMessage).ToList();

                    var ApiValidationRespons = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ApiValidationRespons);
                };
            });
            Services.AddControllers();
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer Token"
                });
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            Services.AddOpenApi();

            Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefulatConnectionString"));

            });
            Services.AddSingleton<IConnectionMultiplexer>(option =>
            {
                var connection = configuration.GetConnectionString("RediasConnection");
                return ConnectionMultiplexer.Connect(connection);
            });
            Services.AddCors(options =>
            {
                options.AddPolicy("Angular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            return Services;
        }
    }
}
