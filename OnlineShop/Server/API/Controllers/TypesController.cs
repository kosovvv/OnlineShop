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

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            return Ok(await this.typeService.GetProductTypesAsync());
        }
    }
}
