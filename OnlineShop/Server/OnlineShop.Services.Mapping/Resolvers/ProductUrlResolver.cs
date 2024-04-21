using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.Services.Mapping.Resolvers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration config;
        public ProductUrlResolver()
        {
            
        }
        public ProductUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (this.config != null)
            {
                if (!string.IsNullOrEmpty(source.PictureUrl))
                {
                    return config["ApiUrl"] + source.PictureUrl;
                }
            }
            return null;
        }
    }
}