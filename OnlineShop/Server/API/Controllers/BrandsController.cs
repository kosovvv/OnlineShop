using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ProductBrandDto>>> GetProductBrands()
        {
            var brands = await this.brandService.GetProductBrandsAsync();

            return brands.Any() ? Ok(brands) : NoContent();
        }
    }
}
