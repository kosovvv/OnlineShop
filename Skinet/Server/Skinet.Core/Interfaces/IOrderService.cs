using Skinet.Core.Entities.OrderAggregate;

namespace Skinet.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId,
            OrderAddress shippingAddress);

        Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

        Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync();


    } 
}
