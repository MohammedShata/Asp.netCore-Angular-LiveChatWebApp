using api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController: ControllerBase
    {
        
    }
}