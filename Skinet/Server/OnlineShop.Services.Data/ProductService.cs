﻿using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;

namespace Skinet.Infrastructure.Data
{
    public class ProductService : IProductService
    {
        private readonly StoreContext context;
        public ProductService(StoreContext context)
        {
            this.context = context;
        }
        public async Task<int> GetProductsCountAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            return await query.CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await context.Products.AsNoTracking().Include(x => x.ProductBrand)
                .Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            query = ApplyPaging(query, productParams);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await context.ProductTypes.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
        {
            return await context.ProductBrands.AsNoTracking().ToListAsync();
        }

        private static IQueryable<Product> ApplyProductFilters(IQueryable<Product> query, ProductParams productParams)
        {
            return query.AsNoTracking().Where(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            );
        }

        private static IQueryable<Product> ApplyPaging(IQueryable<Product> query, ProductParams productParams)
        {
            return query.AsNoTracking().OrderBy(x => x.Name)
                        .Skip(productParams.PageSize * (productParams.PageIndex - 1))
                        .Take(productParams.PageSize);
        }
    }

}
