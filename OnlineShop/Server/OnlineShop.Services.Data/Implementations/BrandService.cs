using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Brand;

namespace OnlineShop.Services.Data.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IMapper mapper;
        private readonly StoreContext context;

        public BrandService(IMapper mapper, StoreContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ReturnProductBrandDto>> GetProductBrandsAsync()
        {
            var productBrands = await context.ProductBrands.AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ReturnProductBrandDto>>(productBrands);
        }

        public async Task<ProductBrand> CreateProductBrandAsync(CreateProductBrandDto productBrand)
        {
            var isBrandExisting = await this.context.ProductBrands.AnyAsync(x => x.Name == productBrand.Name);

            if (isBrandExisting)
            {
                throw new CreateExistringEntityException("This product brand already exists");
            }

            var brandToCreate = mapper.Map<CreateProductBrandDto, ProductBrand>(productBrand);
            await this.context.ProductBrands.AddAsync(brandToCreate);
            await this.context.SaveChangesAsync();

            return brandToCreate;
        }

        public async Task<ProductBrand> UpdateProductBrandAsync(int id, CreateProductBrandDto productBrand)
        {
            var existingBrand = await context.ProductBrands.FirstOrDefaultAsync(p => p.Id == id);

            if (existingBrand == null)
            {
                throw new EntityNotExistingException("Product brand not found.");
            }

            existingBrand.Name = productBrand.Name;
            await context.SaveChangesAsync();

            return existingBrand;
        }

        public async Task<bool> DeleteProductBrandAsync(int id)
        {
            var productBrandToDelete = await context.ProductBrands.FirstOrDefaultAsync(p => p.Id == id);

            if (productBrandToDelete == null)
            {
                return false;
            }

            context.ProductBrands.Remove(productBrandToDelete);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<ProductToReturnDto>> GetProductsByProductBrandIdAsync(int productBrandId)
        {
            var productBrand = await context.ProductBrands
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == productBrandId);

            if (productBrand == null)
            {
                throw new EntityNotExistingException("This product brand is not existring.");
            }

            return mapper.Map<ICollection<Product>, ICollection<ProductToReturnDto>>(productBrand.Products);
        }
    }
}
