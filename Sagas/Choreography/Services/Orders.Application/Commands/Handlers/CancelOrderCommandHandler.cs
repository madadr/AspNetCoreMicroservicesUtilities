using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Common.Application.EventBus;
using Common.Application.Events;
using Orders.Application.Events;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Application.Commands.Handlers
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrder>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(CancelOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            await _orderRepository.UpdateAsync(new Order(order.OrderId, order.CustomerId, order.ProductId, order.ProductId, OrderState.Cancelled, command.Reason));
        }
    }
}