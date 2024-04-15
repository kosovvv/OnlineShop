using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<ProductBrandDto>> GetProductBrandsAsync();
    }
}
