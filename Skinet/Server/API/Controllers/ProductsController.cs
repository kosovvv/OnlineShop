using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specification;
using Skinet.WebAPI.Controllers;
using Skinet.WebAPI.Dtos;
using Skinet.WebAPI.Errors;
using Skinet.WebAPI.Helpers;


namespace Skinet.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IGenericRepository<Product> productRepository;
        private readonly IGenericRepository<ProductBrand> brandRepository;
        private readonly IGenericRepository<ProductType> typeRepository;
        private readonly IMapper mapper;


        public ProductsController(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> brandRepository,
            IGenericRepository<ProductType> typeRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.brandRepository = brandRepository;
            this.typeRepository = typeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductSpecParams productParams)
        {
            ProductsWithTypesAndBrandsSpecification spec = new(productParams);

            ProductWithFiltersForCountSpecification countSpec = new(productParams);

            var totalItems = await productRepository.CountAsync(countSpec); 

            var products = await productRepository.ListAsync(spec);
             
            var data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

           return Ok(new Pagination<ProductToReturnDto>
               (productParams.PageIndex, productParams.PageSize,totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productRepository.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductBrands()
        {

            return Ok(await brandRepository.LislAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductTypes()
        {
            return Ok(await typeRepository.LislAllAsync());
        }
    }
}
