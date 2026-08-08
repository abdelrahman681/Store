using Store.Errors;

namespace E_Commerce.Errors
{
    public class ApiExceptionResponse : ErrorApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? message = null,string? datiles=null) : base(statusCode, message)
        {
            Details= datiles;
        }
    }
}
