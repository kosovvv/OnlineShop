using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.Core.Interfaces;
using Skinet.WebAPI.Dtos;
using Skinet.WebAPI.Errors;
using Skinet.WebAPI.Extensions;
using System.Net;
using System.Security.Claims;

namespace Skinet.WebAPI.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var address = mapper.Map<AddressDto, OrderAddress>(orderDto.ShipToAddress);

            var order = await orderService.
                CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Order>>> GetOrdersForUser()
        {
            var email = User.RetrieveEmailFromPrincipal();
            var orders = await orderService.GetOrdersForUserAsync(email);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var order = await orderService.GetOrderByIdAsync(id, email);

            if (order == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return order;
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<ICollection<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await orderService.GetDeliveryMethodsAsync();

            return Ok(methods);
        }

    }
}
