using System;
using System.Net;
using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Application.Commands;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICommandHandler<PlaceOrder> _commandHandler;

        public OrderController(ILogger<OrderController> logger, ICommandHandler<PlaceOrder> commandHandler)
        {
            _logger = logger;
            _commandHandler = commandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrder command)
        {
            _logger.LogDebug($"PlaceOrderCommand: {command}.");

            try
            {
                await _commandHandler.HandleAsync(command);
            }
            catch (InfrastructureException ex)
            {
                _logger.LogError($"Server error during processing command: {ex.Message}");
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Client error during processing command: {ex.Message}");
                return BadRequest();
            }

            return Ok();
        }
    }
}