namespace Store.Errors
{
    public class ErrorApiResponse
    {
        #region Ctor
        public ErrorApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefulatMessageForStatusCode(statusCode);
        }

        #endregion

        #region Method
        private string? GetDefulatMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                404 => "Item Not Found",
                401 => "You Are Not Authorized",
                429 => "Too Many Requests",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                _   => null
            };
        } 
        #endregion

        #region Property
        public int StatusCode { get; set; }
        public string? Message { get; set; } 
        #endregion
    }
}
