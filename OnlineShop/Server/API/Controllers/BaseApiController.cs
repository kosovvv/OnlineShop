using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase { }
}
