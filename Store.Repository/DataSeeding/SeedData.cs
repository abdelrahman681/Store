using Store.CoreLayer.Entirty;
using Store.Repository.StoreContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RepositoryLayer.DataSeeding
{
    public static class SeedData
    {
        public async static Task SeedDataAysnc(StoreDbContext dbContext)
        {
            if (!dbContext.Set<ProductBrand>().Any())
            {
                {
                    var productBrand = File.ReadAllText("../Store.Repository/DataSeeding/Data/brands.json");
                    var brandDesrilize = JsonSerializer.Deserialize<List<ProductBrand>>(productBrand);
                    if (brandDesrilize?.Count()>0)
                    {
                        foreach (var brand in brandDesrilize)
                            await dbContext.Set<ProductBrand>().AddAsync(brand);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            if (!dbContext.Set<ProductCategory>().Any())
            {
                {
                    var categoryBrand = File.ReadAllText("../Store.Repository/DataSeeding/Data/categories.json");
                    var categoryDesrilize = JsonSerializer.Deserialize<List<ProductCategory>>(categoryBrand);
                    if (categoryDesrilize?.Count() > 0)
                    {
                        foreach (var category in categoryDesrilize)
                            await dbContext.Set<ProductCategory>().AddAsync(category);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            if (!dbContext.Set<Product>().Any())
            {
                {
                    var Product = File.ReadAllText("../Store.Repository/DataSeeding/Data/products.json");
                    var productDesrilize = JsonSerializer.Deserialize<List<Product>>(Product);
                    if (productDesrilize?.Count() > 0)
                    {
                        foreach (var product in productDesrilize)
                            await dbContext.Set<Product>().AddAsync(product);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            if (!dbContext.Set<DeliveryMethod>().Any())
            {
                {
                    var deliverymethod = File.ReadAllText("../Store.Repository/DataSeeding/Data/delivery.json");
                    var deliveryDesrilize = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymethod);
                    if (deliveryDesrilize?.Count() > 0)
                    {
                        foreach (var delivery in deliveryDesrilize)
                            await dbContext.Set<DeliveryMethod>().AddAsync(delivery);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}