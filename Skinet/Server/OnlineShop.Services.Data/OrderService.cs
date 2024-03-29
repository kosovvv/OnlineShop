using OnlineShop.Data;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;

namespace OnlineShop.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService basketRepo;
        private readonly StoreContext context;
        public OrderService(IBasketService basketRepo, StoreContext context)
        {
            this.basketRepo = basketRepo;
            this.context = context;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, OrderAddress shippingAddress)
        {
            // get basket from repo
            var basket = await this.basketRepo.GetBaskedAsync(basketId);
            // get items from product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items) 
            {
                var productItem = await context.Products.FirstOrDefaultAsync(x => x.Id == item.Id);
                var itemOrdered = 
                    new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(itemOrdered,productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // get deliverymethods

            var deliveryMethod = await context.DeliveryMethods
                .FirstOrDefaultAsync(x => x.Id == deliveryMethodId);
           
            // calc subtotal

            var subTotal = items.Sum(x => x.Price * x.Quantity);
            // create order


            var order = await context.Orders.FirstOrDefaultAsync
                (x => x.PaymentIntentId == basket.PaymentIntentId);

            if (order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.SubTotal = subTotal;

                await context.SaveChangesAsync();
            }
            else
            {
                order = new Order(items, buyerEmail, shippingAddress,
                deliveryMethod, subTotal, basket.PaymentIntentId);

                await context.Orders.AddAsync(order);
            }
           
            var result = await context.SaveChangesAsync();

            if (result <= 0)
            {
                return null;
            }

            return order;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await context.DeliveryMethods.AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await context.Orders
                .AsNoTracking()
               .Where(x => x.BuyerEmail == buyerEmail && x.Id == id)
               .Include(x => x.DeliveryMethod)
               .Include(x => x.OrderItems)
               .OrderByDescending(x => x.OrderDate)
               .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await context.Orders
                .Where(x => x.BuyerEmail == buyerEmail)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return orders;
        }
    }
}
