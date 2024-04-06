
using OnlineShop.Models;
using OnlineShop.Services.Data.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace OnlineShop.Services.Data
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

        public async Task<CustomerBasket> GetBaskedAsync(string id)
        {
            var data = await database.StringGetAsync(id);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await
                database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created)
            {
                return null;
            }

            return await GetBaskedAsync(basket.Id);
        }
    }
}
