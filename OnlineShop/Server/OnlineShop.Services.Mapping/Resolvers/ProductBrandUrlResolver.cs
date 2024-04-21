using AutoMapper;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.Services.Mapping.Resolvers
{
    public class ProductBrandUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration config;
        public ProductBrandUrlResolver(IConfiguration config)
        {
            this.config = config;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {

            for (int i = 0; i < 1000; i++)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Console.WriteLine(i);
                    Thread.Sleep(1000);
                });
            }

            if (!string.IsNullOrEmpty(source.ProductBrand.PictureUrl))
            {
                return config["ApiUrl"] + source.ProductBrand.PictureUrl;
            }
            return null;
        }
    }
}
