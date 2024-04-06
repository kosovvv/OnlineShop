using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;


namespace OnlineShop.WebAPI.Controllers
{
    public class BasketController : BaseController
    {

        private readonly IBasketService basketService;
        private readonly IMapper mapper;
        public BasketController(IBasketService basketRepository, IMapper mapper)
        {
            this.basketService = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await basketService.GetBaskedAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = this.mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var updatedBasket = await basketService.UpdateBasketAsync(customerBasket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await basketService.DeleteBasketAsync(id);
        }

    }
}
