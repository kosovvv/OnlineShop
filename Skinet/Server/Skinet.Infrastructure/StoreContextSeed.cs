﻿using OnlineShop.Data;
using Skinet.Core.Entities;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.Infrastructure.Data;
using System.Text.Json;

namespace Skinet.Infrastructure
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Skinet.Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Skinet.Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Skinet.Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Skinet.Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                context.DeliveryMethods.AddRange(methods);
                await context.SaveChangesAsync();
            }



            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
