using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.CoreLayer.Entirty;
using Store.Repository.StoreContext;
using System.Text;

namespace Store.ApplicationServices
{
    public static class IdintityAppService
    {
        public static IServiceCollection AddIdintityAppService(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<StoreDbContext>().AddDefaultTokenProviders();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>


            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("JWT Failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });
            Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = configuration["LoginWithGoogle:ClientId"];
        options.ClientSecret = configuration["LoginWithGoogle:ClientSecret"];
    });
            return Services;
        }
    }
}
