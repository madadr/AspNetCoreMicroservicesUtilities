using System.Threading.Tasks;
using Common.Application.Commands.Handlers;
using Microsoft.Extensions.Logging;
using Orders.Core.Entities;
using Orders.Core.Repositories;

namespace Orders.Application.Commands.Handlers
{
    public class CancelOrderCommandHandler : CommandHandlerBase<CancelOrder>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository,
            ILogger<CancelOrderCommandHandler> logger) : base(logger)
        {
            _orderRepository = orderRepository;
        }

        public override async Task HandleAsync(CancelOrder command)
        {
            await base.LogHandleAsync(command);
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order != null)
            {
                order.State = OrderState.Cancelled;
                order.CancelledAt = command.CancelledAt;
                order.AdditionalInfo +=
                    $"{(order.AdditionalInfo.Length > 0 ? "; " : "")}Cancellation reason: {command.Reason}";
                await _orderRepository.UpdateAsync(order);
            }
            
        }
    }
}