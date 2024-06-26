﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.WebAPI.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            try
            {
                var order = await orderService.
                CreateOrderAsync(User.GetId(), orderDto.DeliveryMethodId, orderDto.BasketId, orderDto.ShipToAddress);
                return Ok(order);
            }
            catch (CreateOrderFailedException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [Cached(600)]
        public async Task<ActionResult<ICollection<OrderToReturnDto>>> GetOrdersForUser()
        {
            var orders = await orderService.GetOrdersForUserAsync(User.GetId());

            return orders.Any() ? Ok(orders) : NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [Cached(600)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id, User.GetId());

            if (order == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(order);
        }
    }
}