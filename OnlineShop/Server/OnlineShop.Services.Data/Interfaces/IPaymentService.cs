using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
