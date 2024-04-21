using AutoMapper;
using OnlineShop.Data.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Models;
using OnlineShop.Services.Mapping.Resolvers;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Account;
using OnlineShop.Web.ViewModels.Address;
using OnlineShop.Web.ViewModels.Brand;
using OnlineShop.Web.ViewModels.Order;
using OnlineShop.Web.ViewModels.Product;
using OnlineShop.Web.ViewModels.Review;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ReviewsCount, o => o.MapFrom(s => s.Reviews.Count()))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>())
                .ForMember(d => d.ProductBrandPictureUrl, o => o.MapFrom<ProductBrandUrlResolver>());


            CreateMap<ProductToCreateDto, Product>()
                .ForMember(dest => dest.PictureUrl, opt =>
                    opt.MapFrom(src => !string.IsNullOrEmpty(src.PictureUrl) ? src.PictureUrl : "api/images/products/image-placeholder.png"))
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore());

            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.ReviewedProduct, opt => opt.Ignore());

            CreateMap<Review, ReturnReviewDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ReviewedProduct.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Email));

            CreateMap<DeliveryMethod, ReturnDeliveryMethodDto>().ReverseMap();

            CreateMap<Address, ReturnAddressDto>().ReverseMap();
            CreateMap<Address, OrderAddress>().ReverseMap();
            CreateMap<ProductType,  ReturnProductTypeDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items)).ReverseMap();

            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<CreateAddressDto, OrderAddress>();
            CreateMap<ReturnAddressDto, OrderAddress>();

            CreateMap<ProductType, ReturnProductTypeDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Products))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<TypeUrlResolver>());
            CreateMap<CreateProductTypeDto, ProductType>();

            CreateMap<ProductBrand, ReturnProductBrandDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Products))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<BrandUrlResolver>());

            CreateMap<CreateProductBrandDto, ProductBrand>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, opt => opt.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, opt => opt.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(s => s.ItemOrdered.PictureUrl));

            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();

        }
        public MappingProfiles(bool useResolvers)
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ReviewsCount, o => o.MapFrom(s => s.Reviews.Count()));

            CreateMap<ProductToCreateDto, Product>()
                .ForMember(dest => dest.PictureUrl, opt =>
                    opt.MapFrom(src => !string.IsNullOrEmpty(src.PictureUrl) ? src.PictureUrl : "api/images/products/image-placeholder.png"))
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore());

            CreateMap<CreateReviewDto, Review>()
                .ForMember(dest => dest.ReviewedProduct, opt => opt.Ignore());

            CreateMap<Review, ReturnReviewDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ReviewedProduct.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Email));

            CreateMap<DeliveryMethod, ReturnDeliveryMethodDto>().ReverseMap();

            CreateMap<Address, ReturnAddressDto>().ReverseMap();
            CreateMap<Address, OrderAddress>().ReverseMap();
            CreateMap<ProductType, ReturnProductTypeDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items)).ReverseMap();

            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<CreateAddressDto, OrderAddress>();
            CreateMap<ReturnAddressDto, OrderAddress>();

            CreateMap<ProductType, ReturnProductTypeDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Products));

            CreateMap<CreateProductTypeDto, ProductType>();

            CreateMap<ProductBrand, ReturnProductBrandDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Products));

            CreateMap<CreateProductBrandDto, ProductBrand>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, opt => opt.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, opt => opt.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(s => s.ItemOrdered.PictureUrl));

            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();

        }
    }
}
