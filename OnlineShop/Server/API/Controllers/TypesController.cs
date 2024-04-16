using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Type;

namespace OnlineShop.WebAPI.Controllers
{
    public class TypesController : BaseController
    {
        private readonly ITypeService typeService;
        public TypesController(ITypeService typeService) 
        {
            this.typeService = typeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ReturnProductTypeDto>>> GetProductTypes()
        {
            var types = await this.typeService.GetProductTypesAsync();
            
            return types.Any() ? Ok(types) : NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReturnProductTypeDto>> CreateProductType(CreateProductTypeDto productType)
        {
            try
            {
                var createdType = await this.typeService.CreateProductTypeAsync(productType);
                return CreatedAtAction(nameof(CreateProductType), new { id = createdType.Id }, createdType);
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
        public async Task<ActionResult<ReturnProductTypeDto>> UpdateProductType(int id, CreateProductTypeDto productType)
        {
            try
            {
                var updatedProductType = await this.typeService.UpdateProductTypeAsync(id, productType);
                return Ok(updatedProductType);
            }
            catch (EntityNotExistingException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
           
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnProductTypeDto>> DeleteProductType(int id)
        {
            var isDeleted = await this.typeService.DeleteProductTypeAsync(id);
            if (isDeleted)
            {
                return Ok(isDeleted);
            }
            else
            {
                return NotFound(new ApiResponse(404, "ProductType not found."));
            }
        }
    }
}
