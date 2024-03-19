using Skinet.Core.Entities;

namespace Skinet.Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBaskedAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
