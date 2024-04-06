using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using OnlineShop.Web.ViewModels.Order;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId,
            ReturnAddressDto shippingAddress);

        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);

        Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail);

        Task<IEnumerable<ReturnDeliveryMethodDto>> GetDeliveryMethodsAsync();


    } 
}
