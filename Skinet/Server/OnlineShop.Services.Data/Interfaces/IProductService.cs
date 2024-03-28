using Core.Specifications;
using Skinet.Core.Entities;

namespace Skinet.Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(ProductSpecParams productParams);
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<int> GetProductsCountAsync(ProductSpecParams productParams);
    }
}
