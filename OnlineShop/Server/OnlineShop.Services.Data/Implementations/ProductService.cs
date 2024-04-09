using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Implementations
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
            var isProductExisting = await context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);

            if (isProductExisting != null)
            {
                return null;
            }

            var productToCreate = mapper.Map<ProductToCreateDto, Product>(product);

            var productType = await context.ProductBrands.FirstOrDefaultAsync(x => x.Name == product.ProductBrand);
            var productBrand = await context.ProductTypes.FirstOrDefaultAsync(x => x.Name == product.ProductType);

            if (productType == null || productBrand == null)
            {
                return null;
            }

            productToCreate.ProductBrand = productType;
            productToCreate.ProductType = productBrand;

            await context.Products.AddAsync(productToCreate);
            await context.SaveChangesAsync();
            return mapper.Map<Product, ProductToReturnDto>(productToCreate);
        }

        public async Task<ProductToReturnDto> EditProduct(int id, ProductToCreateDto product)
        {
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ProductBrand = await context.ProductBrands.FirstOrDefaultAsync(x => x.Name == product.ProductBrand);
            existingProduct.ProductType = await context.ProductTypes.FirstOrDefaultAsync(x => x.Name == product.ProductType);

            await context.SaveChangesAsync();
            return mapper.Map<Product, ProductToReturnDto>(existingProduct);
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
                context.Products.Remove(productToDelete);
                await context.SaveChangesAsync();
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

        public async Task<ICollection<ProductToReturnDto>> GetProductsAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(context.Products, productParams);
            query = ApplyPaging(query, productParams);
            query = query.Include(x => x.ProductBrand).Include(x => x.ProductType);
            var products = await query.ToListAsync();

            var result = mapper.Map<ICollection<Product>, ICollection<ProductToReturnDto>>(products);
            return result;
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
        private async Task<ProductType> GetProductTypeByNameAsync(string name)
        {
            return await context.ProductTypes.FirstOrDefaultAsync(x => x.Name == name);
        }

        private async Task<ProductBrand> GetProductBrandByNameAsync(string name)
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
