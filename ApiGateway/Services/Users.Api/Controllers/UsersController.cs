using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly Guid _id;

        public UsersController(ILogger<UsersController> logger, InstanceIdHolder idHolder)
        {
            _logger = logger;
            _id = idHolder.Id;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"GET called on {_id}");
            return Ok(_id);
        }
    }
}