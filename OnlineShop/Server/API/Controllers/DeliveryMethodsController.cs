using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data.Models.OrderAggregate;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using Stripe;

namespace OnlineShop.WebAPI.Controllers
{
    public class DeliveryMethodsController : BaseApiController
    {
        private readonly IDeliveryMethodService dmService;
        public DeliveryMethodsController(IDeliveryMethodService dmService)
        {
            this.dmService = dmService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Cached(600)]
        public async Task<ActionResult<ICollection<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await dmService.GetDeliveryMethodsAsync();
            return methods.Any() ? Ok(methods) : NotFound();
        }
    }
}
