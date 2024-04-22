using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IDeletableEntityRepository<ProductBrand> brandRepository;

        public BrandService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.brandRepository = (IDeletableEntityRepository<ProductBrand>)unitOfWork.GetRepository<ProductBrand>();
        }

        public async Task<IEnumerable<ReturnProductBrandDto>> GetProductBrandsAsync()
        {
            var productBrands = await brandRepository.All().Include(x => x.Products).AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ReturnProductBrandDto>>(productBrands);
        }

        public async Task<ReturnProductBrandDto> CreateProductBrandAsync(CreateProductBrandDto productBrand)
        {
            var isBrandExisting = await this.brandRepository.All().AnyAsync(x => x.Name == productBrand.Name);

            if (isBrandExisting)
            {
                throw new CreateExistringEntityException(typeof(ProductBrand));
            }

            var brandToCreate = mapper.Map<CreateProductBrandDto, ProductBrand>(productBrand);
            await this.brandRepository.AddAsync(brandToCreate);
            await unitOfWork.Save();

            return this.mapper.Map<ProductBrand, ReturnProductBrandDto>(brandToCreate);
        }

        public async Task<ReturnProductBrandDto> UpdateProductBrandAsync(int id, CreateProductBrandDto productBrand)
        {
            var existingBrand = await brandRepository.All().FirstOrDefaultAsync(p => p.Id == id);

            if (existingBrand == null)
            {
                throw new EntityNotExistingException(typeof(ProductBrand));
            }

            existingBrand.Name = productBrand.Name;
            if (!productBrand.PictureUrl.IsNullOrEmpty())
            {
                existingBrand.PictureUrl = productBrand.PictureUrl;
            }
            brandRepository.Update(existingBrand);
            await unitOfWork.Save();

            return this.mapper.Map<ProductBrand, ReturnProductBrandDto>(existingBrand);
        }

        public async Task<bool> DeleteProductBrandAsync(int id)
        {
            var productBrandToDelete = await brandRepository.All().FirstOrDefaultAsync(p => p.Id == id);

            if (productBrandToDelete == null)
            {
                return false;
            }

            brandRepository.Delete(productBrandToDelete);
            await unitOfWork.Save();

            return true;
        }

        public async Task<ICollection<ProductToReturnDto>> GetProductsByProductBrandIdAsync(int productBrandId)
        {
            var productBrand = await brandRepository.All()
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == productBrandId);

            if (productBrand == null)
            {
                throw new EntityNotExistingException(typeof(ProductBrand));
            }

            return mapper.Map<ICollection<Product>, ICollection<ProductToReturnDto>>(productBrand.Products);
        }
    }
}
