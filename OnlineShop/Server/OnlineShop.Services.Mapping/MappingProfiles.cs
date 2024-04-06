﻿using AutoMapper;
using OnlineShop.Data.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Models;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using OnlineShop.Web.ViewModels.Order;
using OnlineShop.Web.ViewModels.Product;
using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.Services.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<ProductToCreateDto, Product>()
                .ForMember(dest => dest.PictureUrl, opt =>
                    opt.MapFrom(src => !string.IsNullOrEmpty(src.PictureUrl) ? src.PictureUrl : "api/images/products/image-placeholder.png"))
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore());

            CreateMap<Review, ReturnReviewDto>();
            CreateMap<DeliveryMethod, ReturnDeliveryMethodDto>().ReverseMap();

            CreateMap<Address, ReturnAddressDto>().ReverseMap();
            CreateMap<Address, OrderAddress>().ReverseMap();
            CreateMap<ProductBrand, ProductBrandDto>().ReverseMap();
            CreateMap<ProductType,  ProductTypeDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<CreateAddressDto, OrderAddress>();
            CreateMap<ReturnAddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, opt => opt.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, opt => opt.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom<OrderItemUrlResolver>());

        }
    }
}
