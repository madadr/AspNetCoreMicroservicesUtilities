using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.Application.Events;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Application.Commands.Handlers
{
    public class PlaceOrderCommandHandler : CommandHandlerBase<PlaceOrder>
    {
        private readonly IMessageBroker _broker;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderCommandHandler(IMessageBroker broker, IOrderRepository orderRepository,
            ILogger<PlaceOrderCommandHandler> logger) : base(logger)
        {
            _broker = broker;
            _orderRepository = orderRepository;
        }

        public override async Task HandleAsync(PlaceOrder command)
        {
            await base.LogHandleAsync(command);
            if (command.ProductId > 0)
            {
                await _orderRepository.AddAsync(new Order(command.Id, command.ClientId, command.ProductId,
                    command.Price, placedAt: command.PlacedAt));
                await _broker.PublishAsync(new OrderCreated(command.Id, command.ClientId, command.ProductId,
                    command.Price));
            }
            else
            {
                await _broker.PublishAsync(new CreateNewOrderFailed(command.Id,
                    "Failed on PlaceOrder validation. Invalid (negative) product ID."));
            }
        }
    }
}