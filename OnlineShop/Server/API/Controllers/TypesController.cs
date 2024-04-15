using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            var types = await this.typeService.GetProductTypesAsync();
            
            return types.Any() ? Ok(types) : NoContent();
        }
    }
}
