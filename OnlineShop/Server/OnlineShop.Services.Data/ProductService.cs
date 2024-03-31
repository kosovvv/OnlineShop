using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using System.Security.Policy;

namespace Skinet.Infrastructure.Data
{
    public class ProductService : IProductService
    {
        private readonly StoreContext context;
        public ProductService(StoreContext context)
        {
            this.context = context;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var isProductExisting = await this.context.Products.SingleOrDefaultAsync(p => p.Name == product.Name);

            if (isProductExisting == null)
            {
                await this.context.Products.AddAsync(product);
                await this.context.SaveChangesAsync();
                return product;
            }

            return null;
        }

        public async Task<Product> EditProduct(int id, Product product)
        {
            var existingProduct = await this.GetProductByIdAsync(id);
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ProductBrand = product.ProductBrand;
            existingProduct.ProductType = product.ProductType;
 
            await this.context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var productToDelete = await this.GetProductByIdAsync(id);

            if (productToDelete == null)
            {
                return false;
            }

            try
            {
                this.context.Products.Remove(productToDelete);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> GetProductsCountAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            return await query.CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await context.Products.Include(x => x.ProductBrand)
                .Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            query = ApplyPaging(query, productParams);
            query = query.Include(x => x.ProductBrand).Include(x => x.ProductType);

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
        public async Task<ProductType> GetProductTypeByNameAsync(string name)
        {
            return await context.ProductTypes.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<ProductBrand> GetProductBrandByNameAsync(string name)
        {
            return await context.ProductBrands.FirstOrDefaultAsync(x => x.Name == name);
        }

        private static IQueryable<Product> ApplyProductFilters(IQueryable<Product> query, ProductParams productParams)
        {
            return query.AsNoTracking().Where(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrand.Id == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductType.Id == productParams.TypeId)
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
