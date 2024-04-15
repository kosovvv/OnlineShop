using OnlineShop.Models;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface ITypeService
    {
        Task<IEnumerable<ReturnProductTypeDto>> GetProductTypesAsync();
        Task<ReturnProductTypeDto> CreateProductTypeAsync(CreateProductTypeDto productType);
        Task<ReturnProductTypeDto> UpdateProductTypeAsync(int id, CreateProductTypeDto productType);
        Task<bool> DeleteProductTypeAsync(int productTypeId);
        Task<ICollection<ProductToReturnDto>> GetProductsByProductTypeIdAsync(int productTypeId);

    }
}
