using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using QuestPDF.Infrastructure;
using RepositoryLayer.DataSeeding;
using StackExchange.Redis;
using Store.ApplicationServices;
using Store.Helpers;
using Store.Middlewares;
using Store.Repository.StoreContext;
using Store.Service;

namespace Store
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ApplyServices(builder.Configuration);
            builder.Services.AddIdintityAppService(builder.Configuration);
            var app = builder.Build();
            #region Update DateBase Outo
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var LoggeFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<StoreDbContext>();
                await context.Database.MigrateAsync();
                await SeedData.SeedDataAysnc(context);
            }
            catch (Exception ex)
            {
                var logger = LoggeFactory.CreateLogger<Program>();
                logger.LogError(ex, ex.Message);
                //logger.LogError(ex, "An error has been  occured during apply the migration ");
            }
           #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseCors("Angular");
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");
            app.Run();
        }
    }
}
