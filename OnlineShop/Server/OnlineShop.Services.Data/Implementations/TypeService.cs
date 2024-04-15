using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Services.Data.Implementations
{
    public class TypeService : ITypeService
    {
        private readonly IMapper mapper;
        private readonly StoreContext context;
        public TypeService(StoreContext context, IMapper mapper)
        {
            this.context= context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ProductTypeDto>> GetProductTypesAsync()
        {
            var productTypes = await context.ProductTypes.AsNoTracking().ToListAsync();
            return mapper.Map<IEnumerable<ProductType>, IEnumerable<ProductTypeDto>>(productTypes);
        }
    }
}
