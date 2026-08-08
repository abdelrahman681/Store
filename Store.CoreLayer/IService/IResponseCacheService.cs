using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface IResponseCacheService
    {
        //Cache Data
        Task CacheResponseAsync(string CacheKey, object ResponseValue, TimeSpan ExpireTime);
        //get cached Date
        Task<string?> GetCacheResponseAsync(string CacheKey);
    }
}
