using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace Store.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Errors(int code)
        {
            return StatusCode(code, new ErrorApiResponse(code));
        }
    }
}
