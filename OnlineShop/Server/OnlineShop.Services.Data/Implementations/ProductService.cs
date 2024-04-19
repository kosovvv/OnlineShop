using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IDeletableEntityRepository<ProductBrand> productBrandRepository;
        private readonly IDeletableEntityRepository<ProductType> productTypeRepository;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.productRepository = (IDeletableEntityRepository<Product>)unitOfWork.GetRepository<Product>();
            this.productBrandRepository = (IDeletableEntityRepository<ProductBrand>)unitOfWork.GetRepository<ProductBrand>();
            this.productTypeRepository = (IDeletableEntityRepository<ProductType>)unitOfWork.GetRepository<ProductType>();
        }

        public async Task<ProductToReturnDto> CreateProduct(ProductToCreateDto product)
        {
            var isProductExisting = await productRepository
                .All()
                .FirstOrDefaultAsync(p => p.Name == product.Name);

            if (isProductExisting != null)
            {
                throw new CreateExistringEntityException("Product with that name already exists.");
            }

            var productToCreate = mapper.Map<ProductToCreateDto, Product>(product);

            var productType = await productTypeRepository
                .All()
                .FirstOrDefaultAsync(x => x.Name == product.ProductType);

            var productBrand = await productBrandRepository
                .All()
                .FirstOrDefaultAsync(x => x.Name == product.ProductBrand);

            if (productType == null || productBrand == null)
            {
                throw new InvalidProductException("Invalid type or brand.");
            }

            productToCreate.ProductBrand = productBrand;
            productToCreate.ProductType = productType;

            await productRepository.AddAsync(productToCreate);

            await unitOfWork.Save();
            return mapper.Map<Product, ProductToReturnDto>(productToCreate);
        }

        public async Task<ProductToReturnDto> EditProduct(int id, ProductToCreateDto product)
        {
            var existingProduct = await productRepository
                .All()
                .FirstOrDefaultAsync(p => p.Name == product.Name);

            if (existingProduct == null)
            {
                throw new EntityNotExistingException("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ProductBrand = await productBrandRepository 
                .All()
                .FirstOrDefaultAsync(x => x.Name == product.ProductBrand);

            existingProduct.ProductType = await productTypeRepository
                .All()
                .FirstOrDefaultAsync(x => x.Name == product.ProductType);

            this.productRepository.Update(existingProduct);
            await unitOfWork.Save();
            return mapper.Map<Product, ProductToReturnDto>(existingProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var productToDelete = await productRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productToDelete == null)
            {
                return false;
            }

            unitOfWork.GetRepository<Product>().Delete(productToDelete);
            await unitOfWork.Save();
            return true;
        }
        public async Task<int> GetProductsCountAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(productRepository.All(), productParams);
            return await query.CountAsync();
        }

        public async Task<ProductToReturnDto> GetProductByIdAsync(int id)
        {
            var product = await productRepository.All().Include(x => x.ProductBrand)
                .Include(x => x.ProductType).Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new EntityNotExistingException("The product you want to review is not existing");
            }

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        public async Task<ICollection<ProductToReturnDto>> GetProductsAsync(ProductParams productParams)
        {
            var query = ApplyProductFilters(productRepository.All(), productParams);
            query = query.Include(x => x.ProductBrand).Include(x => x.ProductType).Include(x => x.Reviews);
            query = ApplyPaging(query, productParams);
            query = ApplySorting(query, productParams);

            var allProducts = await query.ToListAsync();
            return mapper.Map<ICollection<Product>, ICollection<ProductToReturnDto>>(allProducts);
        }
        private static IQueryable<Product> ApplyProductFilters(IQueryable<Product> query, ProductParams productParams)
        {
            return query
                .Where(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue ||x.ProductBrand.Id == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductType.Id == productParams.TypeId)
            );
        }

        private static IQueryable<Product> ApplyPaging(IQueryable<Product> query, ProductParams productParams)
        {
            return query.AsNoTracking()
                        .OrderBy(x => x.Name)
                        .Skip(productParams.PageSize * (productParams.PageIndex - 1))
                        .Take(productParams.PageSize);
        }

        private static IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductParams productParams)
        {
            query = productParams.Sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                "name" => query.OrderBy(x => x.Name),
                _ => query.OrderBy(x => x.Id),
            };
            return query;
        }
    }
}
