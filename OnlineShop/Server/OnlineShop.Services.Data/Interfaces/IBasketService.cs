using OnlineShop.Models;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBasketAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasketDto basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
