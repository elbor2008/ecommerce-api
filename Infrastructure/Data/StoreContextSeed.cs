using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedStoreAsync(StoreContext storeContext, ILoggerFactory loggerFactory)
        {
            try
            {
                await SeedProductTypesAsync(storeContext);
                await SeedProductBrandsAsync(storeContext);
                await SeedProductsAsync(storeContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, "An error occurred during data seeding");
            }
        }
        private static async Task SeedProductTypesAsync(StoreContext storeContext)
        {
            if (!await storeContext.productTypes.AnyAsync())
            {
                var data = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                IEnumerable<ProductType> productTypes = JsonSerializer.Deserialize<IEnumerable<ProductType>>(data);
                await storeContext.productTypes.AddRangeAsync(productTypes);
                await storeContext.SaveChangesAsync();
            }
        }
        private static async Task SeedProductBrandsAsync(StoreContext storeContext)
        {
            if (!await storeContext.productBrands.AnyAsync())
            {
                var data = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var productBrands = JsonSerializer.Deserialize<IEnumerable<ProductBrand>>(data);
                await storeContext.productBrands.AddRangeAsync(productBrands);
                await storeContext.SaveChangesAsync();
            }
        }
        private static async Task SeedProductsAsync(StoreContext storeContext)
        {
            if (!await storeContext.Products.AnyAsync())
            {
                var data = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
                await storeContext.Products.AddRangeAsync(products);
                await storeContext.SaveChangesAsync();
            }
        }
    }
}
