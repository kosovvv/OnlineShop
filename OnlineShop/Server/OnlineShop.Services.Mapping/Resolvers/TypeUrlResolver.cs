using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Mapping.Resolvers
{
    public class TypeUrlResolver : IValueResolver<ProductType, ReturnProductTypeDto, string>
    {
        private readonly IConfiguration config;
        public TypeUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(ProductType source, ReturnProductTypeDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}
