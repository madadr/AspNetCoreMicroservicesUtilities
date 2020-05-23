using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Orders.Application.Events;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Application.Commands.Handlers
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrder>
    {
        private readonly IMessageBroker _broker;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderCommandHandler(IMessageBroker broker, IOrderRepository orderRepository)
        {
            _broker = broker;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(PlaceOrder command)
        {
            if (command.ProductId > 0)
            {
                await _orderRepository.AddAsync(new Order(command.Id, command.CustomerId, command.ProductId, command.Price));
                await _broker.PublishAsync(new OrderCreated(command.Id, command.CustomerId, command.ProductId,
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