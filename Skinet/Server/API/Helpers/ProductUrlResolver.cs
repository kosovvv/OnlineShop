using AutoMapper;
using Skinet.Core.Entities;
using Skinet.WebAPI.Dtos;

namespace Skinet.WebAPI.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration config;
        public ProductUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}