using E_Commerce.Errors;
using System.Net;
using System.Text.Json;

namespace Store.Middlewares
{
    public class ExceptionMiddleware
    {
        #region Field
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        #endregion

        public ExceptionMiddleware(RequestDelegate Next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            _next = Next;
           _logger = logger;
           _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _environment.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                             : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var jsonResponse=JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }

        }
    }
}
