using AutoMapper;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.WebAPI.Dtos;

namespace Skinet.WebAPI.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public readonly IConfiguration config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return config["ApiUrl"] + source.ItemOrdered.PictureUrl;
            }

            return null;
        }
    }
}
