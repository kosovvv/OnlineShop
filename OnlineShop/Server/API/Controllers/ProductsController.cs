using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;


namespace OnlineShop.WebAPI.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Cached(40)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductParams productParams)
        {
            var totalItems = await productService.GetProductsCountAsync(productParams);

            var products = await productService.GetProductsAsync(productParams);

            return Ok(new Pagination<ProductToReturnDto>
                (productParams.PageIndex, productParams.PageSize, totalItems, products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Cached(300)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            if (product == null) return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok(product);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductToCreateDto product)
        {        
            var result = await productService.CreateProduct(product);

            return Ok(result);
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> EditProduct(int id, ProductToCreateDto product)
        {
            var result = await this.productService.EditProduct(id, product);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isDeleted = await productService.DeleteProduct(id);
            return isDeleted ? Ok() : NotFound();
        }

        [Cached(10)]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrandDto>>> GetProductBrands()
        {
            return Ok(await productService.GetProductBrandsAsync());
        }


        [Cached(10)]
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            return Ok(await productService.GetProductTypesAsync());
        }
    }
}
