using OnlineShop.Data;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IProductService
    {
        Task<ProductToReturnDto> CreateProduct(ProductToCreateDto product);
        Task<ProductToReturnDto> EditProduct(int id, ProductToCreateDto product);
        Task<bool> DeleteProduct(int id);
        Task<ProductToReturnDto> GetProductByIdAsync(int id);
        Task<ICollection<ProductToReturnDto>> GetProductsAsync(ProductParams productParams);
        Task<int> GetProductsCountAsync(ProductParams productParams);
    }
}
