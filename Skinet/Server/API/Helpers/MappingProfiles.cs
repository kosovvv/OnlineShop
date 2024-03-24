using API.Dtos;
using AutoMapper;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Entities.OrderAggregate;
using Skinet.WebAPI.Dtos;

namespace Skinet.WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, OrderAddress>();

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
