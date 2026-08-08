using Store.Errors;

namespace E_Commerce.Errors
{
    public class ApiValidationErrorResponse : ErrorApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }


        #region Property
        public IEnumerable<string> Errors { get; set; }
        #endregion
    }
}
