﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.Services.Mapping.Resolvers
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
