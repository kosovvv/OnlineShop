using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAsync(string userEmail, int deliveryMethod, string basketId,
            ReturnAddressDto shippingAddress);

        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string userEmail);

        Task<OrderToReturnDto> GetOrderByIdAsync(int id, string userEmail);

        Task<bool> HasUserBoughtProduct(string userId, int productId);

    } 
}
