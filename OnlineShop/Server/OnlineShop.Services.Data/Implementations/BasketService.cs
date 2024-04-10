using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using StackExchange.Redis;
using System.Text.Json;

namespace OnlineShop.Services.Data.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IDatabase database;
        public BasketService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var data = await database.StringGetAsync(id);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var created = await
                database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created)
            {
                return null;
            }

            return await this.GetBasketAsync(basket.Id);
        }
    }
}
