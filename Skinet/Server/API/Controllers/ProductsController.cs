using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specification;
using Skinet.WebAPI.Dtos;


namespace Skinet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            ProductsWithTypesAndBrandsSpecification spec = new();
            var products = await productRepository.ListAsync(spec);

            return products.Select(x => new ProductToReturnDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PictureUrl = x.PictureUrl,
                Price = x.Price,
                ProductBrand = x.ProductBrand.Name,
                ProductType = x.ProductType.Name,
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productRepository.GetEntityWithSpec(spec);

            return mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductBrands()
        {

            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetProductTypes()
        {
            return Ok(await typeRepository.LislAllAsync());
        }
    }
}
