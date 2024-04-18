using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Data.Implementations
{
    public class TypeService : ITypeService
    {
        private readonly IMapper mapper;
        private readonly StoreContext context;
        public TypeService(StoreContext context, IMapper mapper)
        {
            this.context= context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ReturnProductTypeDto>> GetProductTypesAsync()
        {
            var productTypes = await context.ProductTypes.Include(x => x.Products).ThenInclude(x => x.ProductBrand).AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<ReturnProductTypeDto>>(productTypes);
        }

        public async Task<ReturnProductTypeDto> CreateProductTypeAsync(CreateProductTypeDto productType)
        {
            var isProductTypeExisting = await this.context.ProductTypes.AnyAsync(x => x.Name == productType.Name);

            if (isProductTypeExisting)
            {
                throw new CreateExistringEntityException("This product type already exists");
            }

            var typeToCreate = this.mapper.Map<CreateProductTypeDto,ProductType>(productType);
            await this.context.ProductTypes.AddAsync(typeToCreate);
            await this.context.SaveChangesAsync();
            return mapper.Map<ProductType, ReturnProductTypeDto>(typeToCreate);
        }

        public async Task<ReturnProductTypeDto> UpdateProductTypeAsync(int id, CreateProductTypeDto productType)
        {
            var existingProductType = await context.ProductTypes.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProductType == null)
            {
                throw new EntityNotExistingException("Product type not found.");
            }

            existingProductType.Name = productType.Name;
            if (!productType.PictureUrl.IsNullOrEmpty())
            {
                existingProductType.PictureUrl = productType.PictureUrl;
            }
            
            await context.SaveChangesAsync();
            return mapper.Map<ProductType, ReturnProductTypeDto>(existingProductType);
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var productTypeToDelete = await context.ProductTypes.FirstOrDefaultAsync(p => p.Id == id);

            if (productTypeToDelete == null)
            {
                return false;
            }

            context.ProductTypes.Remove(productTypeToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<ProductToReturnDto>> GetProductsByProductTypeIdAsync(int productTypeId)
        {
            var productType = await this.context.ProductTypes
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == productTypeId);

            if (productType == null)
            {
                throw new EntityNotExistingException("This product type does not exists");
            }

            return this.mapper.Map<ICollection<Product>, ICollection<ProductToReturnDto>>(productType.Products);
        }


    }
}
