using OnlineShop.Data;
using OnlineShop.Models;
namespace OnlineShop.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(ProductParams productParams);
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<int> GetProductsCountAsync(ProductParams productParams);
    }
}
