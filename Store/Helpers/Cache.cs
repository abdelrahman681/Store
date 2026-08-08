using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.CoreLayer.IService;
using System.Text;

namespace Store.Helpers
{
    public class Cache : Attribute, IAsyncActionFilter
    {
        private readonly int expireTimeInSeconds;

        public Cache(int ExpireTimeInMinute)
        {
            expireTimeInSeconds = ExpireTimeInMinute;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey= GenerateCacheKeyFromRequest(context.HttpContext.Request).ToString();
            var cacheResponse=await cacheService.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var content = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                context.Result = content;
                return;
            }
            var executedEndPointContext = await next.Invoke();
            if(executedEndPointContext.Result is OkObjectResult result)
                await cacheService.CacheResponseAsync(cacheKey, result.Value, TimeSpan.FromSeconds(expireTimeInSeconds));
        }

        private object GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder=new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (Key,Value) in request.Query.OrderBy(x=>x.Key))
                KeyBuilder.Append($"|{Key}-{Value}");
            return KeyBuilder;
        }
    }
}
