using OnlineShop.Models;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Brand;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<ReturnProductBrandDto>> GetProductBrandsAsync();
        Task<ProductBrand> CreateProductBrandAsync(CreateProductBrandDto productBrand);
        Task<ProductBrand> UpdateProductBrandAsync(int id, CreateProductBrandDto productBrand);
        Task<bool> DeleteProductBrandAsync(int productBrand);
        Task<ICollection<ProductToReturnDto>> GetProductsByProductBrandIdAsync(int productBrandId);
    }
}
