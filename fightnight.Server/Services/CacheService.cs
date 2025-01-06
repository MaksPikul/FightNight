using fightnight.Server.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace fightnight.Server.Services
{
    public class CacheService(IDistributedCache dCache) : ICacheService
    {
        private readonly IDistributedCache _dCache = dCache;
        public async Task AddToCacheAsync(string key, object obj)
        {
            string value = JsonSerializer.Serialize(obj);
            _dCache.SetStringAsync(key, value);
        }
        public async Task<string> GetFromCacheAsync(string key)
        {
            string result =  await _dCache.GetStringAsync(key);
            return result;

        }
        public async Task RemoveFromCacheAsync(string key)
        {
            _dCache.Remove(key);
        }

    }
}
