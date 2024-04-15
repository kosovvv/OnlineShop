using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IMapper mapper;
        private readonly StoreContext context;
        public BrandService(IMapper mapper, StoreContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ProductBrandDto>> GetProductBrandsAsync()
        {
            var productTypes = await context.ProductBrands.AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandDto>>(productTypes);
        }
    }
}
