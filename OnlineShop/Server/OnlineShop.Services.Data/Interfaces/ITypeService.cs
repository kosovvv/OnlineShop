using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface ITypeService
    {
        Task<IEnumerable<ProductTypeDto>> GetProductTypesAsync();
    }
}
