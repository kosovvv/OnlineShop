using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IBrandService brandService;
        private readonly ITypeService typeService;

        public ProductsController(IProductService productService, IBrandService brandService, ITypeService typeService)
        {
            this.productService = productService;
            this.brandService = brandService;
            this.typeService = typeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductParams productParams)
        {
            var totalItems = await this.productService.GetProductsCountAsync(productParams);
            var products = await this.productService.GetProductsAsync(productParams);


            return products.Any() ? 
                Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, products))
                : NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            try
            {
                var product = await this.productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (ProductNotExistingException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductToReturnDto>> AddProduct(ProductToCreateDto product)
        {
            try
            {
                var createdProduct = await this.productService.CreateProduct(product);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (CreateExistringProductException ex)
            {   
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (InvalidProductException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(int id, [FromBody] ProductToCreateDto product)
        {
            try
            {
                var updatedProduct = await this.productService.EditProduct(id, product);
                return Ok(updatedProduct);
            }
            catch (ProductNotExistingException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (InvalidProductException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveProduct(int id)
        {
            var isDeleted = await this.productService.DeleteProduct(id);
            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new ApiResponse(404, "Product not found."));
            }
        }
    }
}
