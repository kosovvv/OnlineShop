using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models;
using OnlineShop.Web.ViewModels.Brand;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.Services.Mapping
{
    public class BrandUrlResolver : IValueResolver<ProductBrand, ReturnProductBrandDto, string>
    {
        private readonly IConfiguration config;
        public BrandUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(ProductBrand source, ReturnProductBrandDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}
