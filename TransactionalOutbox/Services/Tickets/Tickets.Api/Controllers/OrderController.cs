using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tickets.Application.Commands;
using Tickets.Application.Commands.Handlers;

namespace Tickets.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICommandHandler<BuyTicketCommand> _commandHandler;

        public OrderController(ILogger<OrderController> logger, ICommandHandler<BuyTicketCommand> commandHandler)
        {
            _logger = logger;
            _commandHandler = commandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketCommand command)
        {
            _logger.LogInformation($"BuyTicketCommand: {command}.");

            try
            {
                await _commandHandler.HandleAsync(command);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}