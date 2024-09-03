using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.CacheDatabase.Service
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public BalanceEntity? GetCurrentBalance()
        {
            var balance = _cache.GetString(DateOnly.FromDateTime(DateTime.Now.Date).ToString());

            if (balance is null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BalanceEntity>(balance);
        }

        public void SaveCurrentBalanceRedis(BalanceEntity balance)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            Console.WriteLine($"Salvo no redis: {JsonConvert.SerializeObject(balance)}");

            _cache.SetString(DateOnly.FromDateTime(DateTime.Now).ToString(), JsonConvert.SerializeObject(balance), options);
        }
    }
}
