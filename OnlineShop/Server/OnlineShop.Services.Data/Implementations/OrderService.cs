using OnlineShop.Data;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Order;
using OnlineShop.Web.ViewModels.Address;
using AutoMapper;
using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Services.Data.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService basketRepo;
        private readonly StoreContext context;
        private readonly IMapper mapper;
        public OrderService(IBasketService basketRepo, StoreContext context, IMapper mapper)
        {
            this.basketRepo = basketRepo;
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ReturnAddressDto shippingAddress)
        {
            // get basket from repo
            var basket = await basketRepo.GetBasketAsync(basketId);
            // get items from product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await context.Products.FirstOrDefaultAsync(x => x.Id == item.Id);
                var itemOrdered =
                    new ProductItemOrdered((int)productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
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

            var addressToSet = mapper.Map<Address, OrderAddress>
                (await context.Address.FirstOrDefaultAsync(x => x.Id == shippingAddress.Id));

            if (order != null)
            {
                order.ShipToAddress = addressToSet;
                order.DeliveryMethod = deliveryMethod;
                order.SubTotal = subTotal;

                await context.SaveChangesAsync();
            }
            else
            {
                order = new Order(items, buyerEmail, addressToSet,
                deliveryMethod, subTotal, basket.PaymentIntentId);

                await context.Orders.AddAsync(order);
            }

            var result = await context.SaveChangesAsync();

            if (result <= 0)
            {
                return null;
            }

            return mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<ReturnDeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var methods = await context.DeliveryMethods.AsNoTracking().ToListAsync();
            return mapper.Map<ICollection<DeliveryMethod>, ICollection<ReturnDeliveryMethodDto>>(methods);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await context.Orders
                .AsNoTracking()
               .Where(x => x.BuyerEmail == buyerEmail && x.Id == id)
               .Include(x => x.DeliveryMethod)
               .Include(x => x.OrderItems)
               .OrderByDescending(x => x.OrderDate)
               .FirstOrDefaultAsync();

            return mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await context.Orders
                .Where(x => x.BuyerEmail == buyerEmail)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return mapper.Map<ICollection<Order>, ICollection<OrderToReturnDto>>(orders);
        }

        public async Task<bool> HasUserBoughtProduct(string buyerEmail, int productId)
        {
            var orders = await context.Orders
                .Where(x => x.BuyerEmail == buyerEmail && x.OrderItems.Any(p => p.ItemOrdered.ProductItemId == productId))
                .ToListAsync();

            return orders.Count > 0;
        }
    }
}
