using Microsoft.AspNetCore.Mvc;

namespace Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}