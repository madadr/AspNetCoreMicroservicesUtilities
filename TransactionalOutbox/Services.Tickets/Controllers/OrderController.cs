using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Tickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger) => _logger = logger;

        [HttpPost]
        public IActionResult Post([FromBody] string seat)
        {
            _logger.LogInformation($"Ticket order for seat {seat} requested.");
            return !string.IsNullOrEmpty(seat) ? (IActionResult) Accepted() : BadRequest();
        }
    }
}
