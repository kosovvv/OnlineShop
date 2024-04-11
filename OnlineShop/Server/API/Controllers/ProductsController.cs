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
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductParams productParams)
        {
            var totalItems = await _productService.GetProductsCountAsync(productParams);
            var products = await _productService.GetProductsAsync(productParams);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
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
                var createdProduct = await _productService.CreateProduct(product);
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
                var updatedProduct = await _productService.EditProduct(id, product);
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
            var isDeleted = await _productService.DeleteProduct(id);
            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new ApiResponse(404, "Product not found."));
            }
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductBrandDto>>> GetProductBrands()
        {
            return Ok(await _productService.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            return Ok(await _productService.GetProductTypesAsync());
        }
    }
}
