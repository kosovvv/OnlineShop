using OnlineShop.Data;
using OnlineShop.Models;
namespace OnlineShop.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> EditProduct(int id, Product product);
        Task<bool> DeleteProduct(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(ProductParams productParams);
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<ProductType> GetProductTypeByNameAsync(string name);
        Task<ProductBrand> GetProductBrandByNameAsync(string name);
        Task<int> GetProductsCountAsync(ProductParams productParams);
    }
}
