using Skinet.Core.Entities;

namespace Skinet.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
    }
}
