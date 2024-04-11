using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.WebAPI.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();
            var order = await orderService.
                CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, orderDto.ShipToAddress);

            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }
            return Ok(order);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = User.RetrieveEmailFromPrincipal();
            var orders = await orderService.GetOrdersForUserAsync(email);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var order = await orderService.GetOrderByIdAsync(id, email);

            if (order == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<ICollection<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await orderService.GetDeliveryMethodsAsync();

            return Ok(methods);
        }
    }
}
