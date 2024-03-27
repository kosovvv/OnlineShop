using Skinet.Core.Entities;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.Core.Interfaces;
using Skinet.Core.Specification;

namespace Skinet.Infrastructure.Data
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepo;
        private readonly IUnitOfWork unitOfWork;
        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            this.basketRepo = basketRepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, OrderAddress shippingAddress)
        {
            // get basket from repo
            var basket = await this.basketRepo.GetBaskedAsync(basketId);
            // get items from product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items) 
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = 
                    new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(itemOrdered,productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // get deliverymethods

            var deliveryMethod = await this.unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            // calc subtotal

            var subTotal = items.Sum(x => x.Price * x.Quantity);
            // create order

            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.SubTotal = subTotal;

                unitOfWork.Repository<Order>().Update(order);
            }
            else
            {
                order = new Order(items, buyerEmail, shippingAddress,
                deliveryMethod, subTotal, basket.PaymentIntentId);

                this.unitOfWork.Repository<Order>().Add(order);
            }
           
            var result = await this.unitOfWork.Complete();

            if (result <= 0)
            {
                return null;
            }

            // return order
            return order;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().LislAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
