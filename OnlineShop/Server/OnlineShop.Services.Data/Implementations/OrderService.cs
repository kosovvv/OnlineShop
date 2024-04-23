using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using AutoMapper;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace OnlineShop.Services.Data.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService basketService;
        private readonly IProductService productService;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Order> orderRepository;
        private readonly IMapper mapper;
        public OrderService(IBasketService basketService,IProductService productService,
            IUnitOfWork unitOfWork, IMapper mapper, IDeliveryMethodService deliveryMethodService)
        {
            this.basketService = basketService;
            this.productService = productService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.deliveryMethodService = deliveryMethodService;
            this.orderRepository = unitOfWork.GetRepository<Order>();
        }
        public async Task<OrderToReturnDto> CreateOrderAsync(string userId, int deliveryMethodId, string basketId, ReturnAddressDto shippingAddress)
        {
            // get basket from repo
            var basket = await basketService.GetBasketAsync(basketId);
            // get items from product repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await productService.GetProductByIdAsync(item.Id);
                var itemOrdered =
                    new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // get deliverymethods

            var deliveryMethod = await this.deliveryMethodService.GetDeliveryMethodByIdAsync(deliveryMethodId);

            // calc subtotal

            var subTotal = items.Sum(x => x.Price * x.Quantity);
            // create order


            var order = await orderRepository.All().FirstOrDefaultAsync(x => x.PaymentIntentId == basket.PaymentIntentId);

            var addressToSet = mapper.Map<Address, OrderAddress>
                (await unitOfWork.GetRepository<Address>().GetById(shippingAddress.Id));

            var dm = await unitOfWork.GetRepository<DeliveryMethod>().GetById(deliveryMethodId);

            if (order != null)
            {
                order.ShipToAddress = addressToSet;
                order.DeliveryMethodId = deliveryMethodId;
                order.SubTotal = subTotal;

                await unitOfWork.Save();
            }
            else
            {
                order = new Order(items, userId, addressToSet,dm,
                    deliveryMethodId, subTotal, basket.PaymentIntentId);

                await orderRepository.AddAsync(order);
            }

            await unitOfWork.Save();
            return mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(int id, string userId = null)
        {
            var order = await orderRepository
                .AllAsNoTracking()
               .Include(x => x.DeliveryMethod)
               .Include(x => x.OrderItems)
               .OrderByDescending(x => x.OrderDate)
               .FirstOrDefaultAsync(x => x.Id == id && (userId.IsNullOrEmpty() || x.BuyerId == userId));

            return mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string userId)
        {
            var orders = await orderRepository.All()
                .Where(x => x.Buyer.Id == userId)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return mapper.Map<ICollection<Order>, ICollection<OrderToReturnDto>>(orders);
        }
        public async Task<bool> HasUserBoughtProduct(string userId, int productId)
        {
            return await orderRepository.All()
                .AnyAsync(x => x.Buyer.Id == userId && x.OrderItems.Any(p => p.ItemOrdered.ProductItemId == productId));
        }
    }
}
