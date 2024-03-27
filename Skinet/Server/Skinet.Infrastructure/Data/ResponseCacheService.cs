using Skinet.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Skinet.Infrastructure.Data
{
    public class ResponseCacheService : IResponseCacheService
    {

        private readonly IDatabase database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);

            await database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await database.StringGetAsync(cacheKey);

            if (!cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        } 
    }
}
