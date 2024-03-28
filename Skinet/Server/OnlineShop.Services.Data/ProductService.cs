using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.Infrastructure.Data
{
    public class ProductService : IProductService
    {
        private readonly StoreContext context;
        public ProductService(StoreContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
        {
            return await context.ProductBrands.ToListAsync();
        }

        public async Task<int> GetProductsCountAsync(ProductSpecParams productParams)
        {
            return await context.Products.Where(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)).CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await context.Products.Include(x => x.ProductBrand)
                .Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductSpecParams productParams)
        {
            return await context.Products.Include(x => x.ProductBrand)
                .Include(x => x.ProductType).Where(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            ).OrderBy(x => x.Name).Skip(productParams.PageSize * (productParams.PageIndex - 1))
            .Take(productParams.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await context.ProductTypes.ToListAsync();
        }
    }
}
