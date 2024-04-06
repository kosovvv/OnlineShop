using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;

namespace Skinet.Infrastructure.Data
{
    public class ProductService : IProductService
    {
        private readonly StoreContext context;
        private readonly IMapper mapper;
        public ProductService(StoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ProductToReturnDto> CreateProduct(ProductToCreateDto product)
        {
            var isProductExisting = await this.context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
            var productToCreate = this.mapper.Map<ProductToCreateDto, Product>(product);

            if (isProductExisting == null)
            {
                await this.context.Products.AddAsync(productToCreate);
                await this.context.SaveChangesAsync();
                return this.mapper.Map<Product, ProductToReturnDto>(productToCreate);
            }

            return null;
        }

        public async Task<ProductToReturnDto> EditProduct(int id, ProductToCreateDto product)
        {
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ProductBrand = await this.context.ProductBrands.FirstOrDefaultAsync(x => x.Name == product.Name);
            existingProduct.ProductType = await this.context.ProductTypes.FirstOrDefaultAsync(x => x.Name == product.Name);

            await this.context.SaveChangesAsync();
            return this.mapper.Map<Product, ProductToReturnDto>(existingProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var productToDelete = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

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

        public async Task<ProductToReturnDto> GetProductByIdAsync(int id)
        {
            var product = await context.Products.Include(x => x.ProductBrand)
                .Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            query = ApplyPaging(query, productParams);
            query = query.Include(x => x.ProductBrand).Include(x => x.ProductType);
            var products = await query.ToListAsync();

            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetProductTypesAsync()
        {
            var productTypes = await context.ProductTypes.AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<ProductTypeDto>>(productTypes);
        }
        public async Task<IEnumerable<ProductBrandDto>> GetProductBrandsAsync()
        {
            var productTypes = await context.ProductBrands.AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandDto>>(productTypes);
        }
        public async Task<ProductTypeDto> GetProductTypeByNameAsync(string name)
        {
            var types = await context.ProductTypes.FirstOrDefaultAsync(x => x.Name == name);
            return this.mapper.Map<ProductType, ProductTypeDto>(types);
        }

        public async Task<ProductBrandDto> GetProductBrandByNameAsync(string name)
        {
            var brands = await context.ProductBrands.FirstOrDefaultAsync(x => x.Name == name);
            return this.mapper.Map<ProductBrand, ProductBrandDto>(brands);
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
