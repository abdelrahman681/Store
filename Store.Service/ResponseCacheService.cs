using StackExchange.Redis;
using Store.CoreLayer.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Store.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string CacheKey, object ResponseValue, TimeSpan ExpireTime)
        {
            if (ResponseValue is null) return;
            var option = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonResponse=JsonSerializer.Serialize(ResponseValue, option);
            await _database.StringSetAsync(CacheKey, jsonResponse, ExpireTime);
        }

        public async Task<string?> GetCacheResponseAsync(string CacheKey)
        {
            var ResponseValue=await _database.StringGetAsync(CacheKey);
            if (ResponseValue.IsNullOrEmpty)
                return null;
            return ResponseValue;
        }
    }
}
