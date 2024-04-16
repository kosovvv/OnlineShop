using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Brand;

namespace OnlineShop.WebAPI.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly IBrandService brandService;

        public BrandsController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ReturnProductBrandDto>>> GetProductBrands()
        {
            var brands = await this.brandService.GetProductBrandsAsync();

            return brands.Any() ? Ok(brands) : NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<ReturnProductBrandDto>> CreateProductBrand(CreateProductBrandDto productBrand)
        {
            try
            {
                var createdBrand = await this.brandService.CreateProductBrandAsync(productBrand);
                return CreatedAtAction(nameof(CreateProductBrand), new { id = createdBrand.Id }, createdBrand);
            }
            catch (CreateExistringEntityException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
                throw;
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<ReturnProductBrandDto>> UpdateProductBrand(int id, CreateProductBrandDto productBrand)
        {
            try
            {
                var updatedProductBrand = await this.brandService.UpdateProductBrandAsync(id, productBrand);
                return Ok(updatedProductBrand);
            }
            catch (EntityNotExistingException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<ReturnProductBrandDto>> DeleteProductBrand(int id)
        {
            var isDeleted = await this.brandService.DeleteProductBrandAsync(id);
            if (isDeleted)
            {
                return Ok(isDeleted);
            }
            else
            {
                return NotFound(new ApiResponse(404, "ProductBrand not found."));
            }
        }
    }
}
