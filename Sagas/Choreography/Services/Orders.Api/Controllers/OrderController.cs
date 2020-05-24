using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.Application.Commands;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICommandHandler<PlaceOrder> _commandHandler;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ILogger<OrderController> logger, ICommandHandler<PlaceOrder> commandHandler,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderData data) // TODO: PlaceOrderDto
        {
            _logger.LogInformation($"PlaceOrderData: {JsonConvert.SerializeObject(data)}.");

            var command = new PlaceOrder(Guid.NewGuid(), data.ClientId, data.ProductId, data.Price, DateTime.Now);
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
                _logger.LogInformation($"Client error during processing command: {ex.Message}");
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(new {orderId = command.Id}));
        }

        [HttpGet("get/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            _logger.LogInformation($"GetOrder: {orderId}");
            if (Guid.TryParse(orderId, out var orderGuid))
            {
                return Ok(JsonConvert.SerializeObject(await _orderRepository.GetAsync(orderGuid)));
            }
            else
            {
                _logger.LogError("Invalid orderId.");
                return BadRequest();
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderRepository.GetAllAsync());
        }

        public class PlaceOrderData
        {
            public int ClientId { get; set; }
            public int ProductId { get; set; }
            public double Price { get; set; }

            public PlaceOrderData()
            {
            }
        }
    }
}
