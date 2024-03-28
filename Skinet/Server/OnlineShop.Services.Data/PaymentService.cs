using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Data;
using Skinet.Core.Entities;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.Core.Interfaces;
using Stripe;

namespace Skinet.Infrastructure.Data
{
    public class PaymentService : IPaymentService
    {
        private readonly StoreContext context;
        private readonly IBasketRepository basketRepository;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepository basketRepository, IConfiguration config, StoreContext context)
        {
            this.basketRepository = basketRepository;
            this.configuration = config;
            this.context = context;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await basketRepository.GetBaskedAsync(basketId);

            if (basket == null)
            {
                return null;
            }

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)   
            {
                var deliveryMethod = await context.DeliveryMethods
                    .FirstOrDefaultAsync(x => x.Id == basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await context.Products.FirstOrDefaultAsync(x => x.Id == item.Id);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            // if empty => create
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            // else update
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId, options); 
            }

            await basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var order = await context.Orders.FirstOrDefaultAsync
                (x => x.PaymentIntentId == paymentIntentId);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PaymentFailed;
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId)
        {
            var order = await context.Orders.FirstOrDefaultAsync
                (x => x.PaymentIntentId == paymentIntentId);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PaymentRecieved;
            await context.SaveChangesAsync();
            return order;
        }
    }
}
