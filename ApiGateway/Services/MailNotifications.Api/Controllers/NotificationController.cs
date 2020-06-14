using System;
using System.Net;
using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailNotifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            _logger.LogInformation($"Remove");
            return Ok("NotificationController: Remove ok");
        }
    }
}
