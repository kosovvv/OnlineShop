using AutoMapper;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Infrastructure.Data;
using Skinet.WebAPI.Controllers;


namespace Skinet.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IGenericRepository<ProductBrand> brandRepository;
        private readonly IGenericRepository<ProductType> typeRepository;
        private readonly IMapper mapper;


        public ProductsController(IProductService productService,
            IGenericRepository<ProductBrand> brandRepository,
            IGenericRepository<ProductType> typeRepository,
            IMapper mapper)
        {
            this.productService = productService;
            this.brandRepository = brandRepository;
            this.typeRepository = typeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Cached(600)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductSpecParams productParams)
        {
            ProductsWithTypesAndBrandsSpecification spec = new(productParams);


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
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productService.GetProductByIdAsync(id);

            if (product == null) return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductBrands()
        {
            return Ok(await brandRepository.LislAllAsync());
        }


        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductTypes()
        {
            return Ok(await typeRepository.LislAllAsync());
        }
    }
}
