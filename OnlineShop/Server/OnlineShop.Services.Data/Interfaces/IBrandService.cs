using OnlineShop.Models;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Brand;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<ReturnProductBrandDto>> GetProductBrandsAsync();
        Task<ReturnProductBrandDto> CreateProductBrandAsync(CreateProductBrandDto productBrand);
        Task<ReturnProductBrandDto> UpdateProductBrandAsync(int id, CreateProductBrandDto productBrand);
        Task<bool> DeleteProductBrandAsync(int productBrand);
        Task<ICollection<ProductToReturnDto>> GetProductsByProductBrandIdAsync(int productBrandId);
    }
}
