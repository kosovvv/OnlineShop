using Microsoft.Extensions.Configuration;
using Skinet.Core.Entities;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.Core.Interfaces;
using Skinet.Core.Specification;
using Stripe;

namespace Skinet.Infrastructure.Data
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.configuration = config;
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
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)basket.DeliveryMethodId); 

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);

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
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PaymentFailed;
            await unitOfWork.Complete();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PaymentRecieved;
            await unitOfWork.Complete();
            return order;
        }
    }
}
