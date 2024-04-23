using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Data.Common;
using OnlineShop.Data.Models.Enumerations;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using Stripe;

namespace OnlineShop.Services.Data.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketService basketService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PaymentService(IBasketService basketRepository, IConfiguration config,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.basketService = basketRepository;
            this.configuration = config;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await basketService.GetBasketAsync(basketId);

            if (basket == null)
            {
                return null;
            }

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod>().GetById((int)basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await unitOfWork.GetRepository<Models.Product>().GetById(item.Id);

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

            await basketService.UpdateBasketAsync(mapper.Map<CustomerBasket, CustomerBasketDto>(basket));
            return mapper.Map<CustomerBasket, CustomerBasketDto>(basket);
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order>().All().
                FirstOrDefaultAsync(x => x.PaymentIntentId == paymentIntentId);

            if (order == null)
            {
                throw new UpdateOrderFailedException();
            }

            order.Status = OrderStatus.PaymentFailed;
            await unitOfWork.Save();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order>().All().
                FirstOrDefaultAsync(x => x.PaymentIntentId == paymentIntentId);

            if (order == null)
            {
                throw new UpdateOrderFailedException();
            }

            order.Status = OrderStatus.PaymentRecieved;
            await unitOfWork.Save();
            return order;
        }
    }
}
