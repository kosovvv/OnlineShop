using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;


namespace OnlineShop.WebAPI.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;


        public ProductsController(IProductService productService,
            IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Cached(600)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductParams productParams)
        {
            var totalItems = await productService.GetProductsCountAsync(productParams);

            var products = await productService.GetProductsAsync(productParams);

             
            var data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

           return Ok(new Pagination<ProductToReturnDto>
               (productParams.PageIndex, productParams.PageSize,totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [Cached(600)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            if (product == null) return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productService.GetProductBrandsAsync());
        }


        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductTypes()
        {
            return Ok(await productService.GetProductTypesAsync());
        }
    }
}
