using System;
using System.Net;
using System.Threading.Tasks;
using Common.Application.Commands;
using Common.Application.Commands.Handlers;
using Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            catch (InfrastructureException ex)
            {
                _logger.LogError($"Server error during processing command: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Client error during processing command: {ex.Message}");
                return BadRequest();
            }

            return Ok();
        }
    }
}