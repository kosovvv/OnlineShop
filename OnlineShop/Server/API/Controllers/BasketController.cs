using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;

namespace OnlineShop.WebAPI.Controllers
{
    public class BasketController : BaseController
    {

        private readonly IBasketService basketService;
        public BasketController(IBasketService basketRepository)
        {
            this.basketService = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await basketService.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var updatedBasket = await basketService.UpdateBasketAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await basketService.DeleteBasketAsync(id);
        }

    }
}
