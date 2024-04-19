using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IDeletableEntityRepository<ProductType> typeRepository;
        public TypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.typeRepository = (IDeletableEntityRepository<ProductType>)this.unitOfWork.GetRepository<ProductType>();
        }

        public async Task<IEnumerable<ReturnProductTypeDto>> GetProductTypesAsync()
        {
            var productTypes = await typeRepository.All().Include(x => x.Products).ThenInclude(x => x.ProductBrand).AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<ReturnProductTypeDto>>(productTypes);
        }

        public async Task<ReturnProductTypeDto> CreateProductTypeAsync(CreateProductTypeDto productType)
        {
            var isProductTypeExisting = await this.typeRepository.All().AnyAsync(x => x.Name == productType.Name);

            if (isProductTypeExisting)
            {
                throw new CreateExistringEntityException("This product type already exists");
            }

            var typeToCreate = this.mapper.Map<CreateProductTypeDto,ProductType>(productType);
            await this.typeRepository.AddAsync(typeToCreate);
            await unitOfWork.Save();
            return mapper.Map<ProductType, ReturnProductTypeDto>(typeToCreate);
        }

        public async Task<ReturnProductTypeDto> UpdateProductTypeAsync(int id, CreateProductTypeDto productType)
        {
            var existingProductType = await typeRepository.All().FirstOrDefaultAsync(p => p.Id == id);

            if (existingProductType == null)
            {
                throw new EntityNotExistingException("Product type not found.");
            }

            existingProductType.Name = productType.Name;
            if (!productType.PictureUrl.IsNullOrEmpty())
            {
                existingProductType.PictureUrl = productType.PictureUrl;
            }

            await unitOfWork.Save();
            return mapper.Map<ProductType, ReturnProductTypeDto>(existingProductType);
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var productTypeToDelete = await typeRepository.All().FirstOrDefaultAsync(p => p.Id == id);

            if (productTypeToDelete == null)
            {
                return false;
            }

            typeRepository.Delete(productTypeToDelete);
            await unitOfWork.Save();
            return true;
        }

        public async Task<ICollection<ProductToReturnDto>> GetProductsByProductTypeIdAsync(int productTypeId)
        {
            var productType = await this.typeRepository.All()
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
