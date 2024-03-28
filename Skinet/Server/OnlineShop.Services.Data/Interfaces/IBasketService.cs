using OnlineShop.Models;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBaskedAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
